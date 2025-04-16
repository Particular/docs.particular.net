---
title: RavenDB Gateway Storage
summary: RavenDB deduplication storage for the gateway component
component: GatewayRavenDB
reviewed: 2025-02-07
related:
 - samples/gateway
---

Provides deduplication storage for the [gateway component](/nservicebus/gateway/) in RavenDB.

## Usage

snippet: DefaultUsage

## Cleaning up old records

After a certain amount of time, duplicates are no longer likely and deduplication data should be cleaned up. The RavenDB gateway storage component provides a built-in mechanism based on the RavenDB Expiration feature which is activated by default on the database server.

Deduplication data is kept for 7 days by default, and the cleanup job is executed every 10 minutes. To customize the expiration policy use `DeduplicationDataTimeToLive` and `FrequencyToRunDeduplicationDataCleanup`, as shown below:

snippet: CustomExpiration

In this example, deduplication data are preserved for 15 days and cleanup is run every 24 hours.

> [!NOTE]
> While it is possible to use the same deduplication database for all endpoints within a single [logical site](/nservicebus/gateway/#logically-different-sites), the gateway assumes that _different_ logical sites (which are generally physically separated as well) will use separate storage infrastructure. Sending a message to multiple sites will result in messages with the same message ID delivered to each site, if those sites share a single deduplication table, the deduplication will not work correctly. In that case, separate the storage by using different database names.
