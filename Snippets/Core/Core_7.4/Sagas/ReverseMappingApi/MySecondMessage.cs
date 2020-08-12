namespace Core7.Sagas.ReverseMapping
{
    using NServiceBus;

    public class MySecondMessage : IMessage
    {
        public int SomeOtherId { get; set; }
    }
}