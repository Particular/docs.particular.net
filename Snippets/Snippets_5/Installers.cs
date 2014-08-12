using NServiceBus;

public class ForInstallationOnReplacement
{
    public void Simple()
    {
        #region InstallersV5

        var configure = Configure.With(builder => builder.EnableInstallers());
        var bus = configure.CreateBus();
        bus.Start();

        #endregion
    }

}