using NServiceBus;

public class ForInstallationOnReplacement
{
    public void Simple()
    {
        #region InstallersV5

        Configure.With(builder => builder.EnableInstallers());

        #endregion
    }

}