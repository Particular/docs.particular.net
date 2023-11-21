namespace Store.Messages.Events
{
    using System.Collections.Generic;
    using NServiceBus;

    public class DownloadIsReady :
        IEvent
    {
        public int OrderNumber { get; set; }
        public Dictionary<string, string> ProductUrls { get; set; }
        public string ClientId { get; set; }
    }
}