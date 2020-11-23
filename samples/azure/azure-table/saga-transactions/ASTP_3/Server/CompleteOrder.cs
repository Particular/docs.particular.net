using System;

public class CompleteOrder : IProvideOrderId
{
    public Guid OrderId { get; set; }
    public string OrderDescription { get; set; }
}