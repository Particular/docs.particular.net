
using System;
using NServiceBus;
#region StepByStep-PlaceOrder
public class PlaceOrder : ICommand
{
    public Guid Id { get; set; }

    public string Product { get; set; }
}

#endregion