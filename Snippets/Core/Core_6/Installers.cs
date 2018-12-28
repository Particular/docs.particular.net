// ReSharper disable RedundantJumpStatement
namespace Core6
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

            // This will run the installers but not start the instance.
            await Endpoint.Create(endpointConfiguration)
                .ConfigureAwait(false);

            #endregion
        }
    }

    class SwitchInstallersWithCommandline
    {
        static EndpointConfiguration endpointConfiguration = new EndpointConfiguration("someEndpoint");

        #region InstallersRunWhenNecessaryCommandLine

        public static async Task Main(string[] args)
        {
            var runInstallers = args.Any(x => x.ToLower() == "/runInstallers");

            if (runInstallers)
            {
                endpointConfiguration.EnableInstallers();
                // This will run the installers but not start the instance.
                await Endpoint.Create(endpointConfiguration)
                    .ConfigureAwait(false);
                return; // Exit application
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

            endpointConfiguration.DisableInstallers();

            // this will not run the installers
            await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            #endregion
        }
    }
}
