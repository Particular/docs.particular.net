namespace Core7.Sagas.ReverseMapping
{
    using NServiceBus;

    public class MyFirstMessage : IMessage
    {
        public string SomeId { get; set; }
    }
}