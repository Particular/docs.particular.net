using System;
using NServiceBus;

public class CreateOrderPhase1 :
    IMessage
{
    public DateTime OrderDate { get; set; }
}
