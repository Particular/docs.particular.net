namespace Snippets4
{
    using System;
    using NServiceBus;
    using NServiceBus.Installation.Environments;
    using System.ServiceProcess;

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
            Configure configure = Configure.With();
            configure.DefineEndpointName("TheEndpointName");
            bus = configure.UnicastBus()
                .CreateBus()
                .Start(() => configure.ForInstallationOn<Windows>().Install());
        }

        protected override void OnStop()
        {
            if (bus != null)
            {
                ((IDisposable) bus).Dispose();
            }
        }

    }

    #endregion
}