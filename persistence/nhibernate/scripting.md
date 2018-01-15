---
title: NHibernate Persistence Scripts
summary: Collection of scripts for managing NHibernate Persistence
component: NHibernate
reviewed: 2017-12-11
related:
 - nservicebus/operations
redirects:
 - nservicebus/nhibernate/scripting
---

WARNING: Ensure a backup of the database is created before executing any of the listed scripts.


## Remove subscriptions

Execute the following script against the database which is configured for NHibernate Persistence:

```sql
DELETE
FROM <subscriptionTable>
WHERE SubscriberEndpoint = '<subscriberAddress>'
```

Where:

 * `<subscriberAddress>` is the address of the subscriber. E.g. `My.Endpoint@subscriber-machine`.
 * `<subscriptionTable>` is the configured subscription table for NHibernate Persistence. By default this is `dbo.Subscription`

## Fix up TimeoutEntity_EndpointIdx index

Older versions of NServiceBus had an issue which caused the `TimeoutEntity_EndpointIdx` index to be either missing or created incorrectly. Now there is a built-in check and log warnings in cases when
 * the `TimeoutEntity_EndpointIdx` index has an incorrect column order
 * the system can not find `TimeoutEntity_EndpointIdx` index

In cases where the index was created incorrectly, use the following scripts to drop the index:

```
DROP INDEX [TimeoutEntity_EndpointIdx] ON [dbo].[TimeoutEntity] WITH ( ONLINE = OFF )
GO
```

and following script to create the correct one

```
CREATE CLUSTERED INDEX [TimeoutEntity_EndpointIdx] ON [dbo].[TimeoutEntity]
(
	[Endpoint] ASC,
	[Time] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
```

NOTE: The scripts above assume usage of the default schema `dbo`.
