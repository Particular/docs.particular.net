using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using NServiceBus.Persistence.Sql.ScriptBuilder;
using NServiceBus.Unicast.Subscriptions;
using NUnit.Framework;
#pragma warning disable 618

public class ScriptWriter
{
    [Test]
    public void Write()
    {
        var directory = Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../");
        foreach (var variant in Enum.GetValues(typeof(BuildSqlDialect)).Cast<BuildSqlDialect>())
        {
            Write(directory, variant, "TimeoutCreate", TimeoutScriptBuilder.BuildCreateScript(variant));
            Write(directory, variant, "TimeoutDrop", TimeoutScriptBuilder.BuildDropScript(variant));

            Write(directory, variant, "OutboxCreate", OutboxScriptBuilder.BuildCreateScript(variant));
            Write(directory, variant, "OutboxDrop", OutboxScriptBuilder.BuildDropScript(variant));

            Write(directory, variant, "SubscriptionCreate", SubscriptionScriptBuilder.BuildCreateScript(variant));
            Write(directory, variant, "SubscriptionDrop", SubscriptionScriptBuilder.BuildDropScript(variant));

            var sagaDefinition = new SagaDefinition(
                tableSuffix: "OrderSaga",
                name: "OrderSaga",
                correlationProperty: new CorrelationProperty(
                    name: "OrderNumber",
                    type: CorrelationPropertyType.Int),
                transitionalCorrelationProperty: new CorrelationProperty(
                    name: "OrderId",
                    type: CorrelationPropertyType.Guid));
            Write(directory, variant, "SagaCreate", SagaScriptBuilder.BuildCreateScript(sagaDefinition, variant));
            Write(directory, variant, "SagaDrop", SagaScriptBuilder.BuildDropScript(sagaDefinition, variant));
        }

        var dialects = new SqlDialect[]
        {
            new SqlDialect.MsSqlServer(),
            new SqlDialect.MySql(),
            new SqlDialect.Oracle(),
            new SqlDialect.PostgreSql()
        };

        var assembly = typeof(SqlPersistence).Assembly;
        foreach (var dialect in dialects)
        {
            var timeoutCommands = GetCommand(dialect, "TimeoutCommandBuilder");

            Write(directory, dialect, "TimeoutAdd", GetValue(timeoutCommands, "Add"));
            Write(directory, dialect, "TimeoutNext", GetValue(timeoutCommands, "Next"));
            Write(directory, dialect, "TimeoutRange", GetValue(timeoutCommands, "Range"));
            Write(directory, dialect, "TimeoutRemoveById", GetValue(timeoutCommands, "RemoveById"));
            Write(directory, dialect, "TimeoutRemoveBySagaId", GetValue(timeoutCommands, "RemoveBySagaId"));
            Write(directory, dialect, "TimeoutPeek", GetValue(timeoutCommands, "Peek"));

            var outboxCommands = GetCommand(dialect, "OutboxCommandBuilder");

            Write(directory, dialect, "OutboxCleanup", GetValue(outboxCommands, "Cleanup"));
            Write(directory, dialect, "OutboxGet", GetValue(outboxCommands, "Get"));
            Write(directory, dialect, "OutboxSetAsDispatched", GetValue(outboxCommands, "SetAsDispatched"));
            Write(directory, dialect, "OutboxOptimisticStore", GetValue(outboxCommands, "OptimisticStore"));
            Write(directory, dialect, "OutboxPessimisticBegin", GetValue(outboxCommands, "PessimisticBegin"));
            Write(directory, dialect, "OutboxPessimisticComplete", GetValue(outboxCommands, "PessimisticComplete"));

            var subscriptionCommands = GetCommand(dialect, "SubscriptionCommandBuilder");

            Write(directory, dialect, "SubscriptionSubscribe", GetValue(subscriptionCommands, "Subscribe"));
            Write(directory, dialect, "SubscriptionUnsubscribe", GetValue(subscriptionCommands, "Unsubscribe"));
            Write(directory, dialect, "SubscriptionGetSubscribers", GetSubscribersValue(subscriptionCommands, new List<MessageType>
            {
                new MessageType("MessageTypeName", new Version())
            }));

            dynamic dialectAsDynamic = new ExposeInternalMethods(dialect);
            Write(directory, dialect, "SagaComplete", dialectAsDynamic.BuildCompleteCommand("EndpointName_SagaName"));
            Write(directory, dialect, "SagaGetByProperty", dialectAsDynamic.BuildGetByPropertyCommand("PropertyName", "EndpointName_SagaName"));
            Write(directory, dialect, "SagaGetBySagaId", dialectAsDynamic.BuildGetBySagaIdCommand("EndpointName_SagaName"));
            Write(directory, dialect, "SagaSave", dialectAsDynamic.BuildSaveCommand("CorrelationProperty", "TransitionalCorrelationProperty", "EndpointName_SagaName"));
            Write(directory, dialect, "SagaUpdate", dialectAsDynamic.BuildUpdateCommand("TransitionalCorrelationProperty", "EndpointName_SagaName"));

            // since we don't have docs on oracle saga finders
            if (!(dialect is SqlDialect.Oracle))
            {
                var createSelectWithWhereClause = dialectAsDynamic.BuildSelectFromCommand("EndpointName_SagaName");
                Write(directory, dialect, "SagaSelect", createSelectWithWhereClause("1 = 1"));
            }
        }
    }

    #region CreationScriptSaga

    [SqlSaga(transitionalCorrelationProperty: nameof(OrderSagaData.OrderId))]
    public class OrderSaga : Saga<OrderSaga.OrderSagaData>,
        IAmStartedByMessages<StartSaga>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
        {
            mapper.ConfigureMapping<StartSaga>(msg => msg.OrderNumber).ToSaga(saga => saga.OrderNumber);
        }

        public class OrderSagaData :
            ContainSagaData
        {
            public int OrderNumber { get; set; }
            public Guid OrderId { get; set; }
        }

        #endregion

        public Task Handle(StartSaga message, IMessageHandlerContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class StartSaga : ICommand
    {
        public int OrderNumber { get; set; }
        public Guid OrderId { get; set; }
    }

    class ExposeInternalMethods : DynamicObject 
    {
        private object m_object;

        public ExposeInternalMethods(object obj)
        {
            m_object = obj;
        }

        public override bool TryInvokeMember(
                InvokeMemberBinder binder, object[] args, out object result)
        {
            // Find the called method using reflection
            var methodInfo = m_object.GetType().GetMethod(
                binder.Name,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            // Call the method
            result = methodInfo.Invoke(m_object, args);
            return true;
        }
    }

    static void Write(string testDirectory, BuildSqlDialect variant, string suffix, string script)
    {
        Write(testDirectory, suffix, script, variant.ToString());
    }

    static void Write(string testDirectory, SqlDialect dialect, string suffix, string script)
    {
        Write(testDirectory, suffix, script, dialect.ToString());
    }

    static void Write(string testDirectory, string suffix, string script, string variantAsString)
    {
        var path = Path.Combine(testDirectory, $"{variantAsString}_{suffix}.sql");
        File.Delete(path);
        using (var writer = File.CreateText(path))
        {
            Trace.WriteLine($"{variantAsString}_{suffix}Sql");
            writer.WriteLine($@"startcode	{variantAsString}_{suffix}Sql".Replace("\t", " "));
            writer.WriteLine(script);
            writer.WriteLine("endcode");
        }
    }

    static object GetCommand(SqlDialect dialect, string builderType) {
        return typeof(SqlPersistence).Assembly.GetType(builderType, throwOnError: true)
                .GetMethod("Build")
                ?.Invoke(null, BindingFlags.Static | BindingFlags.NonPublic, null, new object[] { dialect, "EndpointName" }, null);
    }

    static string GetValue(object command, string propertyName) {
        var property = command.GetType().GetProperty(propertyName);
        return (string) property?.GetValue(command);
    }

    static string GetSubscribersValue(object command, List<MessageType> messageTypes) {
        var property = command.GetType().GetProperty("GetSubscribers");
        var propertyValue = (Func<List<MessageType>, string>)property?.GetValue(command);
        return propertyValue?.Invoke(messageTypes);
    }
}