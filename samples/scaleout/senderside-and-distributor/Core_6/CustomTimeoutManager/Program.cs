using System;

namespace CustomTimeoutManager
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.DelayedDelivery;
    using NServiceBus.DeliveryConstraints;
    using NServiceBus.Raw;
    using NServiceBus.Routing;
    using NServiceBus.Transport;

    class Program
    {
        static void Main(string[] args)
        {
            Start().GetAwaiter().GetResult();
        }

        static async Task Start()
        {
            IStartableRawEndpoint timeoutProcessor = null;
            IStartableRawEndpoint timeoutRouter = null;

            var routerQueueName = "TimeoutRouter"; //MSMQ
            var processorQueueName = "TimeoutProcessor"; //SQL

            var timeoutRouterConfig = RawEndpointConfiguration.Create(routerQueueName, (context, dispatcher) =>
            {
                //When the message comes in we verify if it contains the Timeout Manager headers
                if (!context.Headers.TryGetValue("NServiceBus.Timeout.RouteExpiredTimeoutTo", out var destination)
                    || !context.Headers.TryGetValue("NServiceBus.Timeout.Expire", out var expireText))
                {
                    throw new PoisonMessageException("Message is not a timeout");
                }

                //Defer the message using timeoutProcessor -- the SQL transport dispatcher that implements native delayed delivery
                var constraints = new List<DeliveryConstraint>
                {
                    new DoNotDeliverBefore(DateTimeExtensions.ToUtcDateTime(expireText))
                };

                var message = new OutgoingMessage(context.MessageId, context.Headers, context.Body);
                var operation = new TransportOperation(message, new UnicastAddressTag(processorQueueName), DispatchConsistency.Default, constraints);
                return timeoutProcessor.Dispatch(new TransportOperations(operation), context.TransportTransaction, context.Extensions);
            }, "poison"); //used to store messages that have invalid headers

            var timeoutProcessorConfig = RawEndpointConfiguration.Create(processorQueueName, (context, dispatcher) =>
            {
                //When the message is due, get to the SQL Server transport message processor
                //We retrieve the original Timeout Manager destination and send the message there via MSMQ dispatcher -- timeoutRouter
                var destination = context.Headers["NServiceBus.Timeout.RouteExpiredTimeoutTo"];

                //Remove TM headers
                context.Headers.Remove("NServiceBus.Timeout.RouteExpiredTimeoutTo");
                context.Headers.Remove("NServiceBus.Timeout.Expire");

                var message = new OutgoingMessage(context.MessageId, context.Headers, context.Body);
                var operation = new TransportOperation(message, new UnicastAddressTag(destination));
                return timeoutRouter.Dispatch(new TransportOperations(operation), context.TransportTransaction, context.Extensions);
            }, "poison"); //used to store messages that have invalid headers

            timeoutRouterConfig.AutoCreateQueue();
            timeoutRouterConfig.CustomErrorHandlingPolicy(new RetryForeverPolicy());
            var routerTransport = timeoutRouterConfig.UseTransport<MsmqTransport>();

            timeoutProcessorConfig.AutoCreateQueue();
            timeoutProcessorConfig.CustomErrorHandlingPolicy(new RetryForeverPolicy());
            var processorTransport = timeoutProcessorConfig.UseTransport<SqlServerTransport>();
            processorTransport.ConnectionString("data source=(local); initial catalog=CustomTimeoutManager; integrated security=true");

            timeoutProcessor = await RawEndpoint.Create(timeoutProcessorConfig);
            timeoutRouter = await RawEndpoint.Create(timeoutRouterConfig);

            var timeoutProcessorStoppable = await timeoutProcessor.Start();
            var timeoutRouterStoppable = await timeoutRouter.Start();

            Console.WriteLine("Press <enter> to exit");
            Console.ReadLine();

            var stoppedProcessor = await timeoutProcessorStoppable.StopReceiving();
            var stoppedRouter = await timeoutRouterStoppable.StopReceiving();

            await stoppedProcessor.Stop();
            await stoppedRouter.Stop();
        }
    }
}
