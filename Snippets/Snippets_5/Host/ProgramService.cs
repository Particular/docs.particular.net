namespace Snippets5
{
    using System;
    using System.ServiceProcess;
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
            BusConfiguration busConfiguration = new BusConfiguration();
            //other bus configuration. endpoint name, logging, transport, persistence etc
            busConfiguration.EnableInstallers();
            bus = Bus.Create(busConfiguration).Start();
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
}