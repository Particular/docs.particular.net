using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using NServiceBus;

class ProgramService : ServiceBase
{
    IEndpointInstance endpoint;

    #region windowsservice-hosting-main

    static void Main()
    {
        using (ProgramService service = new ProgramService())
        {
            if (Environment.UserInteractive)
            {
                service.OnStart(null);

                Console.WriteLine("Bus created and configured");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();

                service.OnStop();

                return;
            }
            Run(service);
        }
    }

    #endregion

    #region windowsservice-hosting-onstart

    protected override void OnStart(string[] args)
    {
        AsyncOnStart().GetAwaiter().GetResult();
    }

    async Task AsyncOnStart()
    {
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

        endpointConfiguration.EndpointName("Samples.WindowsServiceAndConsole");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpoint = await Endpoint.Start(endpointConfiguration);
        // run any startup actions on the bus
        await endpoint.SendLocal(new MyMessage());
    }

    #endregion

    #region windowsservice-hosting-onstop

    protected override void OnStop()
    {
        endpoint?.Stop().GetAwaiter().GetResult();
    }

    #endregion

}