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

        [Test]
        public async Task Send()
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"send-{randomName}";
            var errorQueueName = $"send-{randomName}-error";

            var state = new State();
            IEndpointInstance endpoint = null;
            try
            {
                endpoint = await StartEndpoint(state, endpointName, errorQueueName).ConfigureAwait(false);

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

                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, includeRetries: true)
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(errorQueueName)
                    .ConfigureAwait(false);
            }
        }

        [Test]
        public async Task SendPowerShell()
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"sendpowershell-{randomName}";
            var errorQueueName = $"sendpowershell-{randomName}-error";

            var state = new State();
            IEndpointInstance endpoint = null;
            try
            {
                endpoint = await StartEndpoint(state, endpointName, errorQueueName).ConfigureAwait(false);

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

                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, includeRetries: true)
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(errorQueueName)
                    .ConfigureAwait(false);
            }
        }

        [Test]
        public async Task SendLarge()
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"sendlarge-{randomName}";
            var errorQueueName = $"sendlarge-{randomName}-error";
            
            var state = new State();
            IEndpointInstance endpoint = null;
            try
            {
                endpoint = await StartEndpoint(state, endpointName, errorQueueName).ConfigureAwait(false);

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

                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, includeRetries: true)
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(errorQueueName)
                    .ConfigureAwait(false);
            }
        }

        [Test]
        public async Task SendLargePowerShell()
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"sendlargepowershell-{randomName}";
            var errorQueueName = $"sendlargepowershell-{randomName}-error";

            var state = new State();
            IEndpointInstance endpoint = null;
            try
            {
                endpoint = await StartEndpoint(state, endpointName, errorQueueName).ConfigureAwait(false);

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

                await DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName, includeRetries: true)
                    .ConfigureAwait(false);
                await QueueDeletionUtils.DeleteQueue(errorQueueName)
                    .ConfigureAwait(false);
            }
        }

        Task<IEndpointInstance> StartEndpoint(State state, string endpointName, string errorQueueName)
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            endpointConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent(x => state, DependencyLifecycle.SingleInstance);
                });
            endpointConfiguration.SendFailedMessagesTo(errorQueueName);
            endpointConfiguration.EnableInstallers();
            var transport = endpointConfiguration.UseTransport<SqsTransport>();
            transport.ConfigureSqsTransport();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            var recoverabilitySettings = endpointConfiguration.Recoverability();
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