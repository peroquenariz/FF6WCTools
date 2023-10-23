using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Threading.Tasks;

namespace TwitchChatbot
{
    public class Chatbot
    {
        private const string TWITCH_IRC_ADDRESS = "irc.chat.twitch.tv";
        private const int TWITCH_IRC_PORT = 6667;

        private readonly string _nick;
        private readonly string _oauth;
        private readonly string _channel;
        //private readonly int _retryCounter;

        public Chatbot(NameValueCollection config)
        {
            _nick = config.Get("nick")!;
            _oauth = config.Get("oauth")!;
            _channel = config.Get("channel")!;
        }

        public async Task StartAsync()
        {
            await Task.Delay(1);

            using TcpClient chatbot = new TcpClient(TWITCH_IRC_ADDRESS, TWITCH_IRC_PORT);
            using NetworkStream networkStream = chatbot.GetStream();
            using StreamReader reader = new StreamReader(networkStream);
            using StreamWriter writer = new StreamWriter(networkStream);

            // Request Twitch Capabilities
            writer.WriteLine("CAP REQ : twitch.tv/commands twitch.tv/membership twitch.tv/tags");
            writer.Flush();
            
            // Send credentials
            writer.WriteLine($"PASS oauth:{_oauth}");
            writer.Flush();
            writer.WriteLine($"NICK {_nick}");
            writer.Flush();
            
            // Join a channel
            writer.WriteLine($"JOIN #{_channel},#{_channel}");
            writer.Flush();

            //// Test message
            //writer.WriteLine($"PRIVMSG #peroquenariz :test :)");
            //writer.Flush();

            try
            {
                while (true)
                {
                    string? line = reader.ReadLine();
                    //Console.WriteLine(line);
                    
                    if (line != null)
                    {
                        if (line == "PING :tmi.twitch.tv")
                        {
                            writer.WriteLine("PONG :tmi.twitch.tv"); // Keepalive message
                            writer.Flush();
                            Console.WriteLine("PONG SENT!!!!");
                        }
                        else
                        {
                            MessageParser.Parse(line, _channel);
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error trying to connect! Retrying in 5 seconds...");
                await Task.Delay(5000);
            }
        }
    }
}