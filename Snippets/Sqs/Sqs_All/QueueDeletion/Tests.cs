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
        public async Task DeleteAllQueuesWithPrefix()
        {
            using (var client = ClientFactory.CreateSqsClient())
            {
                var creationTasks = new Task[12];
                for (var i = 0; i < 10; i++)
                {
                    creationTasks[i] = RetryCreateOnThrottle(client, $"delete-{i}", TimeSpan.FromSeconds(10), 6, queueNamePrefix: "DEV");
                }

                creationTasks[10] = RetryCreateOnThrottle(client, "delete-10", TimeSpan.FromSeconds(10), 6);
                creationTasks[11] = Task.Delay(TimeSpan.FromSeconds(60));

                await Task.WhenAll(creationTasks)
                    .ConfigureAwait(false);
            }

            await QueueDeletionUtils.DeleteAllQueues(queueNamePrefix: "DEV")
                .ConfigureAwait(false);

            Assert.IsTrue(await QueueExistenceUtils.Exists("delete-10"));

            for (var i = 0; i < 10; i++)
            {
                Assert.IsFalse(await QueueExistenceUtils.Exists($"DEVdelete-{i}"));
            }
        }


        [Test]
        public async Task DeleteAllQueues()
        {
            using (var client = ClientFactory.CreateSqsClient())
            {
                var creationTasks = new Task[11];
                for (var i = 0; i < 10; i++)
                {
                    creationTasks[i] = RetryCreateOnThrottle(client, $"delete-{i}", TimeSpan.FromSeconds(10), 6);
                }

                creationTasks[10] = Task.Delay(TimeSpan.FromSeconds(60));

                await Task.WhenAll(creationTasks)
                    .ConfigureAwait(false);
            }

            await QueueDeletionUtils.DeleteAllQueues()
                .ConfigureAwait(false);

            for (var i = 0; i < 10; i++)
            {
                Assert.IsFalse(await QueueExistenceUtils.Exists($"delete-{i}"));
            }
        }

        [Test]
        public async Task DeleteAllQueuesWhenMoreThanAThousandQueues() // Takes several minutes!
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

            for (var i = 0; i < 2000; i++)
            {
                Assert.IsFalse(await QueueExistenceUtils.Exists($"delete-{i}"));
            }
        }

        static async Task RetryCreateOnThrottle(IAmazonSQS client, string queueName, TimeSpan delay, int maxRetryAttempts, string queueNamePrefix = null, int retryAttempts = 0)
        {
            try
            {
                await client.CreateQueueAsync(QueueNameHelper.GetSqsQueueName(queueName, queueNamePrefix)).ConfigureAwait(false);
            }
            catch (AmazonServiceException ex) when (ex.ErrorCode == "RequestThrottled")
            {
                if (retryAttempts < maxRetryAttempts)
                {
                    var attempts = TimeSpan.FromMilliseconds(Convert.ToInt32(delay.TotalMilliseconds * (retryAttempts + 1)));
                    Console.WriteLine($"Retry {queueName} {retryAttempts}/{maxRetryAttempts} with delay {attempts}.");
                    await Task.Delay(attempts).ConfigureAwait(false);
                    await RetryCreateOnThrottle(client, queueName, delay, maxRetryAttempts, retryAttempts: ++retryAttempts).ConfigureAwait(false);
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
        public async Task DeleteQueueForEndpointWithPrefix_Powershell()
        {
            var endpointName = "myendpoint-powershell";
            var errorQueueName = "myerror-powershell";
            var auditQueueName = "myaudit-powershell";

            await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName, queueNamePrefix: "DEV")
                .ConfigureAwait(false);
            await QueueCreationUtils.CreateQueue(errorQueueName, queueNamePrefix: "DEV")
                .ConfigureAwait(false);
            await QueueCreationUtils.CreateQueue(auditQueueName, queueNamePrefix: "DEV")
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
                    command.AddParameter("QueueNamePrefix", "DEV");
                    command.Invoke();

                    command = powerShell.AddCommand("DeleteQueue");
                    command.AddParameter("QueueName", errorQueueName);
                    command.AddParameter("QueueNamePrefix", "DEV");
                    command.Invoke();

                    command = powerShell.AddCommand("DeleteQueue");
                    command.AddParameter("QueueName", auditQueueName);
                    command.AddParameter("QueueNamePrefix", "DEV");
                    command.Invoke();
                }

                await AssertQueuesDeleted(endpointName, errorQueueName, auditQueueName, queueNamePrefix: "DEV")
                    .ConfigureAwait(false);
            }
            finally
            {
                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, queueNamePrefix: "DEV")
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(errorQueueName, queueNamePrefix: "DEV")
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(auditQueueName, queueNamePrefix: "DEV")
                    .ConfigureAwait(false);
            }
        }

        [Test]
        public async Task DeleteQueuesForEndpointWithPrefix()
        {
            var endpointName = "myendpoint";
            var errorQueueName = "myerror";
            var auditQueueName = "myaudit";

            await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName, queueNamePrefix: "DEV")
                .ConfigureAwait(false);
            await QueueCreationUtils.CreateQueue(errorQueueName, queueNamePrefix: "DEV")
                .ConfigureAwait(false);
            await QueueCreationUtils.CreateQueue(auditQueueName, queueNamePrefix: "DEV")
                .ConfigureAwait(false);


            await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, queueNamePrefix: "DEV")
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(errorQueueName, queueNamePrefix: "DEV")
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(auditQueueName, queueNamePrefix: "DEV")
                .ConfigureAwait(false);


            await AssertQueuesDeleted(endpointName, errorQueueName, auditQueueName, queueNamePrefix: "DEV")
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

        static async Task AssertQueuesDeleted(string endpointName, string errorQueueName, string auditQueueName, string queueNamePrefix = null, bool includeRetries = false)
        {
            Assert.IsFalse(await QueueExistenceUtils.Exists(endpointName, queueNamePrefix), $"Queue {endpointName} still exists.");

            Assert.IsFalse(await QueueExistenceUtils.Exists($"{endpointName}.Timeouts", queueNamePrefix), $"Queue {endpointName}.timeouts still exists.");

            Assert.IsFalse(await QueueExistenceUtils.Exists($"{endpointName}.TimeoutsDispatcher", queueNamePrefix), $"Queue {endpointName}.timeoutsdispatcher still exists.");

            if (includeRetries)
            {
                Assert.IsFalse(await QueueExistenceUtils.Exists($"{endpointName}.Retries", queueNamePrefix), $"Queue {endpointName}.retries still exists.");
            }

            Assert.IsFalse(await QueueExistenceUtils.Exists(errorQueueName, queueNamePrefix), $"Queue {errorQueueName} still exists.");

            Assert.IsFalse(await QueueExistenceUtils.Exists(auditQueueName, queueNamePrefix), $"Queue {auditQueueName} still exists.");
        }
    }

}