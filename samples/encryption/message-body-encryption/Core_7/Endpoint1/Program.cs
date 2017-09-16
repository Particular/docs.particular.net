using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        Console.Title = "Samples.MessageBodyEncryption.Endpoint2";
        var endpointConfiguration = new EndpointConfiguration("Samples.MessageBodyEncryption.Endpoint1");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region RegisterMessageEncryptor

        endpointConfiguration.RegisterMessageEncryptor();

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var completeOrder = new CompleteOrder
        {
            CreditCard = "123-456-789"
        };
        await endpointInstance.Send("Samples.MessageBodyEncryption.Endpoint2", completeOrder)
            .ConfigureAwait(false);
        Console.WriteLine("Message sent");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}