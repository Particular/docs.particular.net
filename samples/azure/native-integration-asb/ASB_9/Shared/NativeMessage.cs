using System;
using NServiceBus;

#region NativeMessage

public class NativeMessage :
    IMessage
{
    public string Content { get; set; }
    public DateTime SendOnUtc { get; set; }
}

#endregion
