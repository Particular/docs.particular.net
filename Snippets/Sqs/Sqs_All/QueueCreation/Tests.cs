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

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, maxTimeToLive, delayedDeliveryMethod: delayedDeliveryMethod)
                    .ConfigureAwait(false);
            }
            finally
            {
                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, delayedDeliveryMethod: delayedDeliveryMethod)
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(errorQueueName)
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(auditQueueName)
                    .ConfigureAwait(false);
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

            await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, delayedDeliveryMethod: delayedDeliveryMethod)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(errorQueueName)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(auditQueueName)
                .ConfigureAwait(false);

            try
            {
                await CreateEndpointQueues.CreateQueuesForEndpoint(
                        endpointName: endpointName,
                        maxTimeToLive: maxTimeToLive,
                        delayedDeliveryMethod: delayedDeliveryMethod)
                    .ConfigureAwait(false);

                await QueueCreationUtils.CreateQueue(
                        queueName: errorQueueName,
                        maxTimeToLive: maxTimeToLive)
                    .ConfigureAwait(false);

                await QueueCreationUtils.CreateQueue(
                        queueName: auditQueueName,
                        maxTimeToLive: maxTimeToLive)
                    .ConfigureAwait(false);

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, maxTimeToLive, delayedDeliveryMethod: delayedDeliveryMethod)
                    .ConfigureAwait(false);
            }
            finally
            {
                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, delayedDeliveryMethod: delayedDeliveryMethod)
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(errorQueueName)
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(auditQueueName)
                    .ConfigureAwait(false);
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

            await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, delayedDeliveryMethod: delayedDeliveryMethod)
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
                    command.AddParameter("DelayedDeliveryMethod", delayedDeliveryMethod);
                    command.Invoke();

                    command = powerShell.AddCommand("CreateQueue");
                    command.AddParameter("QueueName", errorQueueName);
                    command.Invoke();

                    command = powerShell.AddCommand("CreateQueue");
                    command.AddParameter("QueueName", auditQueueName);
                    command.Invoke();
                }

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, TimeSpan.FromDays(4), delayedDeliveryMethod: delayedDeliveryMethod)
                    .ConfigureAwait(false);
            }
            finally
            {
                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, delayedDeliveryMethod: delayedDeliveryMethod)
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(errorQueueName)
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(auditQueueName)
                    .ConfigureAwait(false);
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

            await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, delayedDeliveryMethod: delayedDeliveryMethod)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(errorQueueName)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(auditQueueName)
                .ConfigureAwait(false);

            try
            {
                await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName, delayedDeliveryMethod: delayedDeliveryMethod)
                    .ConfigureAwait(false);

                await QueueCreationUtils.CreateQueue(
                        queueName: errorQueueName)
                    .ConfigureAwait(false);

                await QueueCreationUtils.CreateQueue(
                        queueName: auditQueueName)
                    .ConfigureAwait(false);

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, TimeSpan.FromDays(4), delayedDeliveryMethod: delayedDeliveryMethod)
                    .ConfigureAwait(false);
            }
            finally
            {
                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, delayedDeliveryMethod: delayedDeliveryMethod)
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(errorQueueName)
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(auditQueueName)
                    .ConfigureAwait(false);
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
                await CreateEndpointQueuesCloudFormation.CreateQueuesForEndpoint(endpointName, endpointTemplatePath, delayedDeliveryMethod:delayedDeliveryMethod)
                    .ConfigureAwait(false);

                await QueueCreationUtilsCloudFormation.CreateQueue(
                        queueName: errorQueueName,
                        templatePath: queueCreationTemplatePath)
                    .ConfigureAwait(false);

                await QueueCreationUtilsCloudFormation.CreateQueue(
                        queueName: auditQueueName,
                        templatePath: queueCreationTemplatePath)
                    .ConfigureAwait(false);

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, TimeSpan.FromDays(4), delayedDeliveryMethod: delayedDeliveryMethod)
                    .ConfigureAwait(false);
            }
            finally
            {
                await DeleteEndpointQueuesCloudFormation.DeleteQueuesForEndpoint(endpointName)
                    .ConfigureAwait(false);
                await QueueDeletionUtilsCloudFormation.DeleteQueue(errorQueueName)
                    .ConfigureAwait(false);
                await QueueDeletionUtilsCloudFormation.DeleteQueue(auditQueueName)
                    .ConfigureAwait(false);
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

            await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, queueNamePrefix: "DEV", delayedDeliveryMethod: delayedDeliveryMethod)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(errorQueueName, queueNamePrefix: "DEV")
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(auditQueueName, queueNamePrefix: "DEV")
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

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, TimeSpan.FromDays(4), queueNamePrefix: "DEV")
                    .ConfigureAwait(false);
            }
            finally
            {
                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, queueNamePrefix: "DEV", delayedDeliveryMethod: delayedDeliveryMethod)
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(errorQueueName, queueNamePrefix: "DEV")
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(auditQueueName, queueNamePrefix: "DEV")
                    .ConfigureAwait(false);
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

            await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, queueNamePrefix: "DEV", delayedDeliveryMethod: delayedDeliveryMethod)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(errorQueueName, queueNamePrefix: "DEV")
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(auditQueueName, queueNamePrefix: "DEV")
                .ConfigureAwait(false);

            try
            {
                await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName, queueNamePrefix: "DEV", delayedDeliveryMethod: delayedDeliveryMethod)
                    .ConfigureAwait(false);

                await QueueCreationUtils.CreateQueue(
                        queueName: errorQueueName, 
                        queueNamePrefix: "DEV")
                    .ConfigureAwait(false);

                await QueueCreationUtils.CreateQueue(
                        queueName: auditQueueName, 
                        queueNamePrefix: "DEV")
                    .ConfigureAwait(false);

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, TimeSpan.FromDays(4), queueNamePrefix: "DEV", delayedDeliveryMethod: delayedDeliveryMethod)
                    .ConfigureAwait(false);
            }
            finally
            {
                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, queueNamePrefix: "DEV", delayedDeliveryMethod: delayedDeliveryMethod)
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(errorQueueName, queueNamePrefix: "DEV")
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(auditQueueName, queueNamePrefix: "DEV")
                    .ConfigureAwait(false);
            }
        }

        [Test]
        public async Task CreateQueuesForEndpointWithRetries_Powershell()
        {

            await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, includeRetries: true)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(errorQueueName)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(auditQueueName)
                .ConfigureAwait(false);
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

            await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, includeRetries: true)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(errorQueueName)
                .ConfigureAwait(false);
            await QueueDeletionUtils.DeleteQueue(auditQueueName)
                .ConfigureAwait(false);
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"mycreateprefixendpoint-{randomName}";
            var errorQueueName = $"mycreateprefixerror-{randomName}";
            var auditQueueName = $"mycreateprefixaudit-{randomName}";

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

        [Test]
        public void CreateQueuesForEndpointWithLongEndpointNameThrowsAndNoQueuesAreCreated()
        {
            // Below is 76 chars - this is fine for the main queue but too long for the retries, timeouts or dispatcher queues
            var endpointName = "mycreatequeuesendpointwithaverylongnamethatwillhavequeuenamesthataretoolong";

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await CreateEndpointQueues.CreateQueuesForEndpoint(endpointName, includeRetries: true)
                    .ConfigureAwait(false);
            });

            Assert.IsTrue(exception.Message.Contains("is longer than 80 characters and therefore cannot be used to create an SQS queue."));
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
                await CreateEndpointQueuesCloudFormation.CreateQueuesForEndpoint(endpointName, endpointTemplatePath, includeRetries: true, delayedDeliveryMethod: delayedDeliveryMethod)
                    .ConfigureAwait(false);

                await QueueCreationUtilsCloudFormation.CreateQueue(
                        queueName: errorQueueName,
                        templatePath: queueCreationTemplatePath)
                    .ConfigureAwait(false);

                await QueueCreationUtilsCloudFormation.CreateQueue(
                        queueName: auditQueueName,
                        templatePath: queueCreationTemplatePath)
                    .ConfigureAwait(false);

                await AssertQueuesExist(endpointName, errorQueueName, auditQueueName, TimeSpan.FromDays(4), includeRetries: true, delayedDeliveryMethod:delayedDeliveryMethod)
                    .ConfigureAwait(false);
            }
            finally
            {
                await DeleteEndpointQueuesCloudFormation.DeleteQueuesForEndpoint(endpointName)
                    .ConfigureAwait(false);
                await QueueDeletionUtilsCloudFormation.DeleteQueue(errorQueueName)
                    .ConfigureAwait(false);
                await QueueDeletionUtilsCloudFormation.DeleteQueue(auditQueueName)
                    .ConfigureAwait(false);
            }
        }

        static async Task AssertQueuesExist(string endpointName, string errorQueueName, string auditQueueName, TimeSpan maxTimeToLive, string queueNamePrefix = null, bool includeRetries = false, string delayedDeliveryMethod = "Native")
        {
            var timeSpanInSeconds = Convert.ToInt32(maxTimeToLive.TotalSeconds);

            Assert.IsTrue(await QueueExistenceUtils.Exists(endpointName, queueNamePrefix), "Endpoint Queue did not exist");
            Assert.AreEqual(timeSpanInSeconds, (await QueueAccessUtils.Exists(endpointName, queueNamePrefix, new List<string>
            {
                QueueAttributeName.MessageRetentionPeriod
            })).MessageRetentionPeriod);

            switch (delayedDeliveryMethod)
            {
                case "TimeoutManager":

                    Assert.IsTrue(await QueueExistenceUtils.Exists($"{endpointName}.Timeouts", queueNamePrefix), "Timeouts Queue did not exist");
                    Assert.AreEqual(timeSpanInSeconds, (await QueueAccessUtils.Exists(endpointName, queueNamePrefix, new List<string>
                    {
                        QueueAttributeName.MessageRetentionPeriod
                    })).MessageRetentionPeriod);

                    Assert.IsTrue(await QueueExistenceUtils.Exists($"{endpointName}.TimeoutsDispatcher", queueNamePrefix), "TimeoutsDispatcher Queue did not exist");
                    Assert.AreEqual(timeSpanInSeconds, (await QueueAccessUtils.Exists(endpointName, queueNamePrefix, new List<string>
                    {
                        QueueAttributeName.MessageRetentionPeriod
                    })).MessageRetentionPeriod);

                    break;
                case "UnrestrictedDelayedDelivery":

                    var endpointFifoQueueName = $"{endpointName}-delay.fifo";

                    Assert.IsTrue(await QueueExistenceUtils.Exists(endpointFifoQueueName, queueNamePrefix), "Endpoint FIFO Queue did not exist");

                    var queueAttributes = await QueueAccessUtils.Exists(endpointFifoQueueName, queueNamePrefix, new List<string>
                    {
                        QueueAttributeName.MessageRetentionPeriod,
                        QueueAttributeName.DelaySeconds,
                        QueueAttributeName.FifoQueue
                    });

                    Assert.AreEqual(timeSpanInSeconds, queueAttributes.MessageRetentionPeriod);
                    Assert.AreEqual(900, queueAttributes.DelaySeconds);
                    Assert.IsTrue(queueAttributes.FifoQueue);

                    break;
            }

            if (includeRetries)
            {
                Assert.IsTrue(await QueueExistenceUtils.Exists($"{endpointName}.Retries", queueNamePrefix), "Retries Queue did not exist");
                Assert.AreEqual(timeSpanInSeconds, (await QueueAccessUtils.Exists(endpointName, queueNamePrefix, new List<string>
                {
                    QueueAttributeName.MessageRetentionPeriod
                })).MessageRetentionPeriod);
            }

            Assert.IsTrue(await QueueExistenceUtils.Exists(errorQueueName, queueNamePrefix));
            Assert.AreEqual(timeSpanInSeconds, (await QueueAccessUtils.Exists(errorQueueName, queueNamePrefix, new List<string>
            {
                QueueAttributeName.MessageRetentionPeriod
            })).MessageRetentionPeriod);

            Assert.IsTrue(await QueueExistenceUtils.Exists(auditQueueName, queueNamePrefix));
            Assert.AreEqual(timeSpanInSeconds, (await QueueAccessUtils.Exists(auditQueueName, queueNamePrefix, new List<string>
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
        public async Task CreateQueues_Powershell()
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