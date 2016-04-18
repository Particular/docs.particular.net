namespace Store.Messages.Events
{
    using System.Collections.Generic;

    public interface DownloadIsReady 
    {
        int OrderNumber { get; set; }
        Dictionary<string, string> ProductUrls { get; set; }
        string ClientId { get; set; }
    }
}