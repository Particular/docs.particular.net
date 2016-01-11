namespace Operations.Msmq
{
    using System;
    using System.IO;
    using System.Management.Automation;
    using System.Messaging;
    using System.Threading;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;
    using NServiceBus.Features;
    using NServiceBus.Logging;
    using NUnit.Framework;

    [TestFixture]
    [Explicit]
    public class ErrorQueueTests
    {
        static ErrorQueueTests()
        {
            LogManager.Use<DefaultFactory>().Level(LogLevel.Error);
        }
        string endpointName = "ReturnToSourceQueueTests";
        static string errorQueueName = "ReturnToSourceQueueTestsError";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletion.DeleteQueuesForEndpoint(endpointName);
            QueueDeletion.DeleteQueue(errorQueueName);
        }

        [Test]
        public void ReturnMessageToSourceQueue()
        {
            State state = new State();
            using (IBus bus = StartBus(state))
            {
                bus.SendLocal(new MessageToSend());
                string msmqMessageId = GetMsmqMessageId();

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
            State state = new State();
            using (IBus bus = StartBus(state))
            {
                bus.SendLocal(new MessageToSend());
                string msmqMessageId = GetMsmqMessageId();
                state.ShouldHandlerThrow = false;
                string currentDirectory = Path.GetDirectoryName(GetType().Assembly.CodeBase.Remove(0, 8));
                string scriptPath = Path.Combine(currentDirectory, "msmq/ErrorQueue.ps1");
                using (PowerShell powerShell = PowerShell.Create())
                {
                    powerShell.AddScript(File.ReadAllText(scriptPath));
                    powerShell.Invoke();
                    PowerShell command = powerShell.AddCommand("ReturnMessageToSourceQueue");
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
            BusConfiguration config = new BusConfiguration();
            config.RegisterComponents(c=>c.ConfigureComponent(x => state,DependencyLifecycle.SingleInstance));
            config.EndpointName(endpointName);
            config.TypesToScan(TypeScanner.NestedTypes<ErrorQueueTests>());
            config.EnableInstallers();
            config.UsePersistence<InMemoryPersistence>();
            config.DisableFeature<SecondLevelRetries>();
            return Bus.Create(config).Start();
        }

        class State
        {
            public ManualResetEvent ResetEvent = new ManualResetEvent(false);
            public bool ShouldHandlerThrow = true;
        }

        string GetMsmqMessageId()
        {
            string path = @".\private$\" + errorQueueName;
            using (MessageQueue errorQueue = new MessageQueue(path))
            {
                return errorQueue.Peek().Id;
            }
        }

        class MessageHandler : IHandleMessages<MessageToSend>
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

        class ConfigTransport : IProvideConfiguration<TransportConfig>
        {
            public TransportConfig GetConfiguration()
            {
                return new TransportConfig
                {
                    MaxRetries = 0
                };
            }
        }

        class ConfigErrorQueue : IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
        {
            public MessageForwardingInCaseOfFaultConfig GetConfiguration()
            {
                return new MessageForwardingInCaseOfFaultConfig
                {
                    ErrorQueue = errorQueueName
                };
            }
        }
        class MessageToSend : IMessage
        {
        }

    }
}