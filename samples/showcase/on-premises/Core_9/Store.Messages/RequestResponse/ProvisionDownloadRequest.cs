namespace Store.Messages.RequestResponse
{
    using NServiceBus;

    public class ProvisionDownloadRequest :
        IMessage
    {
        public int OrderNumber { get; set; }
        public string[] ProductIds { get; set; }
        public string ClientId { get; set; }
    }
}