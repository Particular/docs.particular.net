using System;
using NServiceBus;

public class CancelOrder : ICommand
{
    public Guid OrderId { get; set; }
}
