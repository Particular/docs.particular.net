using NServiceBus;

public class TrackZipCode :
    ICommand
{
    public string ZipCode { get; set; }
}