using NServiceBus;

namespace Messages.Events
{
    #region OrderPlaced

    public class OrderPlaced :
        IEvent
    {
        public string OrderId { get; set; }
    }

    #endregion
}