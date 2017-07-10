namespace CoreAll.Msmq.ErrorQueue
{
    using System;
    using System.IO;
    using System.Management.Automation;
    using System.Messaging;
    using System.Threading;
    using Common;
    using CoreAll.Msmq.QueueDeletion;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;
    using NServiceBus.Features;
    using NServiceBus.Logging;
    using NUnit.Framework;

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
            DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName);
            QueueDeletionUtils.DeleteQueue(errorQueueName);
        }

        [Test]
        public void ReturnMessageToSourceQueue()
        {
            var state = new State();
            using (var bus = StartBus(state))
            {
                var messageToSend = new MessageToSend();
                bus.SendLocal(messageToSend);
                var msmqMessageId = GetMsmqMessageId();

                state.ShouldHandlerThrow = false;

                ErrorQueue.ReturnMessageToSourceQueue(
                    errorQueueMachine: Environment.MachineName,
                    errorQueueName: errorQueueName,
                    msmqMessageId: msmqMessageId);

                state.ResetEvent.WaitOne();
            }
        }

        [Test]
        public void ReturnMessageToSourceQueuePS()
        {
            var state = new State();
            using (var bus = StartBus(state))
            {
                var messageToSend = new MessageToSend();
                bus.SendLocal(messageToSend);
                var msmqMessageId = GetMsmqMessageId();
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
                state.ResetEvent.WaitOne();
            }
        }

        IBus StartBus(State state)
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent(x => state, DependencyLifecycle.SingleInstance);
                });
            busConfiguration.EndpointName(endpointName);
            busConfiguration.TypesToScan(TypeScanner.NestedTypes<Tests>());
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.DisableFeature<SecondLevelRetries>();
            return Bus.Create(busConfiguration).Start();
        }

        class State
        {
            public ManualResetEvent ResetEvent = new ManualResetEvent(false);
            public bool ShouldHandlerThrow = true;
        }

        string GetMsmqMessageId()
        {
            var path = $@".\private$\{errorQueueName}";
            using (var errorQueue = new MessageQueue(path))
            {
                return errorQueue.Peek().Id;
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

            public void Handle(MessageToSend message)
            {
                if (state.ShouldHandlerThrow)
                {
                    throw new Exception("The exception message from the handler.");
                }
                state.ResetEvent.Set();
            }
        }

        class ConfigTransport :
            IProvideConfiguration<TransportConfig>
        {
            public TransportConfig GetConfiguration()
            {
                return new TransportConfig
                {
                    MaxRetries = 0
                };
            }
        }

        class ConfigErrorQueue :
            IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
        {
            public MessageForwardingInCaseOfFaultConfig GetConfiguration()
            {
                return new MessageForwardingInCaseOfFaultConfig
                {
                    ErrorQueue = errorQueueName
                };
            }
        }

        class MessageToSend :
            IMessage
        {
        }

    }
}