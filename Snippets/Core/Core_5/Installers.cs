// ReSharper disable RedundantJumpStatement
namespace Core5
{
    using System;
    using System.Linq;
    using NServiceBus;

    class ForInstallationOnReplacement
    {
        ForInstallationOnReplacement(BusConfiguration busConfiguration)
        {
            #region Installers

            busConfiguration.EnableInstallers();
            // this will run the installers
            Bus.Create(busConfiguration);

            #endregion
        }

    }

    class SwitchInstallersWithCommandline
    {
        static BusConfiguration busConfiguration = new BusConfiguration();

        #region InstallersRunWhenNecessaryCommandLine

        public static void Main(string[] args)
        {
            var runInstallers = args.Any(x => x.ToLower() == "/runInstallers");

            if (runInstallers)
            {
                busConfiguration.EnableInstallers();
                // This will run the installers but not start the instance.
                Bus.Create(busConfiguration);
                return; // Exit application
            }
        }

        #endregion
    }

    class SwitchInstallersByMachineNameConvention
    {
        void Simple(BusConfiguration endpointConfiguration)
        {
            #region InstallersRunWhenNecessaryMachineNameConvention

            if (!Environment.MachineName.EndsWith("-PROD"))
            {
                endpointConfiguration.EnableInstallers();
            }

            #endregion
        }
    }
}