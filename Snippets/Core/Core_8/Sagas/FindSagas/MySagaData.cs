namespace Core8.Sagas.FindSagas
{
    using NServiceBus;

    public class MySagaData :
        ContainSagaData
    {
        public string SomeId { get; set; }
        public string SomeData { get; set; }
    }
}