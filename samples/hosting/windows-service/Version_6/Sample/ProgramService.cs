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
        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.EndpointName("Samples.WindowsServiceAndConsole");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.SendFailedMessagesTo("error");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        endpoint = await Endpoint.Start(busConfiguration);
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