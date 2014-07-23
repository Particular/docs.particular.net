using NServiceBus;

public class ForInstallationOnReplacement
{
    public void Simple()
    {
        // start code ForInstallationOnReplacementV5
        
        var configure = Configure.With();
        configure.EnableInstallers();
        var bus = configure.CreateBus();
        bus.Start();

        // end code ForInstallationOnReplacementV5
    }

}