namespace Core.Sagas.Timeouts;

using NServiceBus;

public class MySagaData :
    ContainSagaData
{
    public string SomeId { get; set; }

    public bool Message2Arrived { get; set; }
}