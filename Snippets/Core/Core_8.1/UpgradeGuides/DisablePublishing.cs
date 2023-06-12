﻿namespace Core8.PublishSubscribe
{
    using NServiceBus;

    class DisablePublishingUpgradeGuide
    {
        void DisablePublishingConfiguration(EndpointConfiguration endpointConfiguration)
        {
            #region DisablePublishing-UpgradeGuide
            var transportConfiguration = endpointConfiguration.UseTransport(new MyTransport());
            transportConfiguration.DisablePublishing();
            #endregion
        }
    }

    internal class MyTransport : TransportDefinition
    {

    }
}