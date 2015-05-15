using NServiceBus;

public class WantToRunBeforeConfigurationIsFinalized :
    IWantToRunBeforeConfigurationIsFinalized
{
    public void Run(Configure config)
    {
        Logger.WriteLine("Inside IWantToRunBeforeConfigurationIsFinalized.Run");
    }
}