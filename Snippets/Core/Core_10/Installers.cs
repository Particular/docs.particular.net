using Microsoft.Extensions.Hosting;

namespace Core;

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Installation;

class ForInstallationOnReplacement
{
    void Simple(EndpointConfiguration endpointConfiguration)
    {
        #region Installers

        endpointConfiguration.EnableInstallers();

        #endregion
    }
}

class SwitchInstallersWithCommandline
{
    public static void InstallersRunWhenNecessaryCommandLine()
    {
        var endpointConfiguration = new EndpointConfiguration("someEndpoint");

        #region InstallersRunWhenNecessaryCommandLine

        var runInstallers = Environment.GetCommandLineArgs().Any(x => string.Equals(x, "/runInstallers", StringComparison.OrdinalIgnoreCase));

        if (runInstallers)
        {
            endpointConfiguration.EnableInstallers();
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
    public static async Task MainProgram()
    {
        #region installer-setup

        var hostBuilder = new HostApplicationBuilder();
        var endpointConfiguration = new EndpointConfiguration("someEndpoint");

        hostBuilder.Services.AddNServiceBusEndpoint(endpointConfiguration);

        // On run, this will run the installers and then stop the host
        hostBuilder.Services.AddNServiceBusInstallers();

        // OR, this will run the installers and allow the host to continue
        hostBuilder.Services.AddNServiceBusInstallers(options =>
            options.ShutdownBehavior = InstallersShutdownBehavior.Continue);

        // This will run the installers with the requested ShutdownBehavior
        await hostBuilder.Build()
            .RunAsync();

        #endregion
    }
}