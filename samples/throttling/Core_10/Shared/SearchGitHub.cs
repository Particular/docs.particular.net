using NServiceBus;

public class SearchGitHub : IMessage
{
    public string Owner { get; set; }

    public string Repository { get; set; }

    public string Branch { get; set; }
}