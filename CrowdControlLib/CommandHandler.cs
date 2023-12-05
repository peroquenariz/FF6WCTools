using System;
using System.Collections.Generic;
using TwitchChatbot;
using static CrowdControlLib.CrowdControlEffects;

namespace CrowdControlLib;

/// <summary>
/// Handles all Crowd Control command execution.
/// </summary>
public class CommandHandler
{
    public event EventHandler<MessageEventArgs>? OnSuccessfulEffectLoaded; // TODO: decouple from TwitchChatbot!
    public event EventHandler<MessageEventArgs>? OnFailedEffect;
    
    // Commands dictionary.
    private readonly Dictionary<Effect, Action<CrowdControlArgs>> _commands;

    public CommandHandler(Dictionary<Effect, Action<CrowdControlArgs>> commands)
    {
        _commands = commands;
    }

    /// <summary>
    /// Attempts to load a crowd control effect.
    /// </summary>
    /// <returns>True if an effect was successfully loaded, otherwise false.</returns>
    internal bool TryLoadEffect(CrowdControlMessage message)
    {
        // Create the arguments from the Crowd Control message.
        CrowdControlArgs args = new CrowdControlArgs(message.Content);

        // Return if command arguments are not valid.
        if (!args.IsValid)
        {
            OnFailedEffect?.Invoke(this, new MessageEventArgs(message.User, args.ErrorMessage));
            return false;
        }

        // If valid, execute the command.
        _commands.TryGetValue(args.EffectType, out Action<CrowdControlArgs>? command);
        command?.Invoke(args); // TODO: show more detailed effect messages.
        
        // Notify the user in chat.
        OnSuccessfulEffectLoaded?.Invoke(this, new MessageEventArgs(message.User, "Successful!")); // TODO: send more detailed effect info!
        return true;
    }
}