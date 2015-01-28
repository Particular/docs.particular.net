using NServiceBus;
#region V1Message
namespace V1.Messages
{
    public interface ISomethingHappened : IMessage
    {
        int SomeData { get; set; }
    }
}
#endregion