using System;
using System.ComponentModel;
using System.ServiceProcess;
using System.Threading.Tasks;
using NServiceBus;

[DesignerCategory("Code")]
class ProgramService :
    ServiceBase
{
    IEndpointInstance endpointInstance;

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
        AsyncOnStart().GetAwaiter().GetResult();
    }

    async Task AsyncOnStart()
    {
        var endpointConfiguration = new EndpointConfiguration("Samples.WindowsServiceAndConsole");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.EnableInstallers();
        endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        // run any startup actions on the bus
        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage)
            .ConfigureAwait(false);
    }

    #endregion

    #region windowsservice-hosting-onstop

    protected override void OnStop()
    {
        endpointInstance?.Stop().GetAwaiter().GetResult();
    }

    #endregion

}