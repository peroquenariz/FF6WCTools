﻿using System;

namespace TwitchChatbot
{
    static internal class MessageParser
    {
        const string CONNECTED_MESSAGE_FORMAT = "tmi.twitch.tv 001";
        const string PRIVMSG_MESSAGE_FORMAT = "PRIVMSG";

        const string CROWD_CONTROL_COMMAND_TRIGGER = "!cc";

        static public Message? Parse(string message, string channel)
        {
            Message? chatMessage = null;

            if (message.Contains(CONNECTED_MESSAGE_FORMAT)) // TODO: make this an event and subscribe to it with consoleviewer
            {
                //Console.WriteLine($"Connected! Now chatting in #{channel}");
                Console.WriteLine("");
            }
            else if (message.Contains(PRIVMSG_MESSAGE_FORMAT))
            {
                chatMessage = ParsePrivateMessage(message);
            }

            return chatMessage;
        }

        static private Message ParsePrivateMessage(string chatMessage)
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

                    isCrowdControlMessage = messageContent.StartsWith(CROWD_CONTROL_COMMAND_TRIGGER);
                }
            }

            // Concatenates the final string for returning
            return new Message(messageContent, messageSender, isCrowdControlMessage);
        }
    }
}