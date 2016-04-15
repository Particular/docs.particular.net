namespace Snippets6.Sagas.Reply
{
    using NServiceBus;

    public class MySagaData : ContainSagaData
    {
        public string SomeID { get; set; }

        public bool Message2Arrived { get; set; }
    }
}