namespace Core6
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Support;

    class ForInstallationOnReplacement
    {
        async Task Simple(EndpointConfiguration endpointConfiguration)
        {
            #region Installers

            endpointConfiguration.EnableInstallers();

            // this will run the installers
            await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            #endregion
        }
    }

    class SwitchInstallersWithCommandline
    {
        static EndpointConfiguration endpointConfiguration = new EndpointConfiguration("someEndpoint");

        #region InstallersRunWhenNecessaryCommandLine

        public static void Main(string[] args)
        {
            var runInstallers = args.Length == 1 && args[0] == "/runInstallers";

            if (runInstallers)
            {
                endpointConfiguration.EnableInstallers();
            }
        }

        #endregion
    }

    class SwitchInstallersByMachineNameConvention
    {
        async Task Simple(EndpointConfiguration endpointConfiguration)
        {
            #region InstallersRunWhenNecessaryMachineNameConvention

            if (!RuntimeEnvironment.MachineName.EndsWith("-PROD"))
            {
                endpointConfiguration.EnableInstallers();
            }

            #endregion
        }
    }
}