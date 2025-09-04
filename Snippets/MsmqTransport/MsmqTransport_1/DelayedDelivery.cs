using System;
using NServiceBus;
using NServiceBus.Features;

class DelayedDelivery
{
    DelayedDelivery(EndpointConfiguration endpointConfiguration)
    {
        #region delayed-delivery

        var messageStore = new SqlServerDelayedMessageStore(
            connectionString: "database=(local); initial catalog=my_catalog; integrated security=true",
            schema: "my_schema", //optional, defaults to dbo
            tableName: "my_delayed_messages"); //optional, defaults to endpoint name with '.delayed' suffix

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        var delayedDeliverySettings = transport.NativeDelayedDelivery(messageStore);

        delayedDeliverySettings.NumberOfRetries = 7;
        delayedDeliverySettings.MaximumRecoveryFailuresPerSecond = 2;
        delayedDeliverySettings.TimeToTriggerStoreCircuitBreaker = TimeSpan.FromSeconds(20);
        delayedDeliverySettings.TimeToTriggerDispatchCircuitBreaker = TimeSpan.FromSeconds(15);
        delayedDeliverySettings.TimeToTriggerFetchCircuitBreaker = TimeSpan.FromSeconds(45);

        endpointConfiguration.DisableFeature<TimeoutManager>();

        #endregion
    }
}
