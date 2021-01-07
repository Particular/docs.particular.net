namespace NativeSender
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.ServiceBus.Management;

    public static class TopologyManager
    {
        public static async Task CreateQueue(string connectionString, string queuePath)
        {
            var client = new ManagementClient(connectionString);

            try
            {
                #region QueueCreation

                await client.CreateQueueAsync(new QueueDescription(queuePath)
                {
                    EnableBatchedOperations = true,
                    LockDuration = TimeSpan.FromMinutes(5),
                    MaxDeliveryCount = int.MaxValue
                }).ConfigureAwait(false);

                #endregion
            }
            catch (MessagingEntityAlreadyExistsException)
            {
            }
            //HINT: see https://github.com/Azure/azure-service-bus-dotnet/issues/525
            catch (ServiceBusException sbe) when (sbe.Message.Contains("SubCode=40901.")) // An operation is in progress.
            {
            }
        }
        public static Task Subscribe(string connectionString, Type eventType, string inputQueuePath, string topicPath = "bundle-1")
        {
            return Subscribe(
                connectionString,
                eventType.FullName,
                #region EventOneFilteringRule
                new SqlFilter($"[NServiceBus.EnclosedMessageTypes] LIKE '%{eventType.FullName}%'"), 
                #endregion
                inputQueuePath);
        }

        public static Task SubscribeToAll(string connectionString, string inputQueuePath, string topicPath = "bundle-1")
        {
            return Subscribe(
                connectionString,
                "all",
                new TrueFilter(), 
                inputQueuePath);
        }

        static async Task Subscribe(string connectionString, string ruleName, SqlFilter sqlFilter, string inputQueuePath, string topicPath = "bundle-1")
        {
            var client = new ManagementClient(connectionString);

            try
            {
                #region SubscriptionCreation

                await client.CreateSubscriptionAsync(new SubscriptionDescription(topicPath, inputQueuePath)
                {
                    LockDuration = TimeSpan.FromMinutes(5),
                    ForwardTo = inputQueuePath,
                    EnableDeadLetteringOnFilterEvaluationExceptions = false,
                    MaxDeliveryCount = int.MaxValue,
                    EnableBatchedOperations = true,
                    UserMetadata = inputQueuePath
                }, new RuleDescription(ruleName, sqlFilter)).ConfigureAwait(false);

                #endregion
            }
            catch (MessagingEntityAlreadyExistsException)
            {
            }
        }
    }
}