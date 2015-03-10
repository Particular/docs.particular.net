using NServiceBus;
using System;

class IssuePaymentRequest : IMessage
{
	public String PaymentTransactionId { get; set; }
}