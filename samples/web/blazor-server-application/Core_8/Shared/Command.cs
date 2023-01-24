#region Message
using NServiceBus;

public class Command : IMessage
{
    public int Id { get; set; }
}
#endregion