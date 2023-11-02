namespace Testing_9.Saga;

using NServiceBus;

public class MySagaData :
    ContainSagaData
{
    public string MyId { get; set; }
}