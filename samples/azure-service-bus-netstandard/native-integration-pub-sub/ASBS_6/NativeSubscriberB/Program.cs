using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using NativeSender;

static class Program
{
    const string EnclosedMessageTypesHeader = "NServiceBus.EnclosedMessageTypes";

    static string ConnectionString
    {
        get
        {
            var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
            }

            return connectionString;
        }
    }

    static async Task Main()
    {
        var queueName = "Samples.ASB.NativeIntegration.NativeSubscriberB";

        Console.Title = queueName;

        await TopologyManager.CreateSubscription(
            ConnectionString,
            queueName,
            ruleName:"$default",
            sqlFilter: new TrueRuleFilter(),
            topicPath: "EventOne",
            forwardTo: queueName
        );
        await TopologyManager.CreateSubscription(
            ConnectionString,
            queueName,
            ruleName:"$default",
            sqlFilter: new TrueRuleFilter(),
            topicPath: "EventTwo",
            forwardTo: queueName
        );

        var serviceBusClient = new ServiceBusClient(ConnectionString);
        var serviceBusProcessor = serviceBusClient.CreateProcessor(queueName);

        serviceBusProcessor.ProcessMessageAsync += MessageHandler;
        serviceBusProcessor.ProcessErrorAsync += ErrorHandler;

        await serviceBusProcessor.StartProcessingAsync();

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }

    static async Task MessageHandler(ProcessMessageEventArgs args)
    {
        var messageType = (string) args.Message.ApplicationProperties[EnclosedMessageTypesHeader];
        var bodyJson = Encoding.UTF8.GetString(args.Message.Body.ToArray());

        Console.WriteLine($"Received: {messageType}");
        Console.WriteLine(bodyJson);

        // complete the message. messages is deleted from the subscription.
        await args.CompleteMessageAsync(args.Message);
    }

    static Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }
}