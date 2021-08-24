using NServiceBus;

namespace Messages
{
    #region OrderPlaced

    public class OrderPlaced :
        IEvent
    {
        public string OrderId { get; set; }
    }

    #endregion
}