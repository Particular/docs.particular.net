namespace Operations.Msmq.Tests
{
    using System;
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
    public class ReturnToSourceQueueTests
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        string endpointName = "ReturnToSourceQueueTests";
        static string errorQueueName = "ReturnToSourceQueueTestsError";
        static bool shouldHandlerThrow = true;
        public static string MessageId;

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueCreation.DeleteQueuesForEndpoint(endpointName);
            QueueCreation.DeleteQueue(errorQueueName);
        }

        [Test]
        public void ReturnMessageToSourceQueue()
        {
            LogManager.Use<DefaultFactory>().Level(LogLevel.Error);
            using (IBus bus = Bus.Create(BuildConfig()).Start())
            {
                bus.SendLocal(new MessageToSend());
                Thread.Sleep(2000);
            }

            string msmqMessageId = GetMsmqMessageId();
            ErrorQueue.ReturnMessageToSourceQueue(
                machineName: Environment.MachineName,
                queueName: errorQueueName,
                msmqMessageId: msmqMessageId);

            shouldHandlerThrow = false;
            using (IBus bus = Bus.Create(BuildConfig()).Start())
            {
                ManualResetEvent.WaitOne();
            }
        }

        string GetMsmqMessageId()
        {
            string path = Environment.MachineName + "\\private$\\" + errorQueueName;
            using (var errorQueue = new MessageQueue(path))
            {
                return errorQueue.Peek().Id;
            }
        }

        BusConfiguration BuildConfig()
        {
            BusConfiguration config = new BusConfiguration();
            config.EndpointName(endpointName);
            config.TypesToScan(TypeScanner.NestedTypes<ReturnToSourceQueueTests>());
            config.EnableInstallers();
            config.UsePersistence<InMemoryPersistence>();
            config.DisableFeature<SecondLevelRetries>();
            return config;
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

        class MessageHandler : IHandleMessages<MessageToSend>
        {
            public void Handle(MessageToSend message)
            {
                if (shouldHandlerThrow)
                {
                    throw new Exception("The exception message from the handler.");
                }
                ManualResetEvent.Set();
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

        class MessageToSend : IMessage
        {
        }

    }
}