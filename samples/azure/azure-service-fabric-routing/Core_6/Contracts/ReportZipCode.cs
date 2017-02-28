using NServiceBus;

public class ReportZipCode :
    ICommand
{
    public string ZipCode { get; set; }
    public int NumberOfVotes { get; set; }
}