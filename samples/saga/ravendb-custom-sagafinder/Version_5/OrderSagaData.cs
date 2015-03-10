using System;
using NServiceBus.Saga;

#region OrderSagaDataRavenDB
public class OrderSagaData : IContainSagaData
{
	public Guid Id { get; set; }
	public string Originator { get; set; }
	public string OriginalMessageId { get; set; }

	[Raven.Client.UniqueConstraints.UniqueConstraint( CaseInsensitive = true )]
	public string OrderId { get; set; }

	[Raven.Client.UniqueConstraints.UniqueConstraint( CaseInsensitive = true )]
	public string PaymentTransactionId { get; set; }
}
#endregion