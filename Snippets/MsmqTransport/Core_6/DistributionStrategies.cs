using System;
using NServiceBus;
using NServiceBus.Routing;

public class DistributionStrategies
{
    void CustomDistributionStrategy(EndpointConfiguration endpointConfiguration)
    {
        #region RoutingExtensibility-Distribution

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        var routing = transport.Routing();
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

        public override string SelectReceiver(string[] receiverAddresses)
        {
            return receiverAddresses[random.Next(receiverAddresses.Length)];
        }
    }

    #endregion

    class Version_6_2
    {
        #region RoutingExtensibility-DistributionStrategy [6.2,)

        class RandomStrategy :
            DistributionStrategy
        {
            static Random random = new Random();

            public RandomStrategy(string endpoint, DistributionStrategyScope scope) : base(endpoint, scope)
            {
            }

            // Method will not be called since SelectDestination doesn't call base.SelectDestination
            public override string SelectReceiver(string[] receiverAddresses)
            {
                throw new NotImplementedException();
            }

            public override string SelectDestination(DistributionContext context)
            {
                // access to headers, payload...
                return context.ReceiverAddresses[random.Next(context.ReceiverAddresses.Length)];
            }
        }

        #endregion
    }
}