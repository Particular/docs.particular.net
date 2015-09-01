using System;
using NServiceBus;

#region OrderPlaced
public class OrderPlaced : IEvent
{
    public Guid OrderId { get; set; }
}
#endregion