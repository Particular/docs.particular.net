using NServiceBus;

public interface PaymentTransactionCompleted :
    IEvent
{
	string PaymentTransactionId { get; set; }
}
