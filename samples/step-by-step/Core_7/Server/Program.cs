using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    #region ServerInit
    static async Task Main()
    {
        Console.Title = "Samples.StepByStep.Server";
        var endpointConfiguration = new EndpointConfiguration("Samples.StepByStep.Server");
        endpointConfiguration.UseSerialization<XmlSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
    #endregion
}
