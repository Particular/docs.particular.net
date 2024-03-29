﻿namespace Core_8
{
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus.Installation;
    using System.Threading.Tasks;
    using NServiceBus;

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