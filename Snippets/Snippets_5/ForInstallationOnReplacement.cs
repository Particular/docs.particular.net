using NServiceBus;

public class ForInstallationOnReplacement
{
    public void Simple()
    {
        #region ForInstallationOnReplacementV5

        var configure = Configure.With(builder => builder.EnableInstallers());
        var bus = configure.CreateBus();
        bus.Start();

        #endregion
    }

}