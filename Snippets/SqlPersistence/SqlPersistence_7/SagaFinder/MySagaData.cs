namespace SagaFinder
{
    using NServiceBus;

    public class MySagaData :
        ContainSagaData
    {
        public string OrderId { get; set; }
        public string PaymentTransactionId { get; set; }
    }
}