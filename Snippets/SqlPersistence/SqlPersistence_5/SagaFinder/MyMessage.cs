namespace SagaFinder
{
    using NServiceBus;

    public class MyMessage :
        IMessage
    {
        public string PropertyValue { get; set; }
    }
}