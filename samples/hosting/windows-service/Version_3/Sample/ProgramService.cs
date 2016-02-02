﻿using System;
using NServiceBus;
using NServiceBus.Installation.Environments;
using System.ServiceProcess;

class ProgramService : ServiceBase
{
    IBus bus;

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
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.WindowsServiceAndConsole");
        configure.Log4Net();
        configure.DefaultBuilder();
        configure.Sagas();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();

        bus = configure.UnicastBus()
            .CreateBus()
            .Start(() => configure.ForInstallationOn<Windows>().Install());      
        
        // run any startup actions on the bus
        bus.SendLocal(new MyMessage());
    }

    #endregion

    #region windowsservice-hosting-onstop

    protected override void OnStop()
    {
        if (bus != null)
        {
            ((IDisposable)bus).Dispose();
        }
    }

    #endregion

}