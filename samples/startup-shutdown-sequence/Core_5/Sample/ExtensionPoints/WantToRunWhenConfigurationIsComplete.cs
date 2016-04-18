using NServiceBus;
using NServiceBus.Config;

public class WantToRunWhenConfigurationIsComplete :
    IWantToRunWhenConfigurationIsComplete
{
    public void Run(Configure config)
    {
        Logger.WriteLine("Inside IWantToRunWhenConfigurationIsComplete.Run");
    }
}