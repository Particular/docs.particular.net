using NServiceBus;

#region asb-cloudevents-message-definition
public class BlobCreated :
    IMessage
{
    public string Api { get; set; }
    public string ClientRequestId { get; set; }
    public string RequestId { get; set; }
    public string ETag { get; set; }
    public string ContentType { get; set; }
    public int ContentLength { get; set; }
    public string BlobType { get; set; }
    public string Url { get; set; }
    public string Sequencer { get; set; }
    public StorageDiagnostics StorageDiagnostics { get; set; }
}

public class StorageDiagnostics
{
    public string BatchId { get; set; }
}
#endregion
