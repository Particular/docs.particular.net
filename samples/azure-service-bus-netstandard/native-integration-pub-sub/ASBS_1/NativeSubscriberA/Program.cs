using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using NativeSender;
class Program
{
    static string EnclosedMessageTypesHeader = "NServiceBus.EnclosedMessageTypes";

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
        var subscriptionName = "Samples.ASB.NativeIntegration.NativeSubscriberA";
        
        Console.Title = subscriptionName;

        #region EventOneSubscription
        await TopologyManager.CreateSubscription(
            ConnectionString,
            subscriptionName,
            ruleName:"event-one", 
            sqlFilter: new SqlFilter($"[NServiceBus.EnclosedMessageTypes] LIKE '%{typeof(EventOne).FullName}%'")
            );
        #endregion

        var subscription = new SubscriptionClient(ConnectionString, "bundle-1", subscriptionName);

        subscription.RegisterMessageHandler((m, _) =>
        {
            var messageType = (string) m.UserProperties[EnclosedMessageTypesHeader];
            var bodyJson = Encoding.UTF8.GetString(m.Body);

            Console.WriteLine($"Received: {messageType}");
            Console.WriteLine(bodyJson);

            return Task.CompletedTask;
        }, e => Task.CompletedTask);


        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}