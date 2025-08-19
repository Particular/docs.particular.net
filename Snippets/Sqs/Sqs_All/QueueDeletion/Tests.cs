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
                    creationTasks[i] = RetryCreateOnThrottle(client, $"deleteprefix-{i}", TimeSpan.FromSeconds(10), 6, queueNamePrefix: "DEV");
                }

                creationTasks[10] = RetryCreateOnThrottle(client, "deleteprefix-10", TimeSpan.FromSeconds(10), 6);
                creationTasks[11] = Task.Delay(TimeSpan.FromSeconds(60));

                await Task.WhenAll(creationTasks);
            }

            await QueueDeletionUtils.DeleteAllQueues(queueNamePrefix: "DEV");

            Assert.That(await QueueExistenceUtils.Exists("deleteprefix-10"), Is.True);

            for (var i = 0; i < 10; i++)
            {
                Assert.That(await QueueExistenceUtils.Exists($"DEVdeleteprefix-{i}"), Is.False);
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
                    creationTasks[i] = RetryCreateOnThrottle(client, $"deleteall-{i}", TimeSpan.FromSeconds(10), 6);
                }

                creationTasks[10] = Task.Delay(TimeSpan.FromSeconds(60));

                await Task.WhenAll(creationTasks);
            }

            await QueueDeletionUtils.DeleteAllQueues();

            for (var i = 0; i < 10; i++)
            {
                Assert.That(await QueueExistenceUtils.Exists($"deleteall-{i}"), Is.False);
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
                    creationTasks[i] = RetryCreateOnThrottle(client, $"deletemore2000-{i}", TimeSpan.FromSeconds(10), 6);
                }

                creationTasks[2000] = Task.Delay(TimeSpan.FromSeconds(60));

                await Task.WhenAll(creationTasks);
            }

            await QueueDeletionUtils.DeleteAllQueues();

            for (var i = 0; i < 2000; i++)
            {
                Assert.That(await QueueExistenceUtils.Exists($"deletemore2000-{i}"), Is.False);
            }
        }

        static async Task RetryCreateOnThrottle(IAmazonSQS client, string queueName, TimeSpan delay, int maxRetryAttempts, string queueNamePrefix = null, int retryAttempts = 0)
        {
            try
            {
                await client.CreateQueueAsync(QueueNameHelper.GetSqsQueueName(queueName, queueNamePrefix));
            }
            catch (AmazonServiceException ex) when (ex.ErrorCode == "RequestThrottled")
            {
                if (retryAttempts < maxRetryAttempts)
                {
                    var attempts = TimeSpan.FromMilliseconds(Convert.ToInt32(delay.TotalMilliseconds * (retryAttempts + 1)));
                    Console.WriteLine($"Retry {queueName} {retryAttempts}/{maxRetryAttempts} with delay {attempts}.");
                    await Task.Delay(attempts);
                    await RetryCreateOnThrottle(client, queueName, delay, maxRetryAttempts, retryAttempts: ++retryAttempts);
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
        [TestCase("TimeoutManager")]
        [TestCase("Native")]
        [TestCase("UnrestrictedDelayedDelivery")]
        public async Task DeleteQueueForEndpoint_Powershell(string delayedDeliveryMethod)
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"mydeleteendpoint-{randomName}-powershell";
            var errorQueueName = $"mydeleteerror-{randomName}-powershell";
            var auditQueueName = $"mydeleteaudit-{randomName}-powershell";

            await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName, delayedDeliveryMethod: delayedDeliveryMethod);
            await QueueCreationUtils.CreateQueue(errorQueueName);
            await QueueCreationUtils.CreateQueue(auditQueueName);

            try
            {
                var scriptPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "QueueDeletion/QueueDeletion.ps1");
                using (var powerShell = PowerShell.Create())
                {
                    powerShell.AddScript(File.ReadAllText(scriptPath));
                    powerShell.Invoke();
                    var command = powerShell.AddCommand("DeleteQueuesForEndpoint");
                    command.AddParameter("EndpointName", endpointName);
                    command.AddParameter("DelayedDeliveryMethod", delayedDeliveryMethod);
                    command.Invoke();

                    command = powerShell.AddCommand("DeleteQueue");
                    command.AddParameter("QueueName", errorQueueName);
                    command.Invoke();

                    command = powerShell.AddCommand("DeleteQueue");
                    command.AddParameter("QueueName", auditQueueName);
                    command.Invoke();
                }

                await AssertQueuesDeleted(endpointName, errorQueueName, auditQueueName, delayedDeliveryMethod: delayedDeliveryMethod);
            }
            finally
            {
                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, delayedDeliveryMethod: delayedDeliveryMethod);
                await QueueDeletionUtils.DeleteQueue(errorQueueName);
                await QueueDeletionUtils.DeleteQueue(auditQueueName);
            }
        }

        [Test]
        [TestCase("TimeoutManager")]
        [TestCase("Native")]
        [TestCase("UnrestrictedDelayedDelivery")]
        public async Task DeleteQueuesForEndpoint(string delayedDeliveryMethod)
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"mydeleteendpoint-{randomName}";
            var errorQueueName = $"mydeleteerror-{randomName}";
            var auditQueueName = $"mydeleteaudit-{randomName}";

            await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName, delayedDeliveryMethod: delayedDeliveryMethod);
            await QueueCreationUtils.CreateQueue(errorQueueName);
            await QueueCreationUtils.CreateQueue(auditQueueName);


            await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, delayedDeliveryMethod: delayedDeliveryMethod);
            await QueueDeletionUtils.DeleteQueue(errorQueueName);
            await QueueDeletionUtils.DeleteQueue(auditQueueName);


            await AssertQueuesDeleted(endpointName, errorQueueName, auditQueueName);
        }

        [Test]
        [TestCase("TimeoutManager")]
        [TestCase("Native")]
        [TestCase("UnrestrictedDelayedDelivery")]
        public async Task DeleteQueueForEndpointWithPrefix_Powershell(string delayedDeliveryMethod)
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"mydeleteprefixendpoint-{randomName}-powershell";
            var errorQueueName = $"mydeleteprefixerror-{randomName}-powershell";
            var auditQueueName = $"mydeleteprefixaudit-{randomName}-powershell";

            await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName, queueNamePrefix: "DEV", delayedDeliveryMethod: delayedDeliveryMethod);
            await QueueCreationUtils.CreateQueue(errorQueueName, queueNamePrefix: "DEV");
            await QueueCreationUtils.CreateQueue(auditQueueName, queueNamePrefix: "DEV");

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
                    command.AddParameter("DelayedDeliveryMethod", delayedDeliveryMethod);
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

                await AssertQueuesDeleted(endpointName, errorQueueName, auditQueueName, queueNamePrefix: "DEV", delayedDeliveryMethod: delayedDeliveryMethod);
            }
            finally
            {
                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, queueNamePrefix: "DEV", delayedDeliveryMethod: delayedDeliveryMethod);
                await QueueDeletionUtils.DeleteQueue(errorQueueName, queueNamePrefix: "DEV");
                await QueueDeletionUtils.DeleteQueue(auditQueueName, queueNamePrefix: "DEV");
            }
        }

        [Test]
        [TestCase("TimeoutManager")]
        [TestCase("Native")]
        [TestCase("UnrestrictedDelayedDelivery")]
        public async Task DeleteQueuesForEndpointWithPrefix(string delayedDeliveryMethod)
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"mydeleteprefixendpoint-{randomName}";
            var errorQueueName = $"mydeleteprefixerror-{randomName}";
            var auditQueueName = $"mydeleteprefixaudit-{randomName}";

            await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName, queueNamePrefix: "DEV", delayedDeliveryMethod: delayedDeliveryMethod);
            await QueueCreationUtils.CreateQueue(errorQueueName, queueNamePrefix: "DEV");
            await QueueCreationUtils.CreateQueue(auditQueueName, queueNamePrefix: "DEV");


            await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, queueNamePrefix: "DEV", delayedDeliveryMethod: delayedDeliveryMethod);
            await QueueDeletionUtils.DeleteQueue(errorQueueName, queueNamePrefix: "DEV");
            await QueueDeletionUtils.DeleteQueue(auditQueueName, queueNamePrefix: "DEV");


            await AssertQueuesDeleted(endpointName, errorQueueName, auditQueueName, queueNamePrefix: "DEV", delayedDeliveryMethod: delayedDeliveryMethod);
        }

        [Test]
        [TestCase("TimeoutManager")]
        [TestCase("Native")]
        [TestCase("UnrestrictedDelayedDelivery")]
        public async Task DeleteQueueForEndpointWithRetries_Powershell(string delayedDeliveryMethod)
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"mydeleteretriesendpoint-{randomName}-powershell";
            var errorQueueName = $"mydeleteretrieserror-{randomName}-powershell";
            var auditQueueName = $"mydeleteretriesaudit-{randomName}-powershell";

            await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName, includeRetries: true, delayedDeliveryMethod: delayedDeliveryMethod);
            await QueueCreationUtils.CreateQueue(errorQueueName);
            await QueueCreationUtils.CreateQueue(auditQueueName);

            var scriptPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "QueueDeletion/QueueDeletion.ps1");
            using (var powerShell = PowerShell.Create())
            {
                powerShell.AddScript(File.ReadAllText(scriptPath));
                powerShell.Invoke();
                var command = powerShell.AddCommand("DeleteQueuesForEndpoint");
                command.AddParameter("EndpointName", endpointName);
                command.AddParameter("IncludeRetries");
                command.AddParameter("DelayedDeliveryMethod", delayedDeliveryMethod);
                command.Invoke();

                command = powerShell.AddCommand("DeleteQueue");
                command.AddParameter("QueueName", errorQueueName);
                command.Invoke();

                command = powerShell.AddCommand("DeleteQueue");
                command.AddParameter("QueueName", auditQueueName);
                command.Invoke();
            }

            await AssertQueuesDeleted(endpointName, errorQueueName, auditQueueName, includeRetries: true, delayedDeliveryMethod: delayedDeliveryMethod);
        }

        [Test]
        [TestCase("TimeoutManager")]
        [TestCase("Native")]
        [TestCase("UnrestrictedDelayedDelivery")]
        public async Task DeleteQueuesForEndpointWithRetries(string delayedDeliveryMethod)
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"mydeleteretriesendpoint-{randomName}";
            var errorQueueName = $"mydeleteretrieserror-{randomName}";
            var auditQueueName = $"mydeleteretriesaudit-{randomName}";

            await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName, includeRetries: true, delayedDeliveryMethod: delayedDeliveryMethod);
            await QueueCreationUtils.CreateQueue(errorQueueName);
            await QueueCreationUtils.CreateQueue(auditQueueName);


            await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, includeRetries: true, delayedDeliveryMethod: delayedDeliveryMethod);
            await QueueDeletionUtils.DeleteQueue(errorQueueName);
            await QueueDeletionUtils.DeleteQueue(auditQueueName);


            await AssertQueuesDeleted(endpointName, errorQueueName, auditQueueName, includeRetries: true, delayedDeliveryMethod: delayedDeliveryMethod);
        }

        static async Task AssertQueuesDeleted(string endpointName, string errorQueueName, string auditQueueName, string queueNamePrefix = null, bool includeRetries = false, string delayedDeliveryMethod = "native")
        {
            Assert.That(await QueueExistenceUtils.Exists(endpointName, queueNamePrefix), Is.False, $"Queue {endpointName} still exists.");

            switch (delayedDeliveryMethod)
            {
                case "TimeoutManager":

                    Assert.That(await QueueExistenceUtils.Exists($"{endpointName}.Timeouts", queueNamePrefix), Is.False, $"Queue {endpointName}.timeouts still exists.");

                    Assert.That(await QueueExistenceUtils.Exists($"{endpointName}.TimeoutsDispatcher", queueNamePrefix), Is.False, $"Queue {endpointName}.timeoutsdispatcher still exists.");

                    break;
                case "UnrestrictedDelayedDelivery":

                    Assert.That(await QueueExistenceUtils.Exists($"{endpointName}-delay.fifo", queueNamePrefix), Is.False, $"Queue {endpointName}-delay.fifo still exists.");

                    break;
            }

            if (includeRetries)
            {
                Assert.That(await QueueExistenceUtils.Exists($"{endpointName}.Retries", queueNamePrefix), Is.False, $"Queue {endpointName}.retries still exists.");
            }

            Assert.Multiple(async () =>
            {
                Assert.That(await QueueExistenceUtils.Exists(errorQueueName, queueNamePrefix), Is.False, $"Queue {errorQueueName} still exists.");

                Assert.That(await QueueExistenceUtils.Exists(auditQueueName, queueNamePrefix), Is.False, $"Queue {auditQueueName} still exists.");
            });
        }
    }

}