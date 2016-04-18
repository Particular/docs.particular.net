using NServiceBus;

public class WantToRunBeforeConfiguration :
    IWantToRunBeforeConfiguration
{
    public void Init()
    {
        Logger.WriteLine("Inside IWantToRunBeforeConfiguration.Init");
    }

}