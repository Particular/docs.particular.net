using System;
using NServiceBus;

#region StepByStep-OrderPlaced
public class OrderPlaced : IEvent
{
    public Guid OrderId { get; set; }
}
#endregion