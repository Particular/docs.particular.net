using System;
using System.ComponentModel;
using System.ServiceProcess;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Logging;

#region MsLoggingInService
[DesignerCategory("Code")]
class ProgramService :
    ServiceBase
{
    IEndpointInstance endpointInstance;
    LoggerFactory loggerFactory;

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
        loggerFactory = new LoggerFactory();
        loggerFactory.AddConsole();
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