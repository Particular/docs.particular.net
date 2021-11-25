---
title: NServiceBus Gateway RavenDB Persistence Upgrade from 1 to 2
summary: Migration instructions on how to upgrade NServiceBus.Gateway.RavenDB 1 to 2
component: GatewayRavenDB
related:
- nservicebus/gateway
- nservicebus/gateway/ravendb
reviewed: 2021-11-25
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

## Cluster-wide transactions support

Starting with NServiceBus.Gateway.RavenDB version 2, the gateway persistence supports RavenDB cluster-wide transactions. RavenDB 5.2 or greater is required for both the RavenDB client and server. Cluster-wide transactions support is disabled by default. To enable it, configure the persister like follows:

```
var ravenGatewayDeduplicationConfiguration = new RavenGatewayDeduplicationConfiguration((builder, _) => 
{
   //create and configure the RavenDB document store.
})
{
   EnableClusterWideTransactions = true
};
```

If the persister is configured to connect to a database, whose replication factor is greater than one, and cluster-wide transaction are not enabled, the persister will thow the following exception:

> The configured database is replicated across multiple nodes, in order to continue, use EnableClusterWideTransactions on the gateway configuration.
