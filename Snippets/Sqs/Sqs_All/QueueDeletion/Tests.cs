namespace SqsAll.QueueDeletion
{
    using System;
    using System.IO;
    using System.Management.Automation;
    using System.Threading.Tasks;
    using Amazon.Runtime;
    using Amazon.SQS;
    using SqsAll.QueueCreation;
    using NUnit.Framework;

    [TestFixture]
    [Explicit]
    public class Tests
    {

        [Test]
        public async Task DeleteAllQueues() // Takes several minutes!
        {
            using (var client = ClientFactory.CreateSqsClient())
            {
                var creationTasks = new Task[2001];
                for (var i = 0; i < 2000; i++)
                {
                    creationTasks[i] = RetryCreateOnThrottle(client, $"delete-{i}", TimeSpan.FromSeconds(10), 6);
                }

                creationTasks[2000] = Task.Delay(TimeSpan.FromSeconds(60));

                await Task.WhenAll(creationTasks)
                    .ConfigureAwait(false);
            }

            await QueueDeletionUtils.DeleteAllQueues()
                .ConfigureAwait(false);
        }

        static async Task RetryCreateOnThrottle(IAmazonSQS client, string queueName, TimeSpan delay, int maxRetryAttempts, int retryAttempts = 0)
        {
            try
            {
                await client.CreateQueueAsync(queueName).ConfigureAwait(false);
            }
            catch (AmazonServiceException ex) when (ex.ErrorCode == "RequestThrottled")
            {
                if (retryAttempts < maxRetryAttempts)
                {
                    var attempts = TimeSpan.FromMilliseconds(Convert.ToInt32(delay.TotalMilliseconds * (retryAttempts + 1)));
                    Console.WriteLine($"Retry {queueName} {retryAttempts}/{maxRetryAttempts} with delay {attempts}.");
                    await Task.Delay(attempts).ConfigureAwait(false);
                    await RetryCreateOnThrottle(client, queueName, delay, maxRetryAttempts, ++retryAttempts).ConfigureAwait(false);
                }
                else
                {
                    Console.WriteLine($"Unable to create queue. Max retry attempts reached on {queueName}.");
                }
            }
            catch (AmazonServiceException ex)
            {
                Console.WriteLine($"Unable to create {queueName} on {retryAttempts}/{maxRetryAttempts}. Reason: {ex.Message}");
            }
        }

        [Test]
        public async Task DeleteQueueForEndpoint_Powershell()
        {
            var endpointName = "myendpoint-powershell";
            var errorQueueName = "myerror-powershell";
            var auditQueueName = "myaudit-powershell";

            await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName)
                .ConfigureAwait(false);
            await QueueCreationUtils.CreateQueue(errorQueueName)
                .ConfigureAwait(false);
            await QueueCreationUtils.CreateQueue(auditQueueName)
                .ConfigureAwait(false);

            try
            {
                var scriptPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "QueueDeletion/QueueDeletion.ps1");
                using (var powerShell = PowerShell.Create())
                {
                    powerShell.AddScript(File.ReadAllText(scriptPath));
                    powerShell.Invoke();
                    var command = powerShell.AddCommand("DeleteQueuesForEndpoint");
                    command.AddParameter("EndpointName", endpointName);
                    command.Invoke();

                    command = powerShell.AddCommand("DeleteQueue");
                    command.AddParameter("QueueName", errorQueueName);
                    command.Invoke();

                    command = powerShell.AddCommand("DeleteQueue");
                    command.AddParameter("QueueName", auditQueueName);
                    command.Invoke();
                }

                await AssertQueuesDeleted(endpointName, errorQueueName, auditQueueName)
                    .ConfigureAwait(false);
            }
            finally
            {
                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName)
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(errorQueueName)
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(auditQueueName)
                    .ConfigureAwait(false);
            }
        }

        [Test]
        public async Task DeleteQueuesForEndpoint()
        {
            var endpointName = "myendpoint";
            var errorQueueName = "myerror";
            var auditQueueName = "myaudit";

            await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName)
                .ConfigureAwait(false);
            await QueueCreationUtils.CreateQueue(errorQueueName)
                .ConfigureAwait(false);
            await QueueCreationUtils.CreateQueue(auditQueueName)
                .ConfigureAwait(false);


            await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(errorQueueName)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(auditQueueName)
                .ConfigureAwait(false);


            await AssertQueuesDeleted(endpointName, errorQueueName, auditQueueName)
                .ConfigureAwait(false);
        }

        [Test]
        public async Task DeleteQueueForEndpointWithRetries_Powershell()
        {
            var endpointName = "myendpoint-powershell";
            var errorQueueName = "myerror-powershell";
            var auditQueueName = "myaudit-powershell";

            await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName, includeRetries: true)
                .ConfigureAwait(false);
            await QueueCreationUtils.CreateQueue(errorQueueName)
                .ConfigureAwait(false);
            await QueueCreationUtils.CreateQueue(auditQueueName)
                .ConfigureAwait(false);

            var scriptPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "QueueDeletion/QueueDeletion.ps1");
            using (var powerShell = PowerShell.Create())
            {
                powerShell.AddScript(File.ReadAllText(scriptPath));
                powerShell.Invoke();
                var command = powerShell.AddCommand("DeleteQueuesForEndpoint");
                command.AddParameter("EndpointName", endpointName);
                command.AddParameter("IncludeRetries");
                command.Invoke();

                command = powerShell.AddCommand("DeleteQueue");
                command.AddParameter("QueueName", errorQueueName);
                command.Invoke();

                command = powerShell.AddCommand("DeleteQueue");
                command.AddParameter("QueueName", auditQueueName);
                command.Invoke();
            }

            await AssertQueuesDeleted(endpointName, errorQueueName, auditQueueName, includeRetries: true)
                .ConfigureAwait(false);
        }

        [Test]
        public async Task DeleteQueuesForEndpointWithRetries()
        {
            var endpointName = "myendpoint";
            var errorQueueName = "myerror";
            var auditQueueName = "myaudit";

            await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName, includeRetries: true)
                .ConfigureAwait(false);
            await QueueCreationUtils.CreateQueue(errorQueueName)
                .ConfigureAwait(false);
            await QueueCreationUtils.CreateQueue(auditQueueName)
                .ConfigureAwait(false);


            await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, includeRetries: true)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(errorQueueName)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(auditQueueName)
                .ConfigureAwait(false);


            await AssertQueuesDeleted(endpointName, errorQueueName, auditQueueName, includeRetries: true)
                .ConfigureAwait(false);
        }

        static async Task AssertQueuesDeleted(string endpointName, string errorQueueName, string auditQueueName, bool includeRetries = false)
        {
            Assert.IsFalse(await QueueExistenceUtils.Exists(endpointName), $"Queue {endpointName} still exists.");

            Assert.IsFalse(await QueueExistenceUtils.Exists($"{endpointName}.timeouts"), $"Queue {endpointName}.timeouts still exists.");

            Assert.IsFalse(await QueueExistenceUtils.Exists($"{endpointName}.timeoutsdispatcher"), $"Queue {endpointName}.timeoutsdispatcher still exists.");

            if (includeRetries)
            {
                Assert.IsFalse(await QueueExistenceUtils.Exists($"{endpointName}.retries"), $"Queue {endpointName}.retries still exists.");
            }

            Assert.IsFalse(await QueueExistenceUtils.Exists(errorQueueName), $"Queue {errorQueueName} still exists.");

            Assert.IsFalse(await QueueExistenceUtils.Exists(auditQueueName), $"Queue {auditQueueName} still exists.");
        }
    }

}