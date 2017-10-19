namespace Gateway_2
{
    using System;
    using NServiceBus;

    public class Timeouts
    {
        //customGatewayTimeout
        public Timeouts()
        {
            var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

            #region CustomGatewayTimeout

            var gatewayConfig = endpointConfiguration.Gateway();

            gatewayConfig.TransactionTimeout(TimeSpan.FromSeconds(40));

            #endregion
        }
    }
}