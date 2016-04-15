namespace Core6.Sagas.FindSagas
{
    using NServiceBus;

    public class MySagaData : ContainSagaData
    {
        public string SomeID { get; set; }
        public string SomeData { get; set; }
    }
}