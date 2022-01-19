---
title: Cosmos DB Persistence Upgrade from 1.0 to 1.1
summary: Instructions on how to upgrade NServiceBus.Persistence.CosmosDB 1.0 to 1.1
component: CosmosDB
reviewed: 2022-01-18
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

## EnableMigrationMode has moved

The previous location of the `EnableMigrationMode` has been deprecated and now the `persistenceConfiguration.Sagas().EnableMigrationMode()` should be used instead.

## Changes to transactions

### Move to the new transactions API

A [new Transactions API](persistence/cosmosdb/transactions.md) has been introduced to determine the `PartitionKey` and `ContainerInformation`. It is recommended to move from using a behavior-based approach to using this new API instead.

The most simple way to extract the `PartitionKey` from the headers is by using the following method:

snippet: ExtractPartitionKeyFromHeaderSimple

The most simple way to extract the `PartitionKey` from the message is by using the following method:

snippet: ExtractPartitionKeyFromMessageExtractor

For more ways to set the `PartitionKey` see the [Transactions API documentation](persistence/cosmosdb/transactions.md).

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
