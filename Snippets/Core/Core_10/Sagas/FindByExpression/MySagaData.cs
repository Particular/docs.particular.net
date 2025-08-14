namespace Core.Sagas.FindByExpression;

using NServiceBus;

public class MySagaData :
    ContainSagaData
{
    public string SomeId { get; set; }
}