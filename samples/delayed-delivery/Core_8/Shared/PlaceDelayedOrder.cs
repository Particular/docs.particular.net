
using System;
using NServiceBus;
#region PlaceDelayedOrder
public class PlaceDelayedOrder :
    ICommand
{
    public Guid Id { get; set; }
    public string Product { get; set; }
}

#endregion