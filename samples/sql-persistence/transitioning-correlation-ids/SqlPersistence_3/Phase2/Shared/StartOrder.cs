using System;
using NServiceBus;

#region messagePhase2
public class StartOrder :
    IMessage
{
    public int OrderNumber { get; set; }
    public Guid OrderId { get; set; }
}
#endregion