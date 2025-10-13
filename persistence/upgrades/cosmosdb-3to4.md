---
title: Cosmos DB Persistence Upgrade from 3 to 4
summary: Instructions on how to upgrade NServiceBus.Persistence.CosmosDB 3 to 4
component: CosmosDB
reviewed: 2025-10-13
isUpgradeGuide: true
---

## Changes to Container Information Overrides

Customers that have both a [default container](/persistence/cosmosdb/#usage-customizing-the-container-used), and a [message container extractor](/persistence/cosmosdb/transactions.md#specifying-the-container-to-use-for-the-transaction-using-the-message-contents) configured will be affected by this update.

**In version 3.1 and under**, when a default container and a message container extractor are both configured, the container information is not overwritten by the message container extractor and falls back to the configured default container. The expected outcome is that the container information sourced from the message extractor overrides the default container configured.

**In version 3.2.1**, an opt-in configuration API was introduced to allow for the expected behavior of the container information being sourced from the message extractor, rather than the default container. This was made opt-in as it could potentially result in a breaking change in some solutions.

**In version 4**, the opt-in configuration API functionality will be enabled by default, and the opt-in API, if used, will throw a build error.

### Solution

Affected customers should perform one of the below options prior to updating to version 4:

1. Customers can remove the configured message container extractor and only rely on the default container.
2. Customers can update their message container extractor to use the same container specified as the default container.
3. Customers can [migrate relevant records](https://learn.microsoft.com/en-us/azure/cosmos-db/container-copy?tabs=online-copy&pivots=api-nosql) from the default container to the container specified in the message container extractor. This option may require [changing the container partition key](https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/change-partition-key).
