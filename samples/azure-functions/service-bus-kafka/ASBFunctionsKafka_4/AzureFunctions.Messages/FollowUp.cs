using NServiceBus;

public class FollowUp : IMessage
{
    public string Value { get; set; }
}