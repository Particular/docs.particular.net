namespace Core4.Sagas.FindSagas
{
    using NServiceBus.Saga;

    public class MySagaData :
        ContainSagaData
    {
        public string SomeID { get; set; }
        public string SomeData { get; set; }
    }
}