using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

class Program
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
        Console.Title = "Samples.Sqs.Simple";
        #region ConfigureEndpoint

        var endpointConfiguration = new EndpointConfiguration("Samples.Sqs.Simple");
        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.Region("us-east-1");
        transport.S3BucketForLargeMessages("bucketname", "my/key/prefix");

        #endregion
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        #region sendonly
        //TODO: uncomment to view a message in transit
        //endpointConfiguration.SendOnly();
        #endregion
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        #region sends
        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage)
            .ConfigureAwait(false);

        var myLargeMessage = new MyMessage
        {
            Data = new byte[257 * 1024]
        };
        await endpointInstance.SendLocal(myLargeMessage)
            .ConfigureAwait(false);

        #endregion
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}