using System;
using System.Collections.Generic;
using TwitchChatbot;
using static CrowdControlLib.CrowdControlEffects;

namespace CrowdControlLib;

/// <summary>
/// Handles all Crowd Control command execution.
/// </summary>
internal class CommandHandler
{
    public const string CURRENCY_NAME = "GP";
    public const int STARTING_CURRENCY_AMOUNT = 100;
    
    public event EventHandler<MessageEventArgs>? OnSuccessfulEffectLoaded; // TODO: decouple from TwitchChatbot!
    public event EventHandler<MessageEventArgs>? OnFailedEffect;
    
    // Commands dictionary.
    private readonly Dictionary<Effect, Action<CrowdControlArgs>> _commands;
    
    /// <summary>
    /// Currency amount for each user in chat.
    /// Key: Twitch user; Value: fake currency
    /// </summary>
    private readonly Dictionary<string, int> _wallets;

    public CommandHandler(Dictionary<Effect, Action<CrowdControlArgs>> commands)
    {
        _commands = commands;
        _wallets = new Dictionary<string, int>();
    }

    /// <summary>
    /// Attempts to load a crowd control effect.
    /// </summary>
    /// <returns>True if an effect was successfully loaded, otherwise false.</returns>
    internal bool TryLoadEffect(CrowdControlMessage message)
    {
        bool wasLoaded = false;
        
        // Create the arguments from the Crowd Control message.
        CrowdControlArgs args = new(message);

        // Return if command arguments are not valid.
        if (!args.IsValid)
        {
            OnFailedEffect?.Invoke(this, new MessageEventArgs(message.User, args.ErrorMessage));
        }
        else
        {
            // TODO: Check if the command is in cooldown.

            // Check if user has enough currency to buy the effect.
            if (!_wallets.ContainsKey(message.User))
            {
                // If user isn't in the currency list/dict, add it and assign a starting amount.
                _wallets.Add(message.User, STARTING_CURRENCY_AMOUNT);
            }

            if (args.Cost > _wallets[message.User])
            {
                OnFailedEffect?.Invoke(this, new MessageEventArgs(message.User,
                    $"Not enough money! Cost: {args.Cost}{CURRENCY_NAME}, wallet: {_wallets[message.User]}{CURRENCY_NAME}"));
            }
            else
            {
                _wallets[message.User] -= args.Cost;
                
                _commands.TryGetValue(args.EffectType, out Action<CrowdControlArgs>? command);
                command?.Invoke(args); // TODO: show more detailed effect messages.

                OnSuccessfulEffectLoaded?.Invoke(this, new MessageEventArgs(message.User,
                    $"Successful! {_wallets[message.User]} {CURRENCY_NAME} remaining.")); // TODO: send more detailed effect info!
                
                wasLoaded = true;
            }
        }

        return wasLoaded;
    }
}