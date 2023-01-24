using NServiceBus;

#region Message
public class Command : IMessage
{
    public int Id { get; set; }
}
#endregion