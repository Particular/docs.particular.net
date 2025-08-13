namespace Core9.Sagas.FindByProperty
{
    using NServiceBus;

    public class MySagaData :
        ContainSagaData
    {
        public string SomeId { get; set; }
    }
}