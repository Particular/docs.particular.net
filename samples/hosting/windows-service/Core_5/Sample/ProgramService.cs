using System;
using System.ServiceProcess;
using NServiceBus;

class ProgramService : ServiceBase
{
    IBus bus;

    #region windowsservice-hosting-main

    static void Main()
    {
        Console.Title = "Samples.WindowsServiceAndConsole";
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
        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.EndpointName("Samples.WindowsServiceAndConsole");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        bus = Bus.Create(busConfiguration).Start();

        // run any startup actions on the bus
        bus.SendLocal(new MyMessage());
    }

    #endregion

    #region windowsservice-hosting-onstop

    protected override void OnStop()
    {
        if (bus != null)
        {
            bus.Dispose();
        }
    }

    #endregion

}