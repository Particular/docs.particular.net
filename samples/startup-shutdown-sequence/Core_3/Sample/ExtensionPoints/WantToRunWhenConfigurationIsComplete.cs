using NServiceBus.Config;

public class WantToRunWhenConfigurationIsComplete :
    IWantToRunWhenConfigurationIsComplete
{
    public void Run()
    {
        Logger.WriteLine("Inside IWantToRunWhenConfigurationIsComplete.Run");
    }

}