using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace NativeSender
{
    using System;
    using System.Threading.Tasks;

    public static class TopologyManager
    {
        public static async Task CreateSubscription(string connectionString, string subscriptionName, string ruleName, SqlRuleFilter sqlFilter, string topicPath = "bundle-1")
        {
            var client = new ServiceBusAdministrationClient(connectionString);

            try
            {
                #region SubscriptionCreation

                await client.CreateSubscriptionAsync(new CreateSubscriptionOptions(topicPath, subscriptionName)
                {
                    LockDuration = TimeSpan.FromMinutes(5),
                    EnableDeadLetteringOnFilterEvaluationExceptions = false,
                    MaxDeliveryCount = int.MaxValue,
                    EnableBatchedOperations = true,
                }, new CreateRuleOptions(ruleName, sqlFilter)).ConfigureAwait(false);

                #endregion
            }
            catch (ServiceBusException ex) when (ex.Reason == ServiceBusFailureReason.MessagingEntityAlreadyExists)
            {
            }
        }
    }
}