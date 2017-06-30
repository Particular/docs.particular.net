using Bond;
using NServiceBus;

#region message
[Schema]
public class CreateOrder :
    IMessage
{
    [Id(0)]
    public int OrderId { get; set; }

    [Id(1)]
    public long Date { get; set; }

    [Id(2)]
    public int CustomerId { get; set; }
}
#endregion