using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Wire;

static class Program
{
    static void Main()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.MultipleDeserializers.WireEndpoint";
        #region configWire
        var endpointConfiguration = new EndpointConfiguration("Samples.MultipleDeserializers.WireEndpoint");
        endpointConfiguration.UseSerialization<WireSerializer>();
        endpointConfiguration.RegisterOutgoingMessageLogger();
        #endregion
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var message = MesasgeBuilder.BuildMessage();
        await endpointInstance.Send("Samples.MultipleDeserializers.ReceivingEndpoint", message)
            .ConfigureAwait(false);
        Console.WriteLine("Order Sent");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}