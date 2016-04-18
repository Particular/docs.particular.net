namespace Core6.Sagas.FindByExpression
{
    using NServiceBus;

    public class MySagaData : ContainSagaData
    {
        public string SomeID { get; set; }
    }
}