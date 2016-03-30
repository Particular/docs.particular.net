using System;
using NServiceBus;

public class DiscountPolicyData : IContainSagaData
{
    public Guid Id { get; set; }
    public string Originator { get; set; }
    public string OriginalMessageId { get; set; }

    public Guid CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
}
