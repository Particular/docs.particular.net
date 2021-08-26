using NServiceBus;
#region V1Message
namespace Versioning.Contracts
{
    public interface ISomethingHappened :
        IEvent
    {
        int SomeData { get; set; }
    }
}
#endregion