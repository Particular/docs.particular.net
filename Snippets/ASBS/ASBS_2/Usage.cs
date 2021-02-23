using System;
using System.Security.Cryptography;
using System.Text;
using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region azure-service-bus-for-dotnet-standard

        var transport = new AzureServiceBusTransport("Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]");
        endpointConfiguration.UseTransport(transport);

        #endregion

        #region custom-prefetch-multiplier

        transport.PrefetchMultiplier = 3;

        #endregion

        #region custom-prefetch-count

        transport.PrefetchCount = 100;

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

        transport.SubscriptionNamingConvention = n => n.Length > MaxEntityName ? HashName(n) : n;
        transport.SubscriptionRuleNamingConvention = n => n.FullName.Length > MaxEntityName ? HashName(n.FullName) : n.FullName;

        #endregion
    }
}