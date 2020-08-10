namespace Core8.Sagas.Reply
{
    using NServiceBus;

    public class MySagaData :
        ContainSagaData
    {
        public string SomeId { get; set; }

        public bool Message2Arrived { get; set; }
    }
}