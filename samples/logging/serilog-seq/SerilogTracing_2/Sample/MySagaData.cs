using NServiceBus.Saga;

public class MySagaData :
    ContainSagaData
{
    public string UserName { get; set; }
}