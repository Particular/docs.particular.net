namespace SqsAll.NativeSend
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Management.Automation;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;
    using NUnit.Framework;
    using SqsAll.QueueDeletion;

    [TestFixture]
    [Explicit]
    public class Tests
    {
        static Tests()
        {
            LogManager.Use<DefaultFactory>().Level(LogLevel.Error);
        }

        string endpointName = "NativeSendTests";
        static string errorQueueName = "NativeSendTestsError";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            DeleteEndpointQueues.DeleteQueuesForEndpoint(QueueNameHelper.GetSqsQueueName(endpointName)).GetAwaiter().GetResult();
            QueueDeletionUtils.DeleteQueue(QueueNameHelper.GetSqsQueueName(errorQueueName)).GetAwaiter().GetResult();
        }

        [Test]
        public async Task Send()
        {
            var state = new State();
            IEndpointInstance endpoint = null;
            try
            {
                endpoint = await StartEndpoint(state).ConfigureAwait(false);

                var message = @"{ Property: 'Value' }";

                var headers = new Dictionary<string, string>
                {
                    {"NServiceBus.EnclosedMessageTypes", typeof(MessageToSend).FullName},
                    {"NServiceBus.MessageId", Guid.NewGuid().ToString()}
                };

                using (var client = ClientFactory.CreateSqsClient())
                {
                    await NativeSend.SendMessage(client, endpointName, message, headers)
                        .ConfigureAwait(false);
                }

                Assert.AreEqual("Value", await state.Signal.Task.ConfigureAwait(false));
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
        public async Task SendPowerShell()
        {
            var state = new State();
            IEndpointInstance endpoint = null;
            try
            {
                endpoint = await StartEndpoint(state).ConfigureAwait(false);

                var message = @"{ Property: 'Value' }";

                var headers = new Dictionary<string, string>
                {
                    {"NServiceBus.EnclosedMessageTypes", typeof(MessageToSend).FullName},
                    {"NServiceBus.MessageId", Guid.NewGuid().ToString()}
                };

                var scriptPath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"NativeSend\NativeSend.ps1");
                using (var powerShell = PowerShell.Create())
                {
                    powerShell.AddScript(File.ReadAllText(scriptPath));
                    powerShell.Invoke();
                    var command = powerShell.AddCommand("SendMessage");
                    command.AddParameter("QueueName", endpointName);
                    command.AddParameter("MessageBody", message);
                    command.AddParameter("Headers", headers);
                    command.Invoke();
                }

                Assert.AreEqual("Value", await state.Signal.Task.ConfigureAwait(false));
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
        public async Task SendLarge()
        {
            var state = new State();
            IEndpointInstance endpoint = null;
            try
            {
                endpoint = await StartEndpoint(state).ConfigureAwait(false);

                var message = @"{ Property: 'Value' }";

                var headers = new Dictionary<string, string>
                {
                    {"NServiceBus.EnclosedMessageTypes", typeof(MessageToSend).FullName},
                    {"NServiceBus.MessageId", Guid.NewGuid().ToString()}
                };

                using(var s3Client = ClientFactory.CreateS3Client())
                using (var client = ClientFactory.CreateSqsClient())
                {
                    await NativeSend.SendLargeMessage(client, s3Client, endpointName, "test", SqsTransportConfigurationExtensions.S3BucketName, message, headers)
                        .ConfigureAwait(false);
                }

                Assert.AreEqual("Value", await state.Signal.Task.ConfigureAwait(false));
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
        public async Task SendLargePowerShell()
        {
            var state = new State();
            IEndpointInstance endpoint = null;
            try
            {
                endpoint = await StartEndpoint(state).ConfigureAwait(false);

                var message = @"{ Property: 'Value' }";

                var headers = new Dictionary<string, string>
                {
                    {"NServiceBus.EnclosedMessageTypes", typeof(MessageToSend).FullName},
                    {"NServiceBus.MessageId", Guid.NewGuid().ToString()}
                };

                var scriptPath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"NativeSend\NativeSend.ps1");
                using (var powerShell = PowerShell.Create())
                {
                    powerShell.AddScript(File.ReadAllText(scriptPath));
                    powerShell.Invoke();
                    var command = powerShell.AddCommand("SendLargeMessage");
                    command.AddParameter("QueueName", endpointName);
                    command.AddParameter("S3Prefix", "test");
                    command.AddParameter("BucketName", SqsTransportConfigurationExtensions.S3BucketName);
                    command.AddParameter("MessageBody", message);
                    command.AddParameter("Headers", headers);
                    command.Invoke();
                }

                Assert.AreEqual("Value", await state.Signal.Task.ConfigureAwait(false));
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
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UseSerialization<JsonSerializer>();
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
                Signal = new TaskCompletionSource<string>();
                // make sure the test does not hang forever
                var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(60));
                tokenSource.Token.Register(() => Signal.TrySetResult(null));
            }

            public TaskCompletionSource<string> Signal;
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
                state.Signal.TrySetResult(message.Property);
                return Task.CompletedTask;
            }
        }

        class MessageToSend :
            IMessage
        {
            public string Property { get; set; }
        }

    }
}