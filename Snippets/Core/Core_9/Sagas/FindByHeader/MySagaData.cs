namespace Core9.Sagas.FindByHeader
{
    using NServiceBus;

    public class MySagaData : ContainSagaData
    {
        public string SomeId { get; set; }
    }
}
