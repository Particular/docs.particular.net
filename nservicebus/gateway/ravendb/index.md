---
title: RavenDB Gateway Storage
summary: RavenDB deduplication storage for the Gateway component
component: GatewayRavenDB
reviewed: 2020-02-06
related:
 - samples/gateway
---

Provides deduplication storage for the [Gateway component](/nservicebus/gateway/) in RavenDB.

## Usage

snippet: DefaultUsage

## Cleaning up old records

After some period when duplicates are no longer likely, deduplication data should be cleaned up. RavenDB Gateway Storage provides a built-in mechanism based on the RavenDB Expiration feature which is activated by default on the database server.

Deduplication data is by default kept for 7 days, and the cleanup job is executed every 10 minutes. To customize the expiration policy use `DeduplicationDataTimeToLive` and `FrequencyToRunDeduplicationDataCleanup`, as shown below:

snippet: CustomExpiration

In the above example, deduplication data are preserved for 15 days and cleanup is run every 24 hours.

NOTE: While it is possible to use the same deduplication database for all endpoints within a single [logical site](/nservicebus/gateway/#logically-different-sites), the Gateway assumes that _different_ logical sites (which are generally physically separated as well) will use separate storage infrastructure. Sending a message to multiple sites will result in messages with the same message-id delivered to each site, if those sites for some reason share a single deduplication table, the deduplication will not work correctly unless. In that case, separate the storage by using different database names.
