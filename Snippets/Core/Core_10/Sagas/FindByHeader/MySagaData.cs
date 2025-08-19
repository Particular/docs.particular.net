namespace Core.Sagas.FindByHeader;

using NServiceBus;

public class MySagaData : ContainSagaData
{
    public string SomeId { get; set; }
}