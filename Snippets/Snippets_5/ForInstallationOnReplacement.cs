using NServiceBus;

public class ForInstallationOnReplacement
{
    public void Simple()
    {
        // start code ForInstallationOnReplacementV5
        
        var configure = Configure.With();
        var bus = configure.CreateBus();
        configure.EnableInstallers();
        bus.Start();

        // end code ForInstallationOnReplacementV5
    }

}