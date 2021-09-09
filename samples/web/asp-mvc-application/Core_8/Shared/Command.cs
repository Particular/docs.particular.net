using System;
using NServiceBus;

[Serializable]
public class Command :
    IMessage
{
    public int Id { get; set; }
}
