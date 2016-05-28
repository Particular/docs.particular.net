namespace Core3
{
    using NServiceBus;
    using NServiceBus.Installation.Environments;

    class ForInstallationOn
    {
        ForInstallationOn(Configure configure)
        {
            #region Installers

            var configUnicastBus = configure.UnicastBus();
            var startableBus = configUnicastBus.CreateBus();
            startableBus.Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

            #endregion
        }
    }
}