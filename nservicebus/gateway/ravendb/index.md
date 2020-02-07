---
title: RavenDB Gateway Storage
summary: RavenDB deduplication storage for the Gateway component
component: GatewayRavenDB
reviewed: 2020-02-06
related:
 - samples/gateway
---

RavenDB Gateway Storage provides deduplication storage for the [Gateway component](/nservicebus/gateway/) in RavenDB.

## Usage

snippet: DefaultUsage

## Cleaning up old records

After some period when duplicates are no longer likely, deduplication data should be cleaned up. RavenDB Gateway Storage provides a built-in mechanism to do this based on the RavenDB Expiration feature. By default the Expiration feature is activated on the configured database server.

Deduplication data are preserved by default for 7 days, and the clean up background job is executed every 10 minutes. It's possible to customize deduplication data expiration policy by using the `DeduplicationDataTimeToLive` and `FrequencyToRunDeduplicationDataCleanup`, as in the following snippet:

snippet: CustomExpiration

In the above sample deduplication data are preserved for 15 days and clean up is run once every 24 hours.

NOTE: While it is possible to use the same deduplication database for all endpoints within a single logical site, the Gateway assumes that _different_ logical sites (which are generally physically separated as well) will use separate storage infrastructure. Because sending a message to multiple sites will result in messages with the same message id delivered to each site, if those sites for some reason share a single deduplication table, the deduplication will not work correctly. In that case, separate the storage by using different database names.
