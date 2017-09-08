using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    #region ServerInit
    static void Main()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
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
