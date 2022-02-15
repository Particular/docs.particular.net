namespace Core7.Sagas.ReverseMapping
{
    using NServiceBus;

    public class MySecondMessage : IMessage
    {
        public string SomeOtherId { get; set; }
    }
}