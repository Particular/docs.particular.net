namespace Contracts
{
    using NServiceBus;

    #region V1Message

    public interface ISomethingHappened : IEvent
    {
        int SomeData { get; set; }
    }

    #endregion
}
