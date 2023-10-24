namespace TwitchChatbot
{
    public class Message
    {
        // Twitch PRIVMSG template:
        //@badge-info=subscriber/68;badges=broadcaster/1,subscriber/0;client-nonce=ad7e319773bbe89baa7ffc6311e233e2;color=#D2691E;display-name=peroquenariz;emotes=;first-msg=0;flags=;id=f8430ca8-4cea-4ccd-b15c-5a605c9671f8;mod=0;returning-chatter=0;room-id=31186962;subscriber=1;tmi-sent-ts=1698101685499;turbo=0;user-id=31186962;user-type= :peroquenariz!peroquenariz@peroquenariz.tmi.twitch.tv PRIVMSG #peroquenariz :test
        private string _content;
        private string _displayName;
        private bool _isCrowdControlMessage;

        public string Content => _content;
        public string DisplayName => _displayName;
        public bool IsCrowdControlMessage => _isCrowdControlMessage;

        public Message(string content, string displayName, bool isCrowdControlMessage)
        {
            _content = content;
            _displayName = displayName;
            _isCrowdControlMessage = isCrowdControlMessage;
        }
    }
}
