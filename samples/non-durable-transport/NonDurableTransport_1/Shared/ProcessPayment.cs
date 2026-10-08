using System;
using NServiceBus;

public class ProcessPayment : ICommand
{
    public Guid OrderId { get; set; }
}
