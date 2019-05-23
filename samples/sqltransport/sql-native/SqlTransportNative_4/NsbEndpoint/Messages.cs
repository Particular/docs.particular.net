using NServiceBus;

#region MessageContract
public class SendMessage :
    IMessage
{
    public string Property { get; set; }
}

public class ReplyMessage :
    IMessage
{
    public string Property { get; set; }
}
#endregion