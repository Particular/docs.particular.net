namespace Core3
{
    using System;
    using System.Linq;
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

    class SwitchInstallersWithCommandline
    {
        static Configure configure = null;

        #region InstallersRunWhenNecessaryCommandLine

        public static void Main(string[] args)
        {
            var runInstallers = args.Any(x => x.ToLower() == "/runInstallers");

            var configUnicastBus = configure.UnicastBus();
            var startableBus = configUnicastBus.CreateBus();

            if (runInstallers)
            {
                startableBus.Start(
                    startupAction: () =>
                    {
                        configure.ForInstallationOn<Windows>().Install();
                    });
            }
        }

        #endregion
    }

    class SwitchInstallersByMachineNameConvention
    {
        void Simple(Configure configure)
        {
            #region InstallersRunWhenNecessaryMachineNameConvention

            var configUnicastBus = configure.UnicastBus();
            var startableBus = configUnicastBus.CreateBus();

            if (!Environment.MachineName.EndsWith("-PROD"))
            {
                startableBus.Start(
                    startupAction: () =>
                    {
                        configure.ForInstallationOn<Windows>().Install();
                    });
            }

            #endregion
        }
    }
}