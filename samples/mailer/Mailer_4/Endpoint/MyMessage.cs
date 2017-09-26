using System;
using NServiceBus;

public class MyMessage:IMessage
{
    public Guid Number { get; set; }
}