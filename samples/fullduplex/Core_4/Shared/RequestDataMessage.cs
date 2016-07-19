using System;
using NServiceBus;

#region RequestMessage
public class RequestDataMessage :
    IMessage
{
    public Guid DataId { get; set; }
    public string String { get; set; }
}
#endregion