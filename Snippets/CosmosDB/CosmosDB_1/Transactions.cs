using System.Collections.Generic;
using Microsoft.Azure.Cosmos;
using NServiceBus;
using NServiceBus.Persistence.CosmosDB;

class Transactions
{
    void ExtractPartitionKeyFromHeaderSimple(PersistenceExtensions<CosmosPersistence> persistence)
    {
        #region ExtractPartitionKeyFromHeaderSimple

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractPartitionKeyFromHeader("PartitionKeyHeader");

        #endregion
    }

    void ExtractPartitionKeyFromHeadersExtractor(PersistenceExtensions<CosmosPersistence> persistence, MyAppsCustomConfigurationHolder appConfig)
    {
        #region ExtractPartitionKeyFromHeadersExtractor

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractPartitionKeyFromHeaders<MyAppsCustomConfigurationHolder>((headers, config) => new PartitionKey(headers["PartitionKeyHeader"]), appConfig);

        #endregion
    }

    void ExtractPartitionKeyFromHeadersCustom(PersistenceExtensions<CosmosPersistence> persistence)
    {
        #region ExtractPartitionKeyFromHeadersCustom

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractPartitionKeyFromHeaders(new CustomPartitionKeyFromHeadersExtractor());

        #endregion
    }

    void ExtractContainerInfoFromHeader(PersistenceExtensions<CosmosPersistence> persistence)
    {
        #region ExtractContainerInfoFromHeader

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractContainerInformationFromHeader("ContainerKey", headerValue => new ContainerInformation(headerValue, new PartitionKeyPath("/partitionKey")));

        #endregion
    }

    void ExtractContainerInfoFromHeaders(PersistenceExtensions<CosmosPersistence> persistence, MyAppsCustomConfigurationHolder appConfig)
    {
        #region ExtractContainerInfoFromHeaders

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractContainerInformationFromHeaders<MyAppsCustomConfigurationHolder>((headers, config) => new ContainerInformation(headers["ContainerNameHeader"], new PartitionKeyPath(appConfig.PartitionKeyPath)), appConfig);

        #endregion
    }

    void ExtractContainerInfoFromHeadersCustom(PersistenceExtensions<CosmosPersistence> persistence)
    {
        #region ExtractContainerInfoFromHeadersCustom

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractContainerInformationFromHeaders(new CustomContainerInformationFromHeadersExtractor());

        #endregion
    }

    void ExtractPartitionKeyFromMessageExtractor(PersistenceExtensions<CosmosPersistence> persistence)
    {
        #region ExtractPartitionKeyFromMessageExtractor

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractPartitionKeyFromMessage<MyMessage>(message => new PartitionKey(message.ItemId));

        #endregion
    }

    void ExtractPartitionKeyFromMessageCustom(PersistenceExtensions<CosmosPersistence> persistence)
    {
        #region ExtractPartitionKeyFromMessageCustom

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractPartitionKeyFromMessage<MyMessage>(message => new PartitionKey(message.ItemId));

        #endregion
    }


    void ExtractContainerInfoFromMessageExtractor(PersistenceExtensions<CosmosPersistence> persistence)
    {
        #region ExtractContainerInfoFromMessageExtractor

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractContainerInformationFromMessage<MyMessage>(message => new ContainerInformation(message.ItemId.ToString(), new PartitionKeyPath("/partitionKey")));

        #endregion
    }

    void ExtractContainerInfoFromMessageCustom(PersistenceExtensions<CosmosPersistence> persistence)
    {
        #region ExtractContainerInfoFromMessageCustom

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractContainerInformationFromMessage(new CustomContainerInformationFromMessagesExtractor());

        #endregion
    }

}

class MyAppsCustomConfigurationHolder
{
    public string PartitionKeyPath { get; set; }
}

#region CustomPartitionKeyFromHeadersExtractor
public class CustomPartitionKeyFromHeadersExtractor : IPartitionKeyFromHeadersExtractor
{
    public bool TryExtract(IReadOnlyDictionary<string, string> headers, out PartitionKey? partitionKey)
    {
        if (headers.TryGetValue("PartitionKeyHeader", out var headerVal))
        {
            partitionKey = new PartitionKey(headerVal);
            return true;
        }

        partitionKey = null;
        return false;
    }
}
#endregion

#region CustomContainerInformationFromHeadersExtractor
public class CustomContainerInformationFromHeadersExtractor : IContainerInformationFromHeadersExtractor
{
    public bool TryExtract(IReadOnlyDictionary<string, string> headers, out ContainerInformation? containerInformation)
    {
        if (headers.TryGetValue("ContainerInformationHeader", out var headerVal))
        {
            containerInformation = new ContainerInformation(headerVal, new PartitionKeyPath("/partitionKey"));
            return true;
        }

        containerInformation = null;
        return false;
    }
}
#endregion

#region CustomPartitionKeyFromMessageExtractor
class CustomPartitionKeyFromMessageExtractor : IPartitionKeyFromMessageExtractor
{
    public bool TryExtract(object message, IReadOnlyDictionary<string, string> headers, out PartitionKey? partitionKey)
    {
        if (message is MyMessage myMessage)
        {
            partitionKey = new PartitionKey(myMessage.ItemId);
            return true;
        }

        partitionKey = null;
        return false;
    }
}
#endregion

#region CustomContainerInformationFromMessageExtractor
class CustomContainerInformationFromMessagesExtractor : IContainerInformationFromMessagesExtractor
{
    public bool TryExtract(object message, IReadOnlyDictionary<string, string> headers, out ContainerInformation? containerInformation)
    {
        if (message is MyMessage myMessage)
        {
            containerInformation = new ContainerInformation("ContainerNameForMyMessage", new PartitionKeyPath("/partitionKeyPath"));
            return true;
        }

        containerInformation = null;
        return false;
    }
}
#endregion