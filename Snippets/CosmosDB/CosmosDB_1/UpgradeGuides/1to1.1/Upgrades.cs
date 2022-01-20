using Microsoft.Azure.Cosmos;
using NServiceBus;

class Upgrades
{
    void ExtractPartitionKeyFromHeaderSimple(PersistenceExtensions<CosmosPersistence> persistence)
    {
        #region ExtractPartitionKeyFromHeaderSimple1to11

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractPartitionKeyFromHeader("PartitionKeyHeader");

        #endregion
    }

    void ExtractPartitionKeyFromMessageExtractor(PersistenceExtensions<CosmosPersistence> persistence)
    {
        #region ExtractPartitionKeyFromMessageExtractor1to11

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractPartitionKeyFromMessage<MyMessage>(message => new PartitionKey(message.ItemId));

        #endregion
    }

    void ExtractContainerInfoFromHeaders(PersistenceExtensions<CosmosPersistence> persistence)
    {
        #region ExtractContainerInfoFromHeaders1to11

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractContainerInformationFromHeaders(headers => new ContainerInformation(headers["ContainerNameHeader"], new PartitionKeyPath("/partitionKeyPath")));

        #endregion
    }

    void ExtractContainerInfoFromMessageExtractor(PersistenceExtensions<CosmosPersistence> persistence)
    {
        #region ExtractContainerInfoFromMessageExtractor1to11

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractContainerInformationFromMessage<MyMessage>(message => new ContainerInformation(message.ItemId.ToString(), new PartitionKeyPath("/partitionKey")));

        #endregion
    }
}