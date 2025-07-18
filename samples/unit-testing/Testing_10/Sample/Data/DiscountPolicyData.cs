using System;
using NServiceBus;

public class DiscountPolicyData :
    ContainSagaData
{
    public Guid CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
}