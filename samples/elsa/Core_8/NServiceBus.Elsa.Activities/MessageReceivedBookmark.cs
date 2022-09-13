using Elsa.Services;

namespace NServiceBus.Activities
{
  internal class MessageReceivedBookmark : IBookmark
  {
    public MessageReceivedBookmark(string messageType)
    {
      MessageType = messageType;
    }

    public string MessageType
    {
      get;
      set;
    }
  }
}
