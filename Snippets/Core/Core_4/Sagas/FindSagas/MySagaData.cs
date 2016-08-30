namespace Core4.Sagas.FindSagas
{
    using NServiceBus.Saga;

    public class MySagaData :
        ContainSagaData
    {
        public string SomeId { get; set; }
        public string SomeData { get; set; }
    }
}