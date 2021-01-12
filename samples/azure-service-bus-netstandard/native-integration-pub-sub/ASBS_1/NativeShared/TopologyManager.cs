namespace NativeSender
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.ServiceBus.Management;

    public static class TopologyManager
    {
        public static async Task CreateSubscription(string connectionString, string subscriptionName, string ruleName, SqlFilter sqlFilter, string topicPath = "bundle-1")
        {
            var client = new ManagementClient(connectionString);

            try
            {
                #region SubscriptionCreation

                await client.CreateSubscriptionAsync(new SubscriptionDescription(topicPath, subscriptionName)
                {
                    LockDuration = TimeSpan.FromMinutes(5),
                    EnableDeadLetteringOnFilterEvaluationExceptions = false,
                    MaxDeliveryCount = int.MaxValue,
                    EnableBatchedOperations = true,
                }, new RuleDescription(ruleName, sqlFilter)).ConfigureAwait(false);

                #endregion
            }
            catch (MessagingEntityAlreadyExistsException)
            {
            }
        }
    }
}