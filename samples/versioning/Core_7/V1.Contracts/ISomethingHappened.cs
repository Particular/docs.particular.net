using NServiceBus;

namespace Versioning.Contracts
{
    public interface ISomethingHappened :
        IEvent
    {
        int SomeData { get; set; }
    }
}