using NServiceBus;

public class TrackZipCodeReply :
    IMessage
{
    public string ZipCode { get; set; }
    public int CurrentCount { get; set; }
}