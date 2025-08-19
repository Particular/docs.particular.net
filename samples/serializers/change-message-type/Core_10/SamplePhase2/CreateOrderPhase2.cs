using System;
using NServiceBus;

public class CreateOrderPhase2 :
    IMessage
{
    public DateTime OrderDate { get; set; }
}