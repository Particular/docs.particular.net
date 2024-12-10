using NServiceBus;
using System;
#region ResponseMessage
public class DataResponseMessage :
    IMessage
{
    public Guid DataId { get; set; }
    public string String { get; set; }
}
#endregion
