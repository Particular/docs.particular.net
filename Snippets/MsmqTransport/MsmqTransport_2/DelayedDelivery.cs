using System;
using NServiceBus;

class DelayedDelivery
{
    DelayedDelivery(EndpointConfiguration endpointConfiguration)
    {
        #region delayed-delivery

        var messageStore = new SqlServerDelayedMessageStore(
            connectionString: "database=(local); initial catalog=my_catalog; integrated security=true",
            schema: "my_schema", //optional, defaults to dbo
            tableName: "my_delayed_messages"); //optional, defaults to endpoint name with '.delayed' suffix

        var transport = new MsmqTransport
        {
            DelayedDelivery = new DelayedDeliverySettings(messageStore)
            {
                NumberOfRetries = 7,
                MaximumRecoveryFailuresPerSecond = 2,
                TimeToTriggerStoreCircuitBreaker = TimeSpan.FromSeconds(20),
                TimeToTriggerDispatchCircuitBreaker = TimeSpan.FromSeconds(15),
                TimeToTriggerFetchCircuitBreaker = TimeSpan.FromSeconds(45)
            }
        };
        endpointConfiguration.UseTransport(transport);

        #endregion
    }
}