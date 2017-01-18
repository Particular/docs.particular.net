//#define POST_MIGRATION

using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
// ReSharper disable RedundantUsingDirective
using NServiceBus.Features;
using NServiceBus.Routing;
using NServiceBus.Transport;
// ReSharper restore RedundantUsingDirective

class Program
{
    static void Main()
    {
        Start().GetAwaiter().GetResult();
    }

    static async Task Start()
    {
        Console.Title = "Samples.SagaMigration.Server.New";
        var endpointConfiguration = new EndpointConfiguration("Samples.SagaMigration.Server");

#if !POST_MIGRATION
        endpointConfiguration.OverrideLocalAddress("Samples.SagaMigration.Server.New");
#endif

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=nservicebus;Integrated Security=True;");
            });
        persistence.TablePrefix("New");
        endpointConfiguration.SendFailedMessagesTo("error");

        var endpoint = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit.");
        Console.ReadLine();

        await endpoint.Stop()
            .ConfigureAwait(false);
    }
}

#if POST_MIGRATION
public class DrainTempQueueSatelliteFeature :
    Feature
{
    public DrainTempQueueSatelliteFeature()
    {
        EnableByDefault();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        #region DrainTempQueueSatellite

        var settings = context.Settings;
        var qualifiedAddress = settings.LogicalAddress()
            .CreateQualifiedAddress("New");
        var tempQueue = settings.GetTransportAddress(qualifiedAddress);
        var mainQueue = settings.LocalAddress();

        context.AddSatelliteReceiver(
            name: "DrainTempQueueSatellite",
            transportAddress: tempQueue,
            runtimeSettings: new PushRuntimeSettings(maxConcurrency: 1),
            recoverabilityPolicy: (config, errorContext) =>
            {
                return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
            },
            onMessage: (builder, messageContext) =>
            {
                var messageDispatcher = builder.Build<IDispatchMessages>();
                var outgoingHeaders = messageContext.Headers;

                var outgoingMessage = new OutgoingMessage(messageContext.MessageId, outgoingHeaders, messageContext.Body);
                var outgoingOperation = new TransportOperation(outgoingMessage, new UnicastAddressTag(mainQueue));

                Console.WriteLine($"Moving message from {tempQueue} to {mainQueue}");

                var transportOperations = new TransportOperations(outgoingOperation);
                return messageDispatcher.Dispatch(transportOperations, messageContext.TransportTransaction, messageContext.Context);
            });

        #endregion
    }
}
#endif