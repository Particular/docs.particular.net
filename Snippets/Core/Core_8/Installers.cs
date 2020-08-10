// ReSharper disable RedundantJumpStatement
namespace Core8
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

        public static async Task Main(string[] args)
        {
            #region InstallersRunWhenNecessaryCommandLine

            var runInstallers = Environment.GetCommandLineArgs().Any(x => string.Equals(x, "/runInstallers", StringComparison.OrdinalIgnoreCase));

            if (runInstallers)
            {
                endpointConfiguration.EnableInstallers();
                // This will run the installers but not start the instance.
                await Endpoint.Create(endpointConfiguration)
                    .ConfigureAwait(false);
                Environment.Exit(0);
            }

            #endregion
        }
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
}
