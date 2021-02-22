using System;
using NServiceBus;
using NServiceBus.Routing;

public class DistributionStrategies
{
    void CustomDistributionStrategy(EndpointConfiguration endpointConfiguration)
    {
        #region RoutingExtensibility-Distribution

        var transport = new MsmqTransport();
        var routing = endpointConfiguration.UseTransport(transport);
        routing.SetMessageDistributionStrategy(new RandomStrategy("Sales", DistributionStrategyScope.Send));

        #endregion
    }


    #region RoutingExtensibility-DistributionStrategy

    class RandomStrategy :
        DistributionStrategy
    {
        static Random random = new Random();

        public RandomStrategy(string endpoint, DistributionStrategyScope scope) : base(endpoint, scope)
        {
        }

        public override string SelectDestination(DistributionContext context)
        {
            // access to headers, payload...
            return context.ReceiverAddresses[random.Next(context.ReceiverAddresses.Length)];
        }
    }

    #endregion
}