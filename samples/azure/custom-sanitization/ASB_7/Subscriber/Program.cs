using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.AzureServiceBus;
using NServiceBus.AzureServiceBus.Addressing;
using NServiceBus.Features;

class Program
{
    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        Console.Title = "Samples.ASB.Serialization.Subscriber";
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.ASB.Serialization.Subscriber");
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString"));
        var topology = transport.UseTopology<ForwardingTopology>();

        #region CustomSanitization

        transport.Sanitization().UseStrategy<Sha1Sanitization>();

        #endregion


        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.DisableFeature<SecondLevelRetries>();


        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            Console.WriteLine("Subscriber is ready to receive events");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}

#region Sha1SanitiazationStrategy

class Sha1Sanitization : ISanitizationStrategy
{
    public string Sanitize(string entityPathOrName, EntityType entityType)
    {
        // remove invalid characters
        if (entityType == EntityType.Queue || entityType == EntityType.Topic)
        {
            var regexQueueAndTopicValidCharacters = new Regex(@"[^a-zA-Z0-9\-\._\/]");
            var regexLeadingAndTrailingForwardSlashes = new Regex(@"^\/|\/$");

            entityPathOrName = regexQueueAndTopicValidCharacters.Replace(entityPathOrName, string.Empty);
            entityPathOrName = regexLeadingAndTrailingForwardSlashes.Replace(entityPathOrName, string.Empty);
        }

        if (entityType == EntityType.Subscription || entityType == EntityType.Rule || entityType == EntityType.EventHub)
        {
            var rgx = new Regex(@"[^a-zA-Z0-9\-\._]");
            entityPathOrName = rgx.Replace(entityPathOrName, "");
        }

        int entityPathOrNameMaxLength = 0;

        switch (entityType)
        {
            case EntityType.Queue:
            case EntityType.Topic:
                entityPathOrNameMaxLength = 260;
                break;
            case EntityType.Subscription:
            case EntityType.Rule:
                entityPathOrNameMaxLength = 50;
                break;
        }

        // hash if too long
        if (entityPathOrName.Length > entityPathOrNameMaxLength)
        {
            entityPathOrName = SHA1DeterministicNameBuilder.Build(entityPathOrName);
        }

        return entityPathOrName;
    }
}

#endregion

#region SHA1DeterministicNameBuilder

static class SHA1DeterministicNameBuilder
{
    public static string Build(string input)
    {
        using (var provider = new SHA1CryptoServiceProvider())
        {
            var inputBytes = Encoding.Default.GetBytes(input);
            var hashBytes = provider.ComputeHash(inputBytes);

            var hashBuilder = new StringBuilder(string.Join("", hashBytes.Select(x => x.ToString("x2"))));
            foreach (var delimeterIndex in new[] { 5, 11, 17, 23, 29, 35, 41 })
            {
                hashBuilder.Insert(delimeterIndex, "-");
            }
            return hashBuilder.ToString();
        }
    }
}

#endregion