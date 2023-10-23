using System;

namespace TwitchChatbot
{
    static internal class MessageParser
    {
        static public void Parse(string message, string channel)
        {
            const string CONNECTED_MESSAGE_FORMAT = "tmi.twitch.tv 001";
        
            if (message.Contains(CONNECTED_MESSAGE_FORMAT))
            {
                Console.Clear();
                Console.WriteLine($"Connected! Now chatting in #{channel}");
                Console.WriteLine("");
            }
            else if (message.Contains("PRIVMSG"))
            {
                string privMsg = ParsePrivMsg(message);
                Console.WriteLine(privMsg);
            }
        }
        static private string ParsePrivMsg(string chatMessage)
        {
            string messageSender = "";
            string messageContent = "";

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
                }
            }

            // Concatenates the final string for returning
            return messageSender + ": " + messageContent;
        }
    }
}