using System;

namespace Router
{
    using System.Threading.Tasks;

    class Program
    {
        static async Task Main(string[] args)
        {
            var salesConnectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString.Sales");
            if (string.IsNullOrWhiteSpace(salesConnectionString))
            {
                throw new Exception("Could not read 'AzureServiceBus.ConnectionString.Sales' environment variable. Check sample prerequisites.");
            }

            var shippingConnectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString.Shipping");
            if (string.IsNullOrWhiteSpace(shippingConnectionString))
            {
                throw new Exception("Could not read 'AzureServiceBus.ConnectionString.Shipping' environment variable. Check sample prerequisites.");
            }

            var serviceControlConnectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString.SC");
            if (string.IsNullOrWhiteSpace(serviceControlConnectionString))
            {
                throw new Exception("Could not read 'AzureServiceBus.ConnectionString.SC' environment variable. Check sample prerequisites.");
            }

            #region Setup
            var namespaces = new[]
            {
                new NamespaceDescription("Sales", salesConnectionString),
                new NamespaceDescription("Shipping", shippingConnectionString)
            };

            var namespaceRouter = new NamespaceRouter("Router", namespaces, "Particular.ServiceControl", serviceControlConnectionString);

            await namespaceRouter.Start().ConfigureAwait(false);

            #endregion

            Console.WriteLine("Press <enter> to exit.");
            Console.ReadLine();

            await namespaceRouter.Stop();

        }

        
    }
}
