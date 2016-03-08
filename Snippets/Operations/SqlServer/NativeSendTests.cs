namespace Operations.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Data.SqlClient;
    using System.IO;
    using System.Management.Automation;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;
    using NServiceBus.Features;
    using NUnit.Framework;

    [TestFixture]
    [Explicit]
    public class NativeSendTests
    {
        string endpointName = "NativeSendTests";
        static string errorQueueName = "NativeSendTestsError";
        static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=samples;Integrated Security=True";
        static string schema = "dbo";

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

                Dictionary<string, string> headers = new Dictionary<string, string>
                {
                    {"NServiceBus.EnclosedMessageTypes", "Operations.SqlServer.NativeSendTests+MessageToSend"}
                };

                NativeSend.SendMessage(connectionString, endpointName, message, headers);
                state.ResetEvent.WaitOne();
            }
        }

        [Test]
        public void SendPowershell()
        {
            State state = new State();
            using (IBus bus = StartBus(state))
            {
                string message = @"{ Property: 'Value' }";

                Dictionary<string, string> headers = new Dictionary<string, string>
                {
                    {"NServiceBus.EnclosedMessageTypes", "Operations.SqlServer.NativeSendTests+MessageToSend"}
                };

                string scriptPath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"SqlServer\NativeSend.ps1");
                string script = File.ReadAllText(scriptPath);

                using (PowerShell powershell = PowerShell.Create())
                {
                    powershell.AddScript(script, false);
                    powershell.Invoke();
                    powershell.Commands.Clear();
                    powershell.AddCommand("SendMessage")
                        .AddParameter(null, connectionString)
                        .AddParameter(null, endpointName)
                        .AddParameter(null, message)
                        .AddParameter(null, headers);
                    Collection<PSObject> results = powershell.Invoke();
                }

                state.ResetEvent.WaitOne();
            }
        }

        IBus StartBus(State state)
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.RegisterComponents(c => c.ConfigureComponent(x => state, DependencyLifecycle.SingleInstance));
            busConfiguration.EndpointName(endpointName);
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.UseTransport<SqlServerTransport>()
                .ConnectionString(connectionString);
            Type[] sqlTypes = typeof(SqlServerTransport).Assembly.GetTypes();
            busConfiguration.TypesToScan(TypeScanner.NestedTypes<NativeSendTests>(sqlTypes));
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.DisableFeature<SecondLevelRetries>();

            return Bus.Create(busConfiguration).Start();
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