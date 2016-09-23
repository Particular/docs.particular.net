namespace Core4
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
            startableBus.Start(
                startupAction: () =>
                {
                    configure.ForInstallationOn<Windows>().Install();
                });

            #endregion
        }
    }
}