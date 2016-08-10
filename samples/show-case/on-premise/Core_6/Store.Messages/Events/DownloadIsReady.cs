namespace Store.Messages.Events
{
    using System.Collections.Generic;
    using NServiceBus;

    public interface DownloadIsReady :
        IEvent
    {
        int OrderNumber { get; set; }
        Dictionary<string, string> ProductUrls { get; set; }
        string ClientId { get; set; }
    }
}