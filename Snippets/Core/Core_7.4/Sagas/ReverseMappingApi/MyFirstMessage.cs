namespace Core7.Sagas.ReverseMapping
{
    using NServiceBus;

    public class MyFirstMessage : IMessage
    {
        public int SomeId { get; set; }
    }
}