using NServiceBus;

#region unsubscribe-message
public class ManualUnsubscribe :
    IMessage
{
    public string MessageTypeName { get; set; }
    public string SubscriberEndpoint { get; set; }
}
#endregion
