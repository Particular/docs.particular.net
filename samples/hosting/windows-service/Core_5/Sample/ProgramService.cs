using System;
using System.ComponentModel;
using System.ServiceProcess;
using NServiceBus;

[DesignerCategory("Code")]
class ProgramService :
    ServiceBase
{
    IBus bus;

    #region windowsservice-hosting-main

    static void Main()
    {
        using (var service = new ProgramService())
        {
            if (ServiceHelper.IsService())
            {
                Run(service);
                return;
            }
            Console.Title = "Samples.WindowsServiceAndConsole";
            service.OnStart(null);

            Console.WriteLine("Bus started. Press any key to exit");
            Console.ReadKey();

            service.OnStop();
        }
    }

    #endregion

    #region windowsservice-hosting-onstart

    protected override void OnStart(string[] args)
    {
        var busConfiguration = new BusConfiguration();

        busConfiguration.EndpointName("Samples.WindowsServiceAndConsole");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        bus = Bus.Create(busConfiguration).Start();

        // run any startup actions on the bus
        var myMessage = new MyMessage();
        bus.SendLocal(myMessage);
    }

    #endregion

    #region windowsservice-hosting-onstop

    protected override void OnStop()
    {
        bus?.Dispose();
    }

    #endregion
}