using System;
using NServiceBus;

public class ProcessInvalidOrderCommandHandler :
    IHandleMessages<PlaceInvalidOrder>
{
    public void Handle(PlaceInvalidOrder placeOrder)
    {
        throw new Exception("Unexpected failure.");
    }
}
