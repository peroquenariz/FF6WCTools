using System;
using System.Collections.Generic;
using TwitchChatbot;

namespace CrowdControlLib;

public class CommandHandler
{
    public event EventHandler<MessageEventArgs>? OnSuccessfulEffectLoaded;
    public event EventHandler<MessageEventArgs>? OnFailedEffect;
    
    private readonly List<Message> _crowdControlMessageQueue;
    private readonly Dictionary<Effect, Action<CrowdControlArgs>> _commands;

    public CommandHandler(Dictionary<Effect, Action<CrowdControlArgs>> commands, List<Message> crowdControlMessageQueue)
    {
        _commands = commands;
        _crowdControlMessageQueue = crowdControlMessageQueue;
    }

    /// <summary>
    /// Removes the first message in the queue and attempts to load it.
    /// </summary>
    /// <returns>True if an effect was successfully loaded, otherwise false.</returns>
    internal bool TryLoadEffect()
    {
        // Return if queue is empty.
        if (_crowdControlMessageQueue.Count == 0) return false;

        // Take the oldest message in the queue.
        Message oldestMessage = _crowdControlMessageQueue[0];
        
        // Get username.
        string username = oldestMessage.DisplayName;

        CrowdControlArgs args = new CrowdControlArgs(oldestMessage.Content);
        _crowdControlMessageQueue.RemoveAt(0);

        // Return if command arguments are not valid.
        if (!args.IsValid)
        {
            OnFailedEffect?.Invoke(this, new MessageEventArgs(username, args.ErrorMessage));
            return false;
        }

        _commands.TryGetValue(args.EffectType, out Action<CrowdControlArgs>? command);
        command?.Invoke(args);
        
        OnSuccessfulEffectLoaded?.Invoke(this, new MessageEventArgs(username, "Successful!")); // TODO: send more detailed effect info!
        return true;
    }
}