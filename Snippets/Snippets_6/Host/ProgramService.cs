using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using NServiceBus;

#region windowsservicehosting
class ProgramService : ServiceBase
{
    IBus bus;

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
        //rest of you bus configuration. eg endpoint name, logging, transport, persistence etc
        busConfiguration.EnableInstallers();
        bus = await Bus.Create(busConfiguration).StartAsync();
    }

    protected override void OnStop()
    {
        if (bus != null)
        {
            bus.Dispose();
        }
    }

}
#endregion