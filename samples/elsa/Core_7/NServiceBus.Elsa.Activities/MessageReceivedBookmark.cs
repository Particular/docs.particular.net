using Elsa.Services;

namespace NServiceBus.Activities
{
    /// <summary>
    /// Elsa uses bookmarks to distinguish between triggers of the same Activity type.
    /// In this case, this bookmark will allow Elsa to distinguish between received messages of different types
    /// </summary>
    internal class MessageReceivedBookmark : IBookmark
    {
        public MessageReceivedBookmark(string? messageType)
        {
            MessageType = messageType;
        }

        public string? MessageType
        {
            get;
            set;
        }
    }
}
