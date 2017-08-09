namespace SqsAll.QueueCreation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Management.Automation;
    using System.Threading;
    using System.Threading.Tasks;
    using Amazon.SQS;
    using NServiceBus;
    using NUnit.Framework;
    using SqsAll.ErrorQueue;
    using SqsAll.QueueDeletion;

    [TestFixture]
    [Explicit]
    public class Tests
    {

        [Test]
        public async Task CreateQueuesForEndpointWithOverridenMaxTTL_Powershell()
        {
            var maxTimeToLive = TimeSpan.FromDays(1);
            var endpointName = "myendpoint-powershell";
            var errorQueueName = "myerror-powershell";
            var auditQueueName = "myaudit-powershell";

            await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(errorQueueName)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(auditQueueName)
                .ConfigureAwait(false);

            try
            {
                var scriptPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "QueueCreation/QueueCreation.ps1");
                using (var powerShell = PowerShell.Create())
                {
                    powerShell.AddScript(File.ReadAllText(scriptPath));
                    powerShell.Invoke();
                    var command = powerShell.AddCommand("CreateQueuesForEndpoint");
                    command.AddParameter("EndpointName", endpointName);
                    command.AddParameter("MaxTimeToLive", maxTimeToLive.ToString("G", CultureInfo.InvariantCulture));
                    command.Invoke();

                    command = powerShell.AddCommand("CreateQueue");
                    command.AddParameter("QueueName", errorQueueName);
                    command.AddParameter("MaxTimeToLive", maxTimeToLive.ToString("G", CultureInfo.InvariantCulture));
                    command.Invoke();

                    command = powerShell.AddCommand("CreateQueue");
                    command.AddParameter("QueueName", auditQueueName);
                    command.AddParameter("MaxTimeToLive", maxTimeToLive.ToString("G", CultureInfo.InvariantCulture));
                    command.Invoke();
                }

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, maxTimeToLive)
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
        public async Task CreateQueuesForEndpointWithOverridenMaxTTL()
        {
            var maxTimeToLive = TimeSpan.FromDays(1);
            var endpointName = "myendpoint";
            var errorQueueName = "myerror";
            var auditQueueName = "myaudit";

            await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(errorQueueName)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(auditQueueName)
                .ConfigureAwait(false);

            try
            {
                await CreateEndpointQueues.CreateQueuesForEndpoint(
                        endpointName: endpointName,
                        maxTimeToLive: maxTimeToLive)
                    .ConfigureAwait(false);

                await QueueCreationUtils.CreateQueue(
                        queueName: errorQueueName,
                        maxTimeToLive: maxTimeToLive)
                    .ConfigureAwait(false);

                await QueueCreationUtils.CreateQueue(
                        queueName: auditQueueName,
                        maxTimeToLive: maxTimeToLive)
                    .ConfigureAwait(false);

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, maxTimeToLive)
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
        public async Task CreateQueuesForEndpointDefaultMaxTTL_Powershell()
        {
            var endpointName = "mydefaultendpoint-powershell";
            var errorQueueName = "mydefaulterror-powershell";
            var auditQueueName = "mydefaultaudit-powershell";

            await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(errorQueueName)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(auditQueueName)
                .ConfigureAwait(false);

            try
            {
                var scriptPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "QueueCreation/QueueCreation.ps1");
                using (var powerShell = PowerShell.Create())
                {
                    powerShell.AddScript(File.ReadAllText(scriptPath));
                    powerShell.Invoke();
                    var command = powerShell.AddCommand("CreateQueuesForEndpoint");
                    command.AddParameter("EndpointName", endpointName);
                    command.Invoke();

                    command = powerShell.AddCommand("CreateQueue");
                    command.AddParameter("QueueName", errorQueueName);
                    command.Invoke();

                    command = powerShell.AddCommand("CreateQueue");
                    command.AddParameter("QueueName", auditQueueName);
                    command.Invoke();
                }

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, TimeSpan.FromDays(4))
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
        public async Task CreateQueuesForEndpointDefaultMaxTTL()
        {
            var endpointName = "mydefaultendpoint";
            var errorQueueName = "mydefaulterror";
            var auditQueueName = "mydefaultaudit";

            await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(errorQueueName)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(auditQueueName)
                .ConfigureAwait(false);

            try
            {
                await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName)
                    .ConfigureAwait(false);

                await QueueCreationUtils.CreateQueue(
                        queueName: errorQueueName)
                    .ConfigureAwait(false);

                await QueueCreationUtils.CreateQueue(
                        queueName: auditQueueName)
                    .ConfigureAwait(false);

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, TimeSpan.FromDays(4))
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
        public async Task CreateQueuesForEndpointWithRetries_Powershell()
        {
            var endpointName = "mydefaultendpoint-powershell";
            var errorQueueName = "mydefaulterror-powershell";
            var auditQueueName = "mydefaultaudit-powershell";

            await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, includeRetries: true)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(errorQueueName)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(auditQueueName)
                .ConfigureAwait(false);

            try
            {
                var scriptPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "QueueCreation/QueueCreation.ps1");
                using (var powerShell = PowerShell.Create())
                {
                    powerShell.AddScript(File.ReadAllText(scriptPath));
                    powerShell.Invoke();
                    var command = powerShell.AddCommand("CreateQueuesForEndpoint");
                    command.AddParameter("EndpointName", endpointName);
                    command.AddParameter("IncludeRetries");
                    command.Invoke();

                    command = powerShell.AddCommand("CreateQueue");
                    command.AddParameter("QueueName", errorQueueName);
                    command.Invoke();

                    command = powerShell.AddCommand("CreateQueue");
                    command.AddParameter("QueueName", auditQueueName);
                    command.Invoke();
                }

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, TimeSpan.FromDays(4), includeRetries: true)
                    .ConfigureAwait(false);
            }
            finally
            {
                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, includeRetries: true)
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(errorQueueName)
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(auditQueueName)
                    .ConfigureAwait(false);
            }
        }

        [Test]
        public async Task CreateQueuesForEndpointWithRetries()
        {
            var endpointName = "mydefaultendpoint";
            var errorQueueName = "mydefaulterror";
            var auditQueueName = "mydefaultaudit";

            await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, includeRetries: true)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(errorQueueName)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(auditQueueName)
                .ConfigureAwait(false);

            try
            {
                await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName, includeRetries: true)
                    .ConfigureAwait(false);

                await QueueCreationUtils.CreateQueue(
                        queueName: errorQueueName)
                    .ConfigureAwait(false);

                await QueueCreationUtils.CreateQueue(
                        queueName: auditQueueName)
                    .ConfigureAwait(false);

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, TimeSpan.FromDays(4), includeRetries: true)
                    .ConfigureAwait(false);
            }
            finally
            {
                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, includeRetries: true)
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(errorQueueName)
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(auditQueueName)
                    .ConfigureAwait(false);
            }
        }

        static async Task AssertQueuesExist(string endpointName, string errorQueueName, string auditQueueName, TimeSpan maxTimeToLive, bool includeRetries = false)
        {
            var timeSpanInSeconds = Convert.ToInt32(maxTimeToLive.TotalSeconds);

            Assert.IsTrue(await QueueExistenceUtils.Exists(endpointName));
            Assert.AreEqual(timeSpanInSeconds, (await QueueAccessUtils.Exists(endpointName, new List<string>
            {
                QueueAttributeName.MessageRetentionPeriod
            })).MessageRetentionPeriod);

            Assert.IsTrue(await QueueExistenceUtils.Exists($"{endpointName}.Timeouts"));
            Assert.AreEqual(timeSpanInSeconds, (await QueueAccessUtils.Exists(endpointName, new List<string>
            {
                QueueAttributeName.MessageRetentionPeriod
            })).MessageRetentionPeriod);

            Assert.IsTrue(await QueueExistenceUtils.Exists($"{endpointName}.TimeoutsDispatcher"));
            Assert.AreEqual(timeSpanInSeconds, (await QueueAccessUtils.Exists(endpointName, new List<string>
            {
                QueueAttributeName.MessageRetentionPeriod
            })).MessageRetentionPeriod);

            if (includeRetries)
            {
                Assert.IsTrue(await QueueExistenceUtils.Exists($"{endpointName}.Retries"));
                Assert.AreEqual(timeSpanInSeconds, (await QueueAccessUtils.Exists(endpointName, new List<string>
                {
                    QueueAttributeName.MessageRetentionPeriod
                })).MessageRetentionPeriod);
            }

            Assert.IsTrue(await QueueExistenceUtils.Exists(errorQueueName));
            Assert.AreEqual(timeSpanInSeconds, (await QueueAccessUtils.Exists(errorQueueName, new List<string>
            {
                QueueAttributeName.MessageRetentionPeriod
            })).MessageRetentionPeriod);

            Assert.IsTrue(await QueueExistenceUtils.Exists(auditQueueName));
            Assert.AreEqual(timeSpanInSeconds, (await QueueAccessUtils.Exists(auditQueueName, new List<string>
            {
                QueueAttributeName.MessageRetentionPeriod
            })).MessageRetentionPeriod);
        }

        string endpointName = "CreateQueuesTests";
        static string errorQueueName = "CreateQueuesTestsError";
        static string auditQueueName = "CreateQueuesTestsAudit";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            DeleteEndpointQueues.DeleteQueuesForEndpoint(QueueNameHelper.GetSqsQueueName(endpointName)).GetAwaiter().GetResult();
            QueueDeletionUtils.DeleteQueue(QueueNameHelper.GetSqsQueueName(errorQueueName)).GetAwaiter().GetResult();
        }

        [Test]
        public async Task CreateQueues()
        {
            var state = new State();
            IEndpointInstance endpoint = null;
            try
            {
                await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName, includeRetries: true)
                    .ConfigureAwait(false);

                await QueueCreationUtils.CreateQueue(
                        queueName: errorQueueName)
                    .ConfigureAwait(false);

                await QueueCreationUtils.CreateQueue(
                        queueName: auditQueueName)
                    .ConfigureAwait(false);

                endpoint = await StartEndpoint(state).ConfigureAwait(false);
                var messageToSend = new MessageToSend();
                await endpoint.SendLocal(messageToSend).ConfigureAwait(false);

                Assert.IsTrue(await state.Signal.Task.ConfigureAwait(false));
            }
            finally
            {
                if (endpoint != null)
                {
                    await endpoint.Stop().ConfigureAwait(false);
                }
            }
        }

        [Test]
        public async Task CreateQueuesPowershell()
        {
            var state = new State();
            IEndpointInstance endpoint = null;
            try
            {
                var scriptPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "QueueCreation/QueueCreation.ps1");
                using (var powerShell = PowerShell.Create())
                {
                    powerShell.AddScript(File.ReadAllText(scriptPath));
                    powerShell.Invoke();
                    var command = powerShell.AddCommand("CreateQueuesForEndpoint");
                    command.AddParameter("EndpointName", endpointName);
                    command.AddParameter("IncludeRetries");
                    command.Invoke();

                    command = powerShell.AddCommand("CreateQueue");
                    command.AddParameter("QueueName", errorQueueName);
                    command.Invoke();

                    command = powerShell.AddCommand("CreateQueue");
                    command.AddParameter("QueueName", auditQueueName);
                    command.Invoke();
                }

                endpoint = await StartEndpoint(state).ConfigureAwait(false);
                var messageToSend = new MessageToSend();
                await endpoint.SendLocal(messageToSend).ConfigureAwait(false);

                Assert.IsTrue(await state.Signal.Task.ConfigureAwait(false));
            }
            finally
            {
                if (endpoint != null)
                {
                    await endpoint.Stop().ConfigureAwait(false);
                }
            }
        }

        Task<IEndpointInstance> StartEndpoint(State state)
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            endpointConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent(x => state, DependencyLifecycle.SingleInstance);
                });
            endpointConfiguration.SendFailedMessagesTo(errorQueueName);
            endpointConfiguration.AuditProcessedMessagesTo(auditQueueName);
            var transport = endpointConfiguration.UseTransport<SqsTransport>();
            transport.ConfigureSqsTransport();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            var recoverabilitySettings = endpointConfiguration.Recoverability();
            recoverabilitySettings.DisableLegacyRetriesSatellite();
            recoverabilitySettings.Immediate(customizations => customizations.NumberOfRetries(0));
            recoverabilitySettings.Delayed(customizations => customizations.NumberOfRetries(0));

            return Endpoint.Start(endpointConfiguration);
        }

        class State
        {
            public State()
            {
                Signal = new TaskCompletionSource<bool>();
                // make sure the test does not hang forever
                var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(60));
                tokenSource.Token.Register(() => Signal.TrySetResult(false));
            }

            public TaskCompletionSource<bool> Signal;
        }

        class MessageHandler :
            IHandleMessages<MessageToSend>
        {
            State state;

            public MessageHandler(State state)
            {
                this.state = state;
            }

            public Task Handle(MessageToSend message, IMessageHandlerContext context)
            {
                state.Signal.TrySetResult(true);
                return Task.CompletedTask;
            }
        }

        class MessageToSend :
            IMessage
        {
        }
    }
}