using NServiceBus;

public class SearchGitHub :
    IMessage
{
    public string SearchFor { get; set; }
    public string Repository { get; set; }
    public string Owner { get; set; }
}