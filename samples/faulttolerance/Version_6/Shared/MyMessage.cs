using System;
using System.Diagnostics;
using NServiceBus;

public class MyMessage : IMessage
{
    public Guid Id { get; set; }

    public MyMessage()
    {
        Debug.WriteLine("sdfsdf");
    }
}