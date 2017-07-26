using NServiceBus;

namespace Messages
{
    #region unsubscribe-message
    public class ManualUnsubscribe :
        IMessage
    {
        public string MessageTypeName { get; set; }
        public string MessageVersion { get; set; }
        public string SubscriberTransportAddress { get; set; }
        public string SubscriberEndpoint { get; set; }
    }
    #endregion
}
