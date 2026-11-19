using System;
using NServiceBus;

#region Message
public class ProcessOrder : ICommand
{
    public Guid OrderId { get; set; }
}
#endregion
