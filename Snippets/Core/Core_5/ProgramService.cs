namespace Core5
{
    using System;
    using System.ComponentModel;
    using System.ServiceProcess;
    using NServiceBus;

    #region windowsservicehosting

    [DesignerCategory("Code")]
    class ProgramService :
        ServiceBase
    {
        IBus bus;

        static void Main()
        {
            using (var service = new ProgramService())
            {
                if (Environment.UserInteractive)
                {
                    service.OnStart(null);

                    Console.WriteLine("Bus started. Press any key to exit");
                    Console.ReadKey();

                    service.OnStop();

                    return;
                }
                Run(service);
            }
        }

        protected override void OnStart(string[] args)
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("EndpointName");
            busConfiguration.EnableInstallers();
            bus = Bus.Create(busConfiguration).Start();
        }

        protected override void OnStop()
        {
            bus?.Dispose();
        }
    }

    #endregion
}