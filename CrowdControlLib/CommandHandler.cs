using System;
using System.Collections.Generic;
using TwitchChatbot;
using static CrowdControlLib.CrowdControlEffects;

namespace CrowdControlLib;

public class CommandHandler
{
    public event EventHandler<MessageEventArgs>? OnSuccessfulEffectLoaded; // TODO: decouple from TwitchChatbot!
    public event EventHandler<MessageEventArgs>? OnFailedEffect;
    
    private readonly Dictionary<Effect, Action<CrowdControlArgs>> _commands;

    public CommandHandler(Dictionary<Effect, Action<CrowdControlArgs>> commands)
    {
        _commands = commands;
    }

    /// <summary>
    /// Removes the first message in the queue and attempts to load it.
    /// </summary>
    /// <returns>True if an effect was successfully loaded, otherwise false.</returns>
    internal bool TryLoadEffect(CrowdControlMessage message)
    {
        CrowdControlArgs args = new CrowdControlArgs(message.Content);

        // Return if command arguments are not valid.
        if (!args.IsValid)
        {
            OnFailedEffect?.Invoke(this, new MessageEventArgs(message.User, args.ErrorMessage));
            return false;
        }

        _commands.TryGetValue(args.EffectType, out Action<CrowdControlArgs>? command);
        command?.Invoke(args); // TODO: show more detailed effect messages.
        
        OnSuccessfulEffectLoaded?.Invoke(this, new MessageEventArgs(message.User, "Successful!")); // TODO: send more detailed effect info!
        return true;
    }
}