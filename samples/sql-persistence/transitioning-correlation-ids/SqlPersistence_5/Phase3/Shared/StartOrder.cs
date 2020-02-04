using System;
using NServiceBus;

#region messagePhase3
public class StartOrder :
    IMessage
{
    public Guid OrderId { get; set; }
}
#endregion