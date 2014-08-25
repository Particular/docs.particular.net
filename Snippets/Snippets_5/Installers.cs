using NServiceBus;

public class ForInstallationOnReplacement
{
    public void Simple()
    {
        #region InstallersV5

        var configuration = new BusConfiguration();

        configuration.EnableInstallers();

        Bus.Create(configuration);//this will run the installers

        #endregion
    }

}