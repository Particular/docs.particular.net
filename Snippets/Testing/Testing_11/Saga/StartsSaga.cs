using NServiceBus;

public class StartsSaga :
    ICommand
{
    public string MyId { get; set; }
}