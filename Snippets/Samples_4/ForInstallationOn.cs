using NServiceBus;
using NServiceBus.Installation.Environments;

public class ForInstallationOn
{
    public void Simple()
    {
        // start code ForInstallationOnV4

        Configure.With().UnicastBus()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

        // end code ForInstallationOnV4
    }

}