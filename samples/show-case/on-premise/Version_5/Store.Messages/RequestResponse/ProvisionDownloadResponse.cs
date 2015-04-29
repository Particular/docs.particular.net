namespace Store.Messages.RequestResponse
{
    public class ProvisionDownloadResponse
    {
        public int OrderNumber { get; set; }
        public string[] ProductIds { get; set; }
        public string ClientId { get; set; }
    }
}
