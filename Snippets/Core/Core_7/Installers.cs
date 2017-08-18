namespace Core7
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using NServiceBus;

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
            var runInstallers = args.Any(x => x.ToLower() == "/runInstallers");

            if (runInstallers)
            {
                endpointConfiguration.EnableInstallers();
            }
        }

        #endregion
    }

    class SwitchInstallersByMachineNameConvention
    {
        Task Simple(EndpointConfiguration endpointConfiguration)
        {
            #region InstallersRunWhenNecessaryMachineNameConvention

            if (!Environment.MachineName.EndsWith("-PROD"))
            {
                endpointConfiguration.EnableInstallers();
            }

            #endregion
            return Task.CompletedTask;
        }
    }

    class DisableInstallers
    {
        async Task Simple(EndpointConfiguration endpointConfiguration)
        {
            #region DisableInstallers

            //endpointConfiguration.DisableInstallers();

            // this will not run the installers
            await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            #endregion
        }
    }
}