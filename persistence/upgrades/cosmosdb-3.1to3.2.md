---
title: Cosmos DB Persistence Upgrade from 3.1 to 3.2
summary: Instructions on how to upgrade NServiceBus.Persistence.CosmosDB 3.1 to 3.2
component: CosmosDB
reviewed: 2025-09-30
isUpgradeGuide: true
---

## Changes to Container Information Overrides

Customers that have both a [default container](/persistence/cosmosdb/#usage-customizing-the-container-used), and a [message container extractor](/persistence/cosmosdb/transactions.md#specifying-the-container-to-use-for-the-transaction-using-the-message-contents) configured will be affected by this update.

**In version 3.1 and under**, when a default container and a message container extractor are both configured, the container information is not overwritten by the message container extractor and falls back to the configured default container. The expected outcome is that the container information sourced from the message extractor overrides the default container configured.

**From version 3.2 and over**, the message container extractor correctly overrides the configured default container information.

### Impact

As a result of the above issue, customers may be unintentionally relying on the use of the default container over the configured message container extractor.

### Solution

Affected customers should perform one of the below options prior to updating to version 3.2:

1. Customers can remove the configured message container extractor and only rely on the default container.
2. Customers can update their message container extractor to use the same container specified as the default container.
3. Customers can [migrate relevant records](https://learn.microsoft.com/en-us/azure/cosmos-db/container-copy?tabs=online-copy&pivots=api-nosql) from the default container to the container specified in the message container extractor. This option may require [changing the container partition key](https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/change-partition-key).
