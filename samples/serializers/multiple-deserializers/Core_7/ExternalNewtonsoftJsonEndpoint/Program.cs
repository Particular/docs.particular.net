using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        Console.Title = "Samples.MultipleDeserializers.ExternalNewtonsoftJsonEndpoint";
        #region configExternalNewtonsoftJson
        var endpointConfiguration = new EndpointConfiguration("Samples.MultipleDeserializers.ExternalNewtonsoftJsonEndpoint");
        var serialization = endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        serialization.ContentTypeKey("NewtonsoftJson");
        endpointConfiguration.RegisterOutgoingMessageLogger();

        #endregion
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