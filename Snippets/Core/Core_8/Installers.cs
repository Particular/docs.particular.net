namespace Core8
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus;
    using NServiceBus.Installation;

    class ForInstallationOnReplacement
    {
        async Task Simple(EndpointConfiguration endpointConfiguration)
        {
            #region Installers

            endpointConfiguration.EnableInstallers();

            // this will run the installers
            await Endpoint.Start(endpointConfiguration);

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
                await Endpoint.Create(endpointConfiguration);
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

    public class InstallerSetup
    {
        #region installer-setup
        public static async Task Main()
        {
            var endpointConfiguration = new EndpointConfiguration("my-endpoint");
            // configure endpoint

            await Installer.Setup(endpointConfiguration);
        }
        #endregion
    }

    public class InstallerSetupExternallyManagedContainer
    {
        #region installer-setup-externally-managed-container
        public static async Task Main()
        {
            var endpointConfiguration = new EndpointConfiguration("my-endpoint");
            // configure endpoint

            var serviceCollection = new ServiceCollection();
            // custom registrations

            var installer = Installer.CreateInstallerWithExternallyManagedContainer(endpointConfiguration, serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            await installer.Setup(serviceProvider);
        }
        #endregion
    }
}
