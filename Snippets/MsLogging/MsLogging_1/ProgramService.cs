using System;
using System.ComponentModel;
using System.ServiceProcess;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Logging;

#region MsLoggingInService
[DesignerCategory("Code")]
class ProgramService :
    ServiceBase
{
    IEndpointInstance endpointInstance;
    Microsoft.Extensions.Logging.ILoggerFactory loggerFactory;

    static void Main()
    {
        using (var service = new ProgramService())
        {
            if (ServiceHelper.IsService())
            {
                Run(service);
                return;
            }
            service.OnStart(null);
            Console.WriteLine("Bus started. Press any key to exit");
            Console.ReadKey();
            service.OnStop();
        }
    }

    protected override void OnStart(string[] args)
    {
        AsyncOnStart().GetAwaiter().GetResult();
    }

    async Task AsyncOnStart()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddFilter(level => level >= Microsoft.Extensions.Logging.LogLevel.Information);
            loggingBuilder.AddConsole();
        });

        var serviceProvider = serviceCollection.BuildServiceProvider();

        loggerFactory = serviceProvider.GetService<Microsoft.Extensions.Logging.ILoggerFactory>();
        var logFactory = LogManager.Use<MicrosoftLogFactory>();
        logFactory.UseMsFactory(loggerFactory);
        var endpointConfiguration = new EndpointConfiguration("EndpointName");
        endpointConfiguration.EnableInstallers();
        endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
    }

    protected override void OnStop()
    {
        AsyncOnStop().GetAwaiter().GetResult();
    }

    async Task AsyncOnStop()
    {
        if (endpointInstance != null)
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
        loggerFactory?.Dispose();
    }
}
#endregion

class ServiceHelper
{
    public static bool IsService()
    {
        throw new NotImplementedException();
    }
}