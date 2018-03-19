namespace SqsAll.ErrorQueue
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Amazon.SQS.Model;
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
        public async Task ReturnMessageToSourceQueue()
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var endpointName = $"returnmessagetosourcequeue-{randomName}";
            var errorQueueName = $"returnmessagetosourcequeue-{randomName}-error";

            var state = new State();
            IEndpointInstance endpoint = null;
            try
            {
                endpoint = await StartEndpoint(state, endpointName, errorQueueName).ConfigureAwait(false);
                var messageToSend = new MessageToSend();
                await endpoint.SendLocal(messageToSend).ConfigureAwait(false);
                var messageId = await GetMessageId(errorQueueName).ConfigureAwait(false);

                state.ShouldHandlerThrow = false;

                await ErrorQueue.ReturnMessageToSourceQueue(
                        errorQueueName: errorQueueName,
                        messageId: messageId)
                    .ConfigureAwait(false);

                Assert.IsTrue(await state.Signal.Task.ConfigureAwait(false));
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
            public bool ShouldHandlerThrow = true;
        }

        static async Task<string> GetMessageId(string errorQueueName)
        {
            var path = QueueNameHelper.GetSqsQueueName($"{errorQueueName}");
            using (var client = ClientFactory.CreateSqsClient())
            {
                var getQueueUrlResponse = await client.GetQueueUrlAsync(path)
                    .ConfigureAwait(false);

                var messages = await client.ReceiveMessageAsync(
                        new ReceiveMessageRequest(getQueueUrlResponse.QueueUrl)
                        {
                            MaxNumberOfMessages = 1,
                            WaitTimeSeconds = 20,
                            VisibilityTimeout = 0, // message needs to be immediately visible again to be seen by the ReturnToSourceQueue algorithm
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