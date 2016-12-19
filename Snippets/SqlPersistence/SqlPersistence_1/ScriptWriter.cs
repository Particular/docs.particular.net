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

public class ScriptWriter
{
    [Test]
    public void Write()
    {
        var directory = Path.Combine(TestContext.CurrentContext.TestDirectory, "../../");
        foreach (var varient in Enum.GetValues(typeof(BuildSqlVarient)).Cast<BuildSqlVarient>())
        {
            Write(directory, varient, "TimeoutCreate", TimeoutScriptBuilder.BuildCreateScript(varient));
            Write(directory, varient, "TimeoutDrop", TimeoutScriptBuilder.BuildDropScript(varient));

            Write(directory, varient, "OutboxCreate", OutboxScriptBuilder.BuildCreateScript(varient));
            Write(directory, varient, "OutboxDrop", OutboxScriptBuilder.BuildDropScript(varient));

            Write(directory, varient, "SubscriptionCreate", SubscriptionScriptBuilder.BuildCreateScript(varient));
            Write(directory, varient, "SubscriptionDrop", SubscriptionScriptBuilder.BuildDropScript(varient));

            var sagaDefinition = new SagaDefinition(
                tableSuffix: "OrderSaga",
                name: "OrderSaga",
                correlationProperty: new CorrelationProperty(
                    name: "OrderNumber",
                    type: CorrelationPropertyType.Int),
                transitionalCorrelationProperty: new CorrelationProperty(
                    name: "OrderId",
                    type: CorrelationPropertyType.Guid));
            Write(directory, varient, "SagaCreate", SagaScriptBuilder.BuildCreateScript(sagaDefinition, varient));
            Write(directory, varient, "SagaDrop", SagaScriptBuilder.BuildDropScript(sagaDefinition, varient));
        }
        foreach (var varient in Enum.GetValues(typeof(SqlVarient)).Cast<SqlVarient>())
        {
            var timeoutCommands = TimeoutCommandBuilder.Build(sqlVarient: varient, tablePrefix: "EndpointName");
            Write(directory, varient, "TimeoutAdd", timeoutCommands.Add);
            Write(directory, varient, "TimeoutNext", timeoutCommands.Next);
            Write(directory, varient, "TimeoutRange", timeoutCommands.Range);
            Write(directory, varient, "TimeoutRemoveById", timeoutCommands.RemoveById);
            Write(directory, varient, "TimeoutRemoveBySagaId", timeoutCommands.RemoveBySagaId);
            Write(directory, varient, "TimeoutPeek", timeoutCommands.Peek);

            var outboxCommands = OutboxCommandBuilder.Build(varient, "EndpointName");
            Write(directory, varient, "OutboxCleanup", outboxCommands.Cleanup);
            Write(directory, varient, "OutboxGet", outboxCommands.Get);
            Write(directory, varient, "OutboxSetAsDispatched", outboxCommands.SetAsDispatched);
            Write(directory, varient, "OutboxStore", outboxCommands.Store);

            var subscriptionCommands = SubscriptionCommandBuilder.Build(varient, "EndpointName");
            Write(directory, varient, "SubscriptionSubscribe", subscriptionCommands.Subscribe);
            Write(directory, varient, "SubscriptionUnsubscribe", subscriptionCommands.Unsubscribe);
            Write(directory, varient, "SubscriptionGetSubscribers", subscriptionCommands.GetSubscribers(new List<MessageType>
            {
                new MessageType("MessageTypeName", new Version())
            }));

            var sagaCommandBuilder = new SagaCommandBuilder(varient, "EndpointName");
            Write(directory, varient, "SagaComplete", sagaCommandBuilder.BuildCompleteCommand("SagaName"));
            Write(directory, varient, "SagadGetByProperty", sagaCommandBuilder.BuildGetByPropertyCommand("SagaName", "PropertyName"));
            Write(directory, varient, "SagaGetBySagaId", sagaCommandBuilder.BuildGetBySagaIdCommand("SagaName"));
            Write(directory, varient, "SagaSave", sagaCommandBuilder.BuildSaveCommand("SagaName", "CorrelationPproperty", "TransitionalCorrelationPproperty"));
            Write(directory, varient, "SagaUpdate", sagaCommandBuilder.BuildUpdateCommand("SagaName", "TransitionalCorrelationPproperty"));
        }
    }

    #region CreationScriptSaga

    [SqlSaga(
        correlationProperty: "OrderNumber",
        transitionalCorrelationProperty: "OrderId")]
    public class OrderSaga :
        Saga<OrderSaga.OrderSagaData>
    {
        public class OrderSagaData :
            ContainSagaData
        {
            public int OrderNumber { get; set; }
            public Guid OrderId { get; set; }
        }

        #endregion

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
        {
        }
    }

    static void Write(string testDirectory, BuildSqlVarient varient, string suffix, string script)
    {
        Write(testDirectory, suffix, script, varient.ToString());
    }

    static void Write(string testDirectory, SqlVarient varient, string suffix, string script)
    {
        Write(testDirectory, suffix, script, varient.ToString());
    }

    static void Write(string testDirectory, string suffix, string script, string varientAsString)
    {
        var path = Path.Combine(testDirectory, $"{varientAsString}_{suffix}.sql");
        File.Delete(path);
        using (var writer = File.CreateText(path))
        {
            Trace.WriteLine($"{varientAsString}_{suffix}Sql");
            writer.WriteLine($@"startcode	{varientAsString}_{suffix}Sql".Replace("\t", " "));
            writer.WriteLine(script);
            writer.WriteLine("endcode");
        }
    }
}