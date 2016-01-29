namespace Snippets4
{
    using NServiceBus;
    using NServiceBus.Installation.Environments;
    using NServiceBus.Unicast.Config;

    public class ForInstallationOn
    {
        public void Simple()
        {
            #region Installers

            Configure configure = Configure.With();
            ConfigUnicastBus configUnicastBus = configure.UnicastBus();
            IStartableBus startableBus = configUnicastBus.CreateBus();
            startableBus.Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

            #endregion
        }
    }
}