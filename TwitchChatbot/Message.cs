namespace TwitchChatbot
{
    public class Message
    {
        // Twitch PRIVMSG template:
        //@badge-info=subscriber/68;badges=broadcaster/1,subscriber/0;client-nonce=ad7e319773bbe89baa7ffc6311e233e2;color=#D2691E;display-name=peroquenariz;emotes=;first-msg=0;flags=;id=f8430ca8-4cea-4ccd-b15c-5a605c9671f8;mod=0;returning-chatter=0;room-id=31186962;subscriber=1;tmi-sent-ts=1698101685499;turbo=0;user-id=31186962;user-type= :peroquenariz!peroquenariz@peroquenariz.tmi.twitch.tv PRIVMSG #peroquenariz :test
        public enum MessageType
        {
            LOGIN_SUCCESSFUL,
            LOGIN_FAILED,
            CONNECTED_TO_CHANNEL,
            CROWD_CONTROL_MESSAGE
        }
        
        private MessageType _type;
        private string _content;
        private string _displayName;

        public MessageType Type => _type;
        public string Content => _content;
        public string DisplayName => _displayName;

        public Message(MessageType type, string content = "", string displayName = "")
        {
            _type = type;
            _content = content;
            _displayName = displayName;
        }
    }
}
