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
    using SqsAll.QueueDeletion;

    [TestFixture]
    [Explicit]
    public class Tests
    {
        [Test]
        [TestCase("TimeoutManager")]
        [TestCase("Native")]
        [TestCase("UnrestrictedDelayedDelivery")]
        public async Task CreateQueuesForEndpointWithOverridenMaxTTL_Powershell(string delayedDeliveryMethod)
        {
            var maxTimeToLive = TimeSpan.FromDays(1);

            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"mycreateendpoint-powershell-{randomName}";
            var errorQueueName = $"mycreateerror-powershell-{randomName}";
            var auditQueueName = $"mycreateaudit-powershell-{randomName}";

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
                    command.AddParameter("DelayedDeliveryMethod", delayedDeliveryMethod);
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

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, maxTimeToLive, delayedDeliveryMethod: delayedDeliveryMethod);
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
        public async Task CreateQueuesForEndpointWithOverridenMaxTTL(string delayedDeliveryMethod)
        {
            var maxTimeToLive = TimeSpan.FromDays(1);

            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"mycreateendpoint-{randomName}";
            var errorQueueName = $"mycreateerror-{randomName}";
            var auditQueueName = $"mycreateaudit-{randomName}";

            try
            {
                await CreateEndpointQueues.CreateQueuesForEndpoint(
                        endpointName: endpointName,
                        maxTimeToLive: maxTimeToLive,
                        delayedDeliveryMethod: delayedDeliveryMethod);

                await QueueCreationUtils.CreateQueue(
                        queueName: errorQueueName,
                        maxTimeToLive: maxTimeToLive);

                await QueueCreationUtils.CreateQueue(
                        queueName: auditQueueName,
                        maxTimeToLive: maxTimeToLive);

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, maxTimeToLive, delayedDeliveryMethod: delayedDeliveryMethod);
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
        public async Task CreateQueuesForEndpointDefaultMaxTTL_Powershell(string delayedDeliveryMethod)
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"mycreatedefaultendpoint-{randomName}-powershell";
            var errorQueueName = $"mycreatedefaulterror-{randomName}-powershell";
            var auditQueueName = $"mycreatedefaultaudit-{randomName}-powershell";

            try
            {
                var scriptPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "QueueCreation/QueueCreation.ps1");
                using (var powerShell = PowerShell.Create())
                {
                    powerShell.AddScript(File.ReadAllText(scriptPath));
                    powerShell.Invoke();
                    var command = powerShell.AddCommand("CreateQueuesForEndpoint");
                    command.AddParameter("EndpointName", endpointName);
                    command.AddParameter("DelayedDeliveryMethod", delayedDeliveryMethod);
                    command.Invoke();

                    command = powerShell.AddCommand("CreateQueue");
                    command.AddParameter("QueueName", errorQueueName);
                    command.Invoke();

                    command = powerShell.AddCommand("CreateQueue");
                    command.AddParameter("QueueName", auditQueueName);
                    command.Invoke();
                }

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, TimeSpan.FromDays(4), delayedDeliveryMethod: delayedDeliveryMethod);
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
        public async Task CreateQueuesForEndpointDefaultMaxTTL(string delayedDeliveryMethod)
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"mycreatedefaultendpoint-{randomName}";
            var errorQueueName = $"mycreatedefaulterror-{randomName}";
            var auditQueueName = $"mycreatedefaultaudit-{randomName}";

            try
            {
                await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName, delayedDeliveryMethod: delayedDeliveryMethod);

                await QueueCreationUtils.CreateQueue(
                        queueName: errorQueueName);

                await QueueCreationUtils.CreateQueue(
                        queueName: auditQueueName);

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, TimeSpan.FromDays(4), delayedDeliveryMethod: delayedDeliveryMethod);
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
        public async Task CreateQueuesForEndpointDefaultMaxTTL_CloudFormation(string delayedDeliveryMethod)
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"mycreatedefaultendpoint-cloudformation-{randomName}";
            var errorQueueName = $"mycreatedefaulterror-cloudformation-{randomName}";
            var auditQueueName = $"mycreatedefaultaudit-cloudformation-{randomName}";

            var queueCreationTemplatePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "QueueCreation/QueueCreation.json");
            var endpointTemplatePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "QueueCreation/CreateEndpointQueues.json");

            try
            {
                await CreateEndpointQueuesCloudFormation.CreateQueuesForEndpoint(endpointName, endpointTemplatePath, delayedDeliveryMethod:delayedDeliveryMethod);

                await QueueCreationUtilsCloudFormation.CreateQueue(
                        queueName: errorQueueName,
                        templatePath: queueCreationTemplatePath);

                await QueueCreationUtilsCloudFormation.CreateQueue(
                        queueName: auditQueueName,
                        templatePath: queueCreationTemplatePath);

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, TimeSpan.FromDays(4), delayedDeliveryMethod: delayedDeliveryMethod);
            }
            finally
            {
                await DeleteEndpointQueuesCloudFormation.DeleteQueuesForEndpoint(endpointName);
                await QueueDeletionUtilsCloudFormation.DeleteQueue(errorQueueName);
                await QueueDeletionUtilsCloudFormation.DeleteQueue(auditQueueName);
            }
        }

        [Test]
        [TestCase("TimeoutManager")]
        [TestCase("Native")]
        [TestCase("UnrestrictedDelayedDelivery")]
        public async Task CreateQueuesForEndpointWithPrefix_Powershell(string delayedDeliveryMethod)
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"mycreateprefixendpoint-powershell-{randomName}";
            var errorQueueName = $"mycreateprefixerror-powershell-{randomName}";
            var auditQueueName = $"mycreateprefixaudit-powershell-{randomName}";

            try
            {
                var scriptPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "QueueCreation/QueueCreation.ps1");
                using (var powerShell = PowerShell.Create())
                {
                    powerShell.AddScript(File.ReadAllText(scriptPath));
                    powerShell.Invoke();
                    var command = powerShell.AddCommand("CreateQueuesForEndpoint");
                    command.AddParameter("EndpointName", endpointName);
                    command.AddParameter("QueueNamePrefix", "DEV");
                    command.AddParameter("IncludeRetries");
                    command.AddParameter("DelayedDeliveryMethod", delayedDeliveryMethod);
                    command.Invoke();

                    command = powerShell.AddCommand("CreateQueue");
                    command.AddParameter("QueueName", errorQueueName);
                    command.AddParameter("QueueNamePrefix", "DEV");
                    command.Invoke();

                    command = powerShell.AddCommand("CreateQueue");
                    command.AddParameter("QueueName", auditQueueName);
                    command.AddParameter("QueueNamePrefix", "DEV");
                    command.Invoke();
                }

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, TimeSpan.FromDays(4), queueNamePrefix: "DEV");
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
        public async Task CreateQueuesForEndpointWithPrefix(string delayedDeliveryMethod)
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"mycreateprefixendpoint-{randomName}";
            var errorQueueName = $"mycreateprefixerror-{randomName}";
            var auditQueueName = $"mycreateprefixaudit-{randomName}";

            try
            {
                await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName, queueNamePrefix: "DEV", delayedDeliveryMethod: delayedDeliveryMethod);

                await QueueCreationUtils.CreateQueue(
                        queueName: errorQueueName,
                        queueNamePrefix: "DEV");

                await QueueCreationUtils.CreateQueue(
                        queueName: auditQueueName,
                        queueNamePrefix: "DEV");

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, TimeSpan.FromDays(4), queueNamePrefix: "DEV", delayedDeliveryMethod: delayedDeliveryMethod);
            }
            finally
            {
                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, queueNamePrefix: "DEV", delayedDeliveryMethod: delayedDeliveryMethod);
                await QueueDeletionUtils.DeleteQueue(errorQueueName, queueNamePrefix: "DEV");
                await QueueDeletionUtils.DeleteQueue(auditQueueName, queueNamePrefix: "DEV");
            }
        }

        [Test]
        public async Task CreateQueuesForEndpointWithRetries_Powershell()
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"mycreateprefixendpoint-{randomName}";
            var errorQueueName = $"mycreateprefixerror-{randomName}";
            var auditQueueName = $"mycreateprefixaudit-{randomName}";

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

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, TimeSpan.FromDays(4), includeRetries: true);
            }
            finally
            {
                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, includeRetries: true);
                await QueueDeletionUtils.DeleteQueue(errorQueueName);
                await QueueDeletionUtils.DeleteQueue(auditQueueName);
            }
        }

        [Test]
        public async Task CreateQueuesForEndpointWithRetries()
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"mycreateprefixendpoint-{randomName}";
            var errorQueueName = $"mycreateprefixerror-{randomName}";
            var auditQueueName = $"mycreateprefixaudit-{randomName}";

            try
            {
                await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName, includeRetries: true);

                await QueueCreationUtils.CreateQueue(
                        queueName: errorQueueName);

                await QueueCreationUtils.CreateQueue(
                        queueName: auditQueueName);

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, TimeSpan.FromDays(4), includeRetries: true);
            }
            finally
            {
                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, includeRetries: true);
                await QueueDeletionUtils.DeleteQueue(errorQueueName);
                await QueueDeletionUtils.DeleteQueue(auditQueueName);
            }
        }

        [Test]
        public void CreateQueuesForEndpointWithLongEndpointNameThrowsAndNoQueuesAreCreated()
        {
            // Below is 76 chars - this is fine for the main queue but too long for the retries, timeouts or dispatcher queues
            var endpointName = "mycreatequeuesendpointwithaverylongnamethatwillhavequeuenamesthataretoolong";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName, includeRetries: true);
            });

            Assert.That(exception.Message, Does.Contain("is longer than 80 characters and therefore cannot be used to create an SQS queue."));
        }

        [Test]
        [TestCase("TimeoutManager")]
        [TestCase("Native")]
        [TestCase("UnrestrictedDelayedDelivery")]
        public async Task CreateQueuesForEndpointWithRetries_CloudFormation(string delayedDeliveryMethod)
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"mycreateretriesendpoint-cloudformation-{randomName}";
            var errorQueueName = $"mycreateretriesendpointerror-cloudformation-{randomName}";
            var auditQueueName = $"mycreateretriesendpointaudit-cloudformation-{randomName}";
            var queueCreationTemplatePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "QueueCreation/QueueCreation.json");
            var endpointTemplatePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "QueueCreation/CreateEndpointQueues.json");

            try
            {
                await CreateEndpointQueuesCloudFormation.CreateQueuesForEndpoint(endpointName, endpointTemplatePath, includeRetries: true, delayedDeliveryMethod: delayedDeliveryMethod);

                await QueueCreationUtilsCloudFormation.CreateQueue(
                        queueName: errorQueueName,
                        templatePath: queueCreationTemplatePath);

                await QueueCreationUtilsCloudFormation.CreateQueue(
                        queueName: auditQueueName,
                        templatePath: queueCreationTemplatePath);

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, TimeSpan.FromDays(4), includeRetries: true, delayedDeliveryMethod:delayedDeliveryMethod);
            }
            finally
            {
                await DeleteEndpointQueuesCloudFormation.DeleteQueuesForEndpoint(endpointName);
                await QueueDeletionUtilsCloudFormation.DeleteQueue(errorQueueName);
                await QueueDeletionUtilsCloudFormation.DeleteQueue(auditQueueName);
            }
        }

        static async Task AssertQueuesExist(string endpointName, string errorQueueName, string auditQueueName, TimeSpan maxTimeToLive, string queueNamePrefix = null, bool includeRetries = false, string delayedDeliveryMethod = "Native")
        {
            var timeSpanInSeconds = Convert.ToInt32(maxTimeToLive.TotalSeconds);

            Assert.Multiple(async () =>
            {
                Assert.That(await QueueExistenceUtils.Exists(endpointName, queueNamePrefix), Is.True, "Endpoint Queue did not exist");
                Assert.That((await QueueAccessUtils.Exists(endpointName, queueNamePrefix, new List<string>
            {
                QueueAttributeName.MessageRetentionPeriod
            })).MessageRetentionPeriod, Is.EqualTo(timeSpanInSeconds));
            });

            switch (delayedDeliveryMethod)
            {
                case "TimeoutManager":

                    Assert.That(await QueueExistenceUtils.Exists($"{endpointName}.Timeouts", queueNamePrefix), Is.True, "Timeouts Queue did not exist");
                    Assert.That((await QueueAccessUtils.Exists(endpointName, queueNamePrefix, new List<string>
                    {
                        QueueAttributeName.MessageRetentionPeriod
                    })).MessageRetentionPeriod, Is.EqualTo(timeSpanInSeconds));

                    Assert.That(await QueueExistenceUtils.Exists($"{endpointName}.TimeoutsDispatcher", queueNamePrefix), Is.True, "TimeoutsDispatcher Queue did not exist");
                    Assert.That((await QueueAccessUtils.Exists(endpointName, queueNamePrefix, new List<string>
                    {
                        QueueAttributeName.MessageRetentionPeriod
                    })).MessageRetentionPeriod, Is.EqualTo(timeSpanInSeconds));

                    break;
                case "UnrestrictedDelayedDelivery":

                    var endpointFifoQueueName = $"{endpointName}-delay.fifo";

                    Assert.That(await QueueExistenceUtils.Exists(endpointFifoQueueName, queueNamePrefix), Is.True, "Endpoint FIFO Queue did not exist");

                    var queueAttributes = await QueueAccessUtils.Exists(endpointFifoQueueName, queueNamePrefix, new List<string>
                    {
                        QueueAttributeName.MessageRetentionPeriod,
                        QueueAttributeName.DelaySeconds,
                        QueueAttributeName.FifoQueue
                    });

                    Assert.That(queueAttributes.MessageRetentionPeriod, Is.EqualTo(timeSpanInSeconds));
                    Assert.That(queueAttributes.DelaySeconds, Is.EqualTo(900));
                    Assert.That(queueAttributes.FifoQueue, Is.True);

                    break;
            }

            if (includeRetries)
            {
                Assert.Multiple(async () =>
                {
                    Assert.That(await QueueExistenceUtils.Exists($"{endpointName}.Retries", queueNamePrefix), Is.True, "Retries Queue did not exist");
                    Assert.That((await QueueAccessUtils.Exists(endpointName, queueNamePrefix, new List<string>
                {
                    QueueAttributeName.MessageRetentionPeriod
                })).MessageRetentionPeriod, Is.EqualTo(timeSpanInSeconds));
                });
            }

            Assert.Multiple(async () =>
            {
                Assert.That(await QueueExistenceUtils.Exists(errorQueueName, queueNamePrefix), Is.True);
                Assert.That((await QueueAccessUtils.Exists(errorQueueName, queueNamePrefix, new List<string>
            {
                QueueAttributeName.MessageRetentionPeriod
            })).MessageRetentionPeriod, Is.EqualTo(timeSpanInSeconds));

                Assert.That(await QueueExistenceUtils.Exists(auditQueueName, queueNamePrefix), Is.True);
                Assert.That((await QueueAccessUtils.Exists(auditQueueName, queueNamePrefix, new List<string>
            {
                QueueAttributeName.MessageRetentionPeriod
            })).MessageRetentionPeriod, Is.EqualTo(timeSpanInSeconds));
            });
        }

        [Test]
        [TestCase("Native")]
        [TestCase("UnrestrictedDelayedDelivery")]
        public async Task CreateQueues(string delayedDeliveryMethod)
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"createqueues-{randomName}";
            var errorQueueName = $"createqueues-{randomName}-error";
            var auditQueueName = $"createqueues-{randomName}-audit";

            var state = new State();
            IEndpointInstance endpoint = null;
            try
            {
                await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName, includeRetries: true, delayedDeliveryMethod: delayedDeliveryMethod);

                await QueueCreationUtils.CreateQueue(
                        queueName: errorQueueName);

                await QueueCreationUtils.CreateQueue(
                        queueName: auditQueueName);

                endpoint = await StartEndpoint(state, endpointName, errorQueueName, auditQueueName, delayedDeliveryMethod);
                var messageToSend = new MessageToSend();
                await endpoint.SendLocal(messageToSend);

                Assert.That(await state.Signal.Task, Is.True);
            }
            finally
            {
                if (endpoint != null)
                {
                    await endpoint.Stop();
                }

                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, includeRetries: true, delayedDeliveryMethod: delayedDeliveryMethod);
                await QueueDeletionUtils.DeleteQueue(errorQueueName);
                await QueueDeletionUtils.DeleteQueue(auditQueueName);
            }
        }

        [Test]
        [TestCase("Native")]
        [TestCase("UnrestrictedDelayedDelivery")]
        public async Task CreateQueues_Powershell(string delayedDeliveryMethod)
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"createqueues-powershell-{randomName}";
            var errorQueueName = $"createqueueserror-powershell-{randomName}";
            var auditQueueName = $"createqueuesaudit-powershell-{randomName}";

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
                    command.AddParameter("DelayedDeliveryMethod", delayedDeliveryMethod);
                    command.Invoke();

                    command = powerShell.AddCommand("CreateQueue");
                    command.AddParameter("QueueName", errorQueueName);
                    command.Invoke();

                    command = powerShell.AddCommand("CreateQueue");
                    command.AddParameter("QueueName", auditQueueName);
                    command.Invoke();
                }

                endpoint = await StartEndpoint(state, endpointName, errorQueueName, auditQueueName, delayedDeliveryMethod);
                var messageToSend = new MessageToSend();
                await endpoint.SendLocal(messageToSend);

                Assert.That(await state.Signal.Task, Is.True);
            }
            finally
            {
                if (endpoint != null)
                {
                    await endpoint.Stop();
                }

                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, includeRetries: true, delayedDeliveryMethod: delayedDeliveryMethod);
                await QueueDeletionUtils.DeleteQueue(errorQueueName);
                await QueueDeletionUtils.DeleteQueue(auditQueueName);
            }
        }

        Task<IEndpointInstance> StartEndpoint(State state, string endpointName, string errorQueueName, string auditQueueName, string delayedDeliveryMethod)
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

            if (delayedDeliveryMethod == "UnrestrictedDelayedDelivery")
            {
                transport.UnrestrictedDurationDelayedDelivery();
            }

            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            var recoverabilitySettings = endpointConfiguration.Recoverability();
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