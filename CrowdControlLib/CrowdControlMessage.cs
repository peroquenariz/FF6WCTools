namespace CrowdControlLib
{
    /// <summary>
    /// Represents a Crowd Control message.
    /// </summary>
    internal class CrowdControlMessage
    {
        public string User { get; private set; }
        public string Content { get; private set; }

        public CrowdControlMessage(string user, string message)
        {
            User = user;
            Content = message;
        }
    }
}