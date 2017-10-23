using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        foreach (var variant in Enum.GetValues(typeof(BuildSqlVariant)).Cast<BuildSqlVariant>())
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
        foreach (var variant in Enum.GetValues(typeof(SqlVariant)).Cast<SqlVariant>())
        {
            var timeoutCommands = TimeoutCommandBuilder.Build(
                sqlVariant: variant,
                tablePrefix: "EndpointName");
            Write(directory, variant, "TimeoutAdd", timeoutCommands.Add);
            Write(directory, variant, "TimeoutNext", timeoutCommands.Next);
            Write(directory, variant, "TimeoutRange", timeoutCommands.Range);
            Write(directory, variant, "TimeoutRemoveById", timeoutCommands.RemoveById);
            Write(directory, variant, "TimeoutRemoveBySagaId", timeoutCommands.RemoveBySagaId);
            Write(directory, variant, "TimeoutPeek", timeoutCommands.Peek);

            var outboxCommands = OutboxCommandBuilder.Build(variant, "EndpointName");
            Write(directory, variant, "OutboxCleanup", outboxCommands.Cleanup);
            Write(directory, variant, "OutboxGet", outboxCommands.Get);
            Write(directory, variant, "OutboxSetAsDispatched", outboxCommands.SetAsDispatched);
            Write(directory, variant, "OutboxStore", outboxCommands.Store);

            var subscriptionCommands = SubscriptionCommandBuilder.Build(variant, "EndpointName");
            Write(directory, variant, "SubscriptionSubscribe", subscriptionCommands.Subscribe);
            Write(directory, variant, "SubscriptionUnsubscribe", subscriptionCommands.Unsubscribe);
            Write(directory, variant, "SubscriptionGetSubscribers", subscriptionCommands.GetSubscribers(new List<MessageType>
            {
                new MessageType("MessageTypeName", new Version())
            }));

            var sagaCommandBuilder = new SagaCommandBuilder(variant, "EndpointName");
            Write(directory, variant, "SagaComplete", sagaCommandBuilder.BuildCompleteCommand("SagaName"));
            Write(directory, variant, "SagadGetByProperty", sagaCommandBuilder.BuildGetByPropertyCommand("SagaName", "PropertyName"));
            Write(directory, variant, "SagaGetBySagaId", sagaCommandBuilder.BuildGetBySagaIdCommand("SagaName"));
            Write(directory, variant, "SagaSave", sagaCommandBuilder.BuildSaveCommand("SagaName", "CorrelationProperty", "TransitionalCorrelationProperty"));
            Write(directory, variant, "SagaUpdate", sagaCommandBuilder.BuildUpdateCommand("SagaName", "TransitionalCorrelationProperty"));
        }
    }

    #region CreationScriptSaga

    [SqlSaga(
        correlationProperty: "OrderNumber",
        transitionalCorrelationProperty: "OrderId")]
    public class OrderSaga :
        SqlSaga<OrderSaga.OrderSagaData>
    {
        public class OrderSagaData :
            ContainSagaData
        {
            public int OrderNumber { get; set; }
            public Guid OrderId { get; set; }
        }

        #endregion

        protected override void ConfigureMapping(MessagePropertyMapper<OrderSagaData> mapper)
        {
        }
    }

    static void Write(string testDirectory, BuildSqlVariant variant, string suffix, string script)
    {
        Write(testDirectory, suffix, script, variant.ToString());
    }

    static void Write(string testDirectory, SqlVariant variant, string suffix, string script)
    {
        Write(testDirectory, suffix, script, variant.ToString());
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
}