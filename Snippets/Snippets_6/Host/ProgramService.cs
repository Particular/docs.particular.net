using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using NServiceBus;

#region windowsservicehosting
class ProgramService : ServiceBase
{
    IEndpoint bus;

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

    protected override void OnStart(string[] args)
    {
        AsyncOnStart().GetAwaiter().GetResult();
    }

    async Task AsyncOnStart()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        //other bus configuration. endpoint name, logging, transport, persistence etc
        busConfiguration.EnableInstallers();
        bus = await Endpoint.StartAsync(busConfiguration);
    }

    protected override void OnStop()
    {
        AsyncOnStop().GetAwaiter().GetResult();
    }

    async Task AsyncOnStop()
    {
        if (bus != null)
        {
            await bus.StopAsync();
        }
    }
}
#endregion