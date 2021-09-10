using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Sales";

        var endpointConfiguration = new EndpointConfiguration("Samples.MultiTenant.Propagation.Sales");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport(new LearningTransport());

        #region configuration

        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(new StoreTenantIdBehavior(), "Stores tenant ID in the session");
        pipeline.Register(new PropagateTenantIdBehavior(), "Propagates tenant ID to outgoing messages");
        
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit.");
        Console.ReadLine();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}