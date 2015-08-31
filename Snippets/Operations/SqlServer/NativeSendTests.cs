namespace Operations.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Data.SqlClient;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;
    using NServiceBus.Features;
    using NUnit.Framework;

    [TestFixture]
    [Explicit]
    public class NativeSendTests
    {
        string endpointName = "sqlserverNativeSendTests";
        static string errorQueueName = "sqlserverNativeSendTestsError";
        static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=samples;Integrated Security=True";
        static string schema = @"dbo";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                QueueDeletion.DeleteQueuesForEndpoint(connection, schema, endpointName);
                QueueDeletion.DeleteQueuesForEndpoint(connection, schema, errorQueueName);
            }
        }

        [Test]
        public void Send()
        {
            State state = new State();
            using (IBus bus = StartBus(state))
            {
                string message = @"{ Property: 'Value' }";

                var headers = new Dictionary<string, string>
                {
                    {"NServiceBus.EnclosedMessageTypes", "Operations.SqlServer.NativeSendTests+MessageToSend"}
                };

                NativeSend.SendMessage(connectionString, endpointName, message, headers);
                state.ResetEvent.WaitOne();
            }
        }

        IBus StartBus(State state)
        {
            BusConfiguration config = new BusConfiguration();
            config.RegisterComponents(c => c.ConfigureComponent(x => state, DependencyLifecycle.SingleInstance));
            config.EndpointName(endpointName);
            config.UseSerialization<JsonSerializer>();
            config.UseTransport<SqlServerTransport>().ConnectionString(connectionString);
            Type[] sqlTypes = typeof(SqlServerTransport).Assembly.GetTypes();
            config.TypesToScan(TypeScanner.NestedTypes<NativeSendTests>(sqlTypes));
            config.EnableInstallers();
            config.UsePersistence<InMemoryPersistence>();
            config.DisableFeature<SecondLevelRetries>();

            return Bus.Create(config).Start();
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
                Assert.AreEqual("Value", message.Property);
                state.ResetEvent.Set();
            }
        }

        class State
        {
            public ManualResetEvent ResetEvent = new ManualResetEvent(false);
        }

        class MessageToSend : IMessage
        {
            public string Property { get; set; }
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
    }
}