using NServiceBus;
using NServiceBus.Installation.Environments;

public class ForInstallationOn
{
    public void Simple()
    {
        #region InstallersV4

        Configure.With()
            .UnicastBus()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

        #endregion
    }

}