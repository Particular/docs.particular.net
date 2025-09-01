using System;
using System.Security.Cryptography;
using System.Text;
using Azure.Messaging.ServiceBus;
using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region azure-service-bus-for-dotnet-standard

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString("Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]");

        #endregion

        #region custom-prefetch-multiplier

        transport.PrefetchMultiplier(3);

        #endregion

        #region custom-prefetch-count

        transport.PrefetchCount(100);

        #endregion

        #region asb-sanitization-compatibility

        string HashName(string input)
        {
            var inputBytes = Encoding.Default.GetBytes(input);
            // use MD5 hash to get a 16-byte hash of the string
            using (var provider = new MD5CryptoServiceProvider())
            {
                var hashBytes = provider.ComputeHash(inputBytes);
                return new Guid(hashBytes).ToString();
            }
        }

        const int MaxEntityName = 50;

        transport.SubscriptionNamingConvention(n => n.Length > MaxEntityName ? HashName(n) : n);
        transport.SubscriptionRuleNamingConvention(t => t.FullName.Length > MaxEntityName ? HashName(t.FullName) : t.FullName);

        #endregion

        #region azure-service-bus-usewebsockets
        transport.UseWebSockets();
        #endregion

        #region azure-service-bus-TimeToWaitBeforeTriggeringCircuitBreaker
        transport.TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromMinutes(2));
        #endregion

        #region azure-service-bus-RetryPolicyOptions
        var azureAsbRetryOptions = new Azure.Messaging.ServiceBus.ServiceBusRetryOptions
        {
            Mode = Azure.Messaging.ServiceBus.ServiceBusRetryMode.Exponential,
            MaxRetries = 5,
            Delay = TimeSpan.FromSeconds(0.8),
            MaxDelay = TimeSpan.FromSeconds(15)
        };
        transport.CustomRetryPolicy(azureAsbRetryOptions);
        #endregion
    }
}