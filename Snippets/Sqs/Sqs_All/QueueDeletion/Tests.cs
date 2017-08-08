namespace SqsAll.QueueDeletion
{
    using System;
    using System.IO;
    using System.Management.Automation;
    using System.Threading.Tasks;
    using SqsAll.QueueCreation;
    using NUnit.Framework;
    using Sqs_All;

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
                    if (i % 200 == 0) // make sure we don't get throttled
                    {
                        await Task.Delay(TimeSpan.FromSeconds(30));
                    }

                    creationTasks[i] = client.CreateQueueAsync($"delete-{i}");
                }

                creationTasks[2000] = Task.Delay(TimeSpan.FromSeconds(60));

                await Task.WhenAll(creationTasks)
                    .ConfigureAwait(false);
            }

            await QueueDeletionUtils.DeleteAllQueues()
                .ConfigureAwait(false);
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