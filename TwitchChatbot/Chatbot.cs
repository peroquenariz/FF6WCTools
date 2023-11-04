using System;
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

    private const string TWITCH_IRC_PING_MESSAGE = "PING :tmi.twitch.tv";
    private const string TWITCH_IRC_PONG_MESSAGE = "PONG :tmi.twitch.tv";

    private readonly string _nick;
    private readonly string _oauth;
    private readonly string _channel;
    //private readonly int _retryCounter;

    private List<Message> _crowdControlMessageQueue;

    public List<Message> CrowdControlMessageQueue => _crowdControlMessageQueue;

    public Chatbot(NameValueCollection config)
    {
        _nick = config.Get("nick")!;
        _oauth = config.Get("oauth")!;
        _channel = config.Get("channel")!;

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

        using TcpClient chatbot = new TcpClient(TWITCH_IRC_ADDRESS, TWITCH_IRC_PORT);
        using NetworkStream networkStream = chatbot.GetStream();
        using StreamReader reader = new StreamReader(networkStream);
        using StreamWriter writer = new StreamWriter(networkStream);
        writer.AutoFlush = true;
        
        // Request Twitch Capabilities
        writer.WriteLine("CAP REQ : twitch.tv/commands twitch.tv/membership twitch.tv/tags");

        // Send credentials
        // TODO: check if login failed, notify the user and exit app.
        writer.WriteLine($"PASS oauth:{_oauth}");
        writer.WriteLine($"NICK {_nick}");
        
        // Join a channel
        writer.WriteLine($"JOIN #{_channel},#{_channel}");

        // Test message
        writer.WriteLine($"PRIVMSG #{_channel} :test :)");

        try
        {
            while (true)
            {
                string? line = reader.ReadLine();
#if DEBUG
                Console.WriteLine(line);
#endif
                if (line != null)
                {
                    if (line == TWITCH_IRC_PING_MESSAGE)
                    {
                        writer.WriteLine(TWITCH_IRC_PONG_MESSAGE); // Keepalive message
                        writer.Flush();
                    }
                    else
                    {
                        Message? chatMessage = MessageParser.Parse(line);
                        
                        if (chatMessage != null)
                        {
                            if (chatMessage.IsCrowdControlMessage)
                            {
                                _crowdControlMessageQueue.Add(chatMessage);
                                await Console.Out.WriteLineAsync(chatMessage.Content);
                            }

                            if (chatMessage.IsConnectionSuccessfulMessage)
                            {
                                OnIRCConnectionSuccessful?.Invoke(this, new IRCConnectionSuccesfulEventArgs(_channel));
                            }
                        }
                    }
                }
            }
        }
        catch
        {
            Console.WriteLine("Error trying to connect! Retrying in 5 seconds..."); // TODO: make an event and subscribe with consoleviewer
            await Task.Delay(5000);
        }
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