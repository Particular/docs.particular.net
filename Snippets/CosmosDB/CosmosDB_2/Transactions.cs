using System;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Persistence.CosmosDB;

class Transactions
{
    void EndpointLevelHeaderExtractionRulesHeaderKey(PersistenceExtensions<CosmosPersistence> persistence)
    {
        #region EndpointLevelHeaderExtractionRulesHeaderKey

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractFromHeader("PartitionKeyHeader");

        #endregion
    }
    void EndpointLevelHeaderExtractionRulesHeaderKeyContainerInfo(PersistenceExtensions<CosmosPersistence> persistence)
    {
        #region EndpointLevelHeaderExtractionRulesHeaderKeyContainerInfo

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractFromHeader("PartitionKeyHeader", new ContainerInformation("ContainerName", new PartitionKeyPath("PartitionKeyPath")));

        #endregion
    }
    void EndpointLevelHeaderExtractionRulesHeaderKeyConverterContainerInfo(PersistenceExtensions<CosmosPersistence> persistence)
    {
        #region EndpointLevelHeaderExtractionRulesHeaderKeyConverterContainerInfo

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractFromHeader("PartitionKeyHeader", (value, toBeRemoved) => value.Replace(toBeRemoved, string.Empty), "__TOBEREMOVED__", new ContainerInformation("ContainerName", new PartitionKeyPath("PartitionKeyPath")));

        #endregion
    }
    void ManualRegistrationOfHeaderExtractors(PersistenceExtensions<CosmosPersistence> persistence)
    {
        #region ManualRegistrationOfHeaderExtractors

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractFromHeaders(new CustomHeaderExtractor());

        #endregion
    }
    void ContainerRegistrationOfHeaderExtractors(EndpointConfiguration config)
    {
        #region ContainerRegistrationOfHeaderExtractors

        config.RegisterComponents(c =>
            c.AddSingleton<ITransactionInformationFromHeadersExtractor>(b => new CustomHeaderExtractor()));

        #endregion
    }


    void EndpointLevelMessageExtractionRulesHeaderKeySaga(PersistenceExtensions<CosmosPersistence> persistence, Context scenarioContext)
    {
        #region EndpointLevelMessageExtractionRulesHeaderKeySaga

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractFromMessage<StartSaga>(startSaga =>
        {
            return new PartitionKey(startSaga.PartitionKey.ToString());
        }, new ContainerInformation("ContainerName", new PartitionKeyPath("PartitionKeyPath")));

        #endregion
    }
    void EndpointLevelMessageExtractionRulesHeaderKeySagaContext(PersistenceExtensions<CosmosPersistence> persistence, Context scenarioContext)
    {
        #region EndpointLevelMessageExtractionRulesHeaderKeySagaContext

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractFromMessage<StartSaga, Context>((startSaga, state) =>
        {
            state.StateMatched = startSaga.PartitionKey.Equals(state.SagaReceivedMessageId);
            return new PartitionKey(startSaga.PartitionKey.ToString());
        }, scenarioContext, new ContainerInformation("ContainerName", new PartitionKeyPath("PartitionKeyPath")));

        #endregion
    }
    void ManualRegistrationOfMessageExtractors(PersistenceExtensions<CosmosPersistence> persistence)
    {
        #region ManualRegistrationOfMessageExtractors

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractFromMessages(new CustomMessageExtractor());

        #endregion
    }
    void ContainerRegistrationOfMessageExtractors(EndpointConfiguration config)
    {
        #region ContainerRegistrationOfMessageExtractors

        config.RegisterComponents(c =>
            c.AddSingleton<ITransactionInformationFromMessagesExtractor>(b => new CustomMessageExtractor()));

        #endregion
    }

}

#region CustomHeaderExtractor
public class CustomHeaderExtractor : ITransactionInformationFromHeadersExtractor
{
    public bool TryExtract(IReadOnlyDictionary<string, string> headers, out PartitionKey? partitionKey, out ContainerInformation? containerInformation)
    {
        partitionKey = new PartitionKey(Guid.NewGuid().ToString());
        containerInformation = new ContainerInformation("ContainerName", new PartitionKeyPath("PartitionKeyPath"));
        return true;
    }
}
#endregion

#region CustomMessageExtractor
public class CustomMessageExtractor : ITransactionInformationFromMessagesExtractor
{
    public bool TryExtract(object message, out PartitionKey? partitionKey,
        out ContainerInformation? containerInformation)
    {
        partitionKey = new PartitionKey(Guid.NewGuid().ToString());
        containerInformation = new ContainerInformation("ContainerName", new PartitionKeyPath("PartitionKeyPath"));
        return true;
    }
}
#endregion

public class StartSaga : ICommand
{
    public Guid DataId { get; set; }
    public Guid PartitionKey { get; set; }
}

public class Context 
{
    public Guid SagaReceivedMessageId { get; set; }
    public bool SagaReceivedMessage { get; set; }
    public bool StateMatched { get; set; }
}