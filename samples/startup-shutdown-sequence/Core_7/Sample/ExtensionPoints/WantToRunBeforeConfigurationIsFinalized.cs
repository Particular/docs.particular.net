using NServiceBus;
using NServiceBus.Settings;

public class WantToRunBeforeConfigurationIsFinalized :
    IWantToRunBeforeConfigurationIsFinalized
{
    public void Run(SettingsHolder settings)
    {
        Logger.WriteLine("Inside WantToRunBeforeConfigurationIsFinalized.Run");
    }
}