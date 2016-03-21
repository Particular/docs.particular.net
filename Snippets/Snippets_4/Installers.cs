namespace Snippets4
{
    using NServiceBus;
    using NServiceBus.Installation.Environments;
    using NServiceBus.Unicast.Config;

    class ForInstallationOn
    {
        ForInstallationOn(Configure configure)
        {
            #region Installers

            ConfigUnicastBus configUnicastBus = configure.UnicastBus();
            IStartableBus startableBus = configUnicastBus.CreateBus();
            startableBus.Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

            #endregion
        }
    }
}