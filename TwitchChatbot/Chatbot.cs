﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TwitchChatbot;

public class Chatbot
{
    public event EventHandler<IRCConnectionSuccesfulEventArgs>? OnIRCConnectionSuccessful;
    
    private const string TWITCH_IRC_ADDRESS = "irc.chat.twitch.tv";
    private const int TWITCH_IRC_PORT = 6667;

    TcpClient _chatbot;
    NetworkStream _networkStream;
    private StreamReader _reader;
    private StreamWriter _writer;

    private const string TWITCH_IRC_PING_MESSAGE = "PING :tmi.twitch.tv";
    private const string TWITCH_IRC_PONG_MESSAGE = "PONG :tmi.twitch.tv";

    private readonly string _nick;
    private readonly string _oauth;
    private readonly string _channel;

    private bool _isConnectedToChannel;
    private bool _isLoginValid;

    private readonly List<Message> _crowdControlMessageQueue;

    public List<Message> CrowdControlMessageQueue => _crowdControlMessageQueue;

    public Chatbot(NameValueCollection config)
    {
        _nick = config.Get("nick")!;
        _oauth = config.Get("oauth")!;
        _channel = config.Get("channel")!;

        _chatbot = new TcpClient(TWITCH_IRC_ADDRESS, TWITCH_IRC_PORT);
        _networkStream = _chatbot.GetStream();
        _reader = new StreamReader(_networkStream);
        _writer = new StreamWriter(_networkStream);
        _writer.AutoFlush = true;

        _crowdControlMessageQueue = new List<Message>();
    }

    public async Task StartAsync()
    {
        await Task.Delay(1);

        // Check if config file is properly set up.
        // TODO: make event to notify the user through console.
        if (string.IsNullOrWhiteSpace(_nick) ||
            string.IsNullOrWhiteSpace(_oauth) ||
            string.IsNullOrWhiteSpace(_channel))
        {
            return;
        }
        
        // Request Twitch Capabilities
        _writer.WriteLine("CAP REQ : twitch.tv/commands twitch.tv/membership twitch.tv/tags");

        // Send credentials
        _writer.WriteLine($"PASS oauth:{_oauth}");
        _writer.WriteLine($"NICK {_nick}");

        // Join a channel
        _writer.WriteLine($"JOIN #{_channel},#{_channel}");

        try
        {
            while (true)
            {
                string? line = _reader.ReadLine();
#if DEBUG
                await Console.Out.WriteLineAsync(line);
#endif
                if (line != null)
                {
                    if (line == TWITCH_IRC_PING_MESSAGE)
                    {
                        _writer.WriteLine(TWITCH_IRC_PONG_MESSAGE); // Keepalive message
                    }
                    else
                    {
                        Message? chatMessage = MessageParser.Parse(line);
                        
                        if (chatMessage != null)
                        {
                            if (chatMessage.Type == Message.MessageType.CROWD_CONTROL_MESSAGE)
                            {
                                _crowdControlMessageQueue.Add(chatMessage);
                            }
                            else if (!_isConnectedToChannel && chatMessage.Type == Message.MessageType.CONNECTED_TO_CHANNEL)
                            {
                                OnIRCConnectionSuccessful?.Invoke(this, new IRCConnectionSuccesfulEventArgs(_channel));
                                _isConnectedToChannel = true;
                            }
                            else if (!_isLoginValid)
                            {
                                if (chatMessage.Type == Message.MessageType.LOGIN_SUCCESSFUL)
                                {
                                    _isLoginValid = true;
                                }
                                else if (chatMessage.Type == Message.MessageType.LOGIN_FAILED)
                                {
                                    await Console.Out.WriteLineAsync("Chatbot login failed! Please check your credentials and try again.");
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            await Console.Out.WriteLineAsync(e.ToString());
            await Console.Out.WriteLineAsync("Error trying to connect! Retrying in 5 seconds..."); // TODO: make an event and subscribe with consoleviewer
            await Task.Delay(5000); // TODO: handle reconnections
        }
    }

    /// <summary>
    /// Writes a message in chat.
    /// </summary>
    /// <param name="message">The message to write.</param>
    public void WriteMessage(string message)
    {
        _writer.WriteLine($"PRIVMSG #{_channel} :{message}");
    }

    public void CrowdControl_OnSuccessfulEffectLoaded(object? sender, MessageEventArgs e)
    {
        WriteMessage($"@{e.User} {e.Message}");
    }

    public void CrowdControl_OnFailedEffect(object? sender, MessageEventArgs e)
    {
        WriteMessage($"@{e.User} {e.Message}");
    }
}

public class IRCConnectionSuccesfulEventArgs
{
    public string Channel { get; }
    public IRCConnectionSuccesfulEventArgs(string channel)
    {
        Channel = channel;
    }
}

public class MessageEventArgs
{
    public string User { get; set; }
    public string Message { get; set; }

    public MessageEventArgs(string user, string message)
    {
        User = user;
        Message = message;
    }
}