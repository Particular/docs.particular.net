---
title: Cosmos DB Persistence Upgrade from 1 to 2
summary: Instructions on how to upgrade NServiceBus.Persistence.CosmosDB 1 to 2
component: CosmosDB
reviewed: 2023-12-08
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

## EnableMigrationMode has moved

The previous location of the `EnableMigrationMode` has been deprecated and the `persistenceConfiguration.Sagas().EnableMigrationMode()` should be used instead.

## Changes to transactions

### Move to the new transactions API

A [new Transactions API](/persistence/cosmosdb/transactions.md) has been introduced to determine the `PartitionKey` and `ContainerInformation`. It is recommended to move from using a behavior-based approach to using this new API instead.

### Identifying the `PartitionKey`

The most simple way to extract the `PartitionKey` from the headers is by using the following method:

snippet: ExtractPartitionKeyFromHeaderSimple1to11

The most simple way to extract the `PartitionKey` from the message is by using the following method:

snippet: ExtractPartitionKeyFromMessageExtractor1to11

For more ways to set the `PartitionKey` see the [Transactions API documentation](/persistence/cosmosdb/transactions.md).

### Identifying the `ContainerInformation`

The most simple way to extract the `ContainerInformation` from the headers is by using the following method:

snippet: ExtractContainerInfoFromHeaders1to11

The most simple way to extract the `ContainerInformation` from the message is by using the following method:

snippet: ExtractContainerInfoFromMessageExtractor1to11

For more ways to set the `ContainerInformation` see the [Transactions API documentation](/persistence/cosmosdb/transactions.md).

### `LogicalOutboxBehavior` has been deprecated

The `LogicalOutboxBehavior` has been marked as deprecated and will be internal only starting with version 3.0.

To continue to use Behaviors to determine the PartitionKey or ContainerInformation registration should be updated to use the string literal of the behavior class name:

```csharp
public class RegisterMyBehavior : RegisterStep
{
    public RegisterMyBehavior() :
        base(stepId: nameof(PartitionKeyIncomingLogicalMessageContextBehavior),
        behavior: typeof(PartitionKeyIncomingLogicalMessageContextBehavior),
        description: "Determines the PartitionKey from the logical message",
        factoryMethod: b => new PartitionKeyIncomingLogicalMessageContextBehavior())
    {
-        InsertBeforeIfExists(nameof(LogicalOutboxBehavior));    
+        InsertBeforeIfExists("LogicalOutboxBehavior");
    }
}
```

NOTE: It is recommended to move from using a behavior-based approach to using the [new Transactions API](/persistence/cosmosdb/transactions.md) instead.
