using System;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Installation.Environments;
using System.ServiceProcess;

class ProgramService : ServiceBase
{
    IBus bus;

    #region windowsservice-hosting-main

    static void Main()
    {
        Console.Title = "Samples.WindowsServiceAndConsole";
        using (var service = new ProgramService())
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
        Configure.Serialization.Json();
        Configure.Features.Enable<Sagas>();

        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.WindowsServiceAndConsole");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();

        bus = configure.UnicastBus()
            .CreateBus()
            .Start(() => configure.ForInstallationOn<Windows>().Install());

        // run any startup actions on the bus
        var myMessage = new MyMessage();
        bus.SendLocal(myMessage);
    }

    #endregion

    #region windowsservice-hosting-onstop

    protected override void OnStop()
    {
        ((IDisposable) bus)?.Dispose();
    }

    #endregion

}