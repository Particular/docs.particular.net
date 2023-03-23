namespace Core7.Sagas.ReverseMapping
{
    using NServiceBus;

    public class MySagaData : ContainSagaData
    {
        public string SomeId { get; set; }
    }
}
