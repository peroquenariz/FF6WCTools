namespace TwitchChatbot;

/// <summary>
/// Provides methods to handle Twitch IRC messages.
/// </summary>
static internal class MessageParser
{
    private const string LOGIN_SUCCESFUL_MESSAGE_FORMAT = "tmi.twitch.tv 001";
    private const string LOGIN_FAILED_MESSAGE_FORMAT = ":tmi.twitch.tv NOTICE * :Login authentication failed";
    private const string JOINED_CHANNEL_MESSAGE_FORMAT = "tmi.twitch.tv JOIN #";
    private const string PRIVMSG_MESSAGE_FORMAT = "PRIVMSG";

    private const string CROWD_CONTROL_COMMAND_TRIGGER = "!";

    public static Message? Parse(string message)
    {
        Message? chatMessage = null;

        if (message.Contains(LOGIN_SUCCESFUL_MESSAGE_FORMAT)) // TODO: make this an event and subscribe to it with consoleviewer
        {
            chatMessage = new Message(Message.MessageType.LOGIN_SUCCESSFUL);
        }
        else if (message.Contains(JOINED_CHANNEL_MESSAGE_FORMAT))
        {
            chatMessage = new Message(Message.MessageType.CONNECTED_TO_CHANNEL);
        }
        else if (message.Contains(LOGIN_FAILED_MESSAGE_FORMAT))
        {
            chatMessage = new Message(Message.MessageType.LOGIN_FAILED);
        }
        else if (message.Contains(PRIVMSG_MESSAGE_FORMAT))
        {
            chatMessage = ParsePrivateMessage(message);
        }

        return chatMessage;
    }

    private static Message? ParsePrivateMessage(string chatMessage)
    {
        string messageSender = "";
        string messageContent = "";
        bool isCrowdControlMessage = false;

        // Split the message into an array
        string[] splitChatMessage = chatMessage.Split(";");

        for (int i = 0; i < splitChatMessage.Length; i++)
        {
            string section = splitChatMessage[i];

            // Grabs displayed name for the user
            if (section.Contains("display-name="))
            {
                messageSender = section[(section.IndexOf("=") + 1)..];
            }

            // Grabs the content of the message
            else if (section.Contains("PRIVMSG #"))
            {
                // Make a new string after the "#"
                string messageSection = section[section.IndexOf("#")..];
                // Everything after the colon is guaranteed to be the message content
                messageContent = messageSection[(messageSection.IndexOf(":") + 1)..];

                // Check if it's a crowd control message
                isCrowdControlMessage = messageContent.ToLower().StartsWith(CROWD_CONTROL_COMMAND_TRIGGER);
                
                if (!isCrowdControlMessage)
                {
                    // Ignore if message doesn't start with the command trigger.
                    return null;
                }
                else if (messageContent.Length == CROWD_CONTROL_COMMAND_TRIGGER.Length)
                {
                    // Ignore if the trigger is correct but no command has been provided.
                    return null;
                }
                else
                {
                    // Save the crowd control message.
                    messageContent = messageContent[CROWD_CONTROL_COMMAND_TRIGGER.Length..].Trim();
                }
            }
        }

        // Creates the message
        return new Message(Message.MessageType.CROWD_CONTROL_MESSAGE, messageContent, messageSender);
    }
}