using NServiceBus;

public class WantToRunBeforeConfigurationIsFinalized :
    IWantToRunBeforeConfigurationIsFinalized
{
    public void Run()
    {
        Logger.WriteLine("Inside IWantToRunBeforeConfigurationIsFinalized.Run");
    }
}