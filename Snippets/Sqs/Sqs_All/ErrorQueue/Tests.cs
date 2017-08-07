namespace SqsAll.ErrorQueue
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using System.Threading.Tasks;
    using Amazon.SQS.Model;
    using NServiceBus;
    using NServiceBus.Logging;
    using NUnit.Framework;
    using SqsAll.QueueCreation;
    using SqsAll.QueueDeletion;
    using Sqs_All;

    [TestFixture]
    [Explicit]
    public class Tests
    {
        static Tests()
        {
            LogManager.Use<DefaultFactory>().Level(LogLevel.Error);
        }

        string endpointName = "ReturnToSourceQueueTests";
        static string errorQueueName = "ReturnToSourceQueueTestsError";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName).GetAwaiter().GetResult();
            QueueDeletionUtils.DeleteQueue(errorQueueName).GetAwaiter().GetResult();
        }

        [Test]
        public async Task ReturnMessageToSourceQueue()
        {
            var state = new State();
            IEndpointInstance endpoint = null;
            try
            {
                endpoint = await StartEndpoint(state).ConfigureAwait(false);
                var messageToSend = new MessageToSend();
                await endpoint.SendLocal(messageToSend).ConfigureAwait(false);
                var messageId = await GetMessageId().ConfigureAwait(false);

                state.ShouldHandlerThrow = false;

                ErrorQueue.ReturnMessageToSourceQueue(
                    errorQueueName: errorQueueName,
                    msmqMessageId: messageId);

                await state.Signal.Task.ConfigureAwait(false);
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
        public async Task ReturnMessageToSourceQueuePS()
        {
            var state = new State();
            IEndpointInstance endpoint = null;
            try
            {
                endpoint = await StartEndpoint(state).ConfigureAwait(false);
                var messageToSend = new MessageToSend();
                await endpoint.SendLocal(messageToSend).ConfigureAwait(false);
                var msmqMessageId = GetMessageId();
                state.ShouldHandlerThrow = false;

                var scriptPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "ErrorQueue/ErrorQueue.ps1");
                using (var powerShell = PowerShell.Create())
                {
                    powerShell.AddScript(File.ReadAllText(scriptPath));
                    powerShell.Invoke();
                    var command = powerShell.AddCommand("ReturnMessageToSourceQueue");
                    command.AddParameter("ErrorQueueMachine", Environment.MachineName);
                    command.AddParameter("ErrorQueueName", errorQueueName);
                    command.AddParameter("MessageId", msmqMessageId);
                    command.Invoke();
                }
                await state.Signal.Task.ConfigureAwait(false);
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
            var transport = endpointConfiguration.UseTransport<SqsTransport>();
            transport.ConfigureSqsTransport("returnsourcequeue");
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            var recoverabilitySettings = endpointConfiguration.Recoverability();
            recoverabilitySettings.DisableLegacyRetriesSatellite();
            recoverabilitySettings.Immediate(customizations => customizations.NumberOfRetries(0));
            recoverabilitySettings.Delayed(customizations => customizations.NumberOfRetries(0));

            return Endpoint.Start(endpointConfiguration);
        }

        class State
        {
            public TaskCompletionSource<bool> Signal = new TaskCompletionSource<bool>();
            public bool ShouldHandlerThrow = true;
        }

        static async Task<string> GetMessageId()
        {
            var path = QueueNameHelper.GetSqsQueueName($"{errorQueueName}");
            using (var client = ClientFactory.CreateSqsClient())
            {
                var messages = await client.ReceiveMessageAsync(
                        new ReceiveMessageRequest(client.GetQueueUrl(path).QueueUrl)
                        {
                            MaxNumberOfMessages = 1,
                            WaitTimeSeconds = 20
                        })
                    .ConfigureAwait(false);

                var message = messages.Messages.Single();
                return message.MessageId;
            }
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
                if (state.ShouldHandlerThrow)
                {
                    throw new Exception("The exception message from the handler.");
                }
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