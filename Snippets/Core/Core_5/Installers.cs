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


        public static void Main(string[] args)
        {
            #region InstallersRunWhenNecessaryCommandLine
                
            var runInstallers = Environment.GetCommandLineArgs().Any(x => x.ToLower() == "/runInstallers");

            if (runInstallers)
            {
                busConfiguration.EnableInstallers();
                // This will run the installers but not start the instance.
                Bus.Create(busConfiguration);
                Environment.Exit(0);
            }
            
            #endregion
        }
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
