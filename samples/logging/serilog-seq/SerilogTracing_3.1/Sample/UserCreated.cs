using NServiceBus;

public class UserCreated :
    IMessage
{
    public string UserName { get; set; }
}