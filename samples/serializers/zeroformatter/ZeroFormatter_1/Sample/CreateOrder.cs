using NServiceBus;
using ZeroFormatter;

#region message
[ZeroFormattable]
public class CreateOrder :
    IMessage
{
    [Index(0)]
    public virtual int OrderId { get; set; }

    [Index(1)]
    public virtual long Date { get; set; }

    [Index(2)]
    public virtual int CustomerId { get; set; }
}
#endregion