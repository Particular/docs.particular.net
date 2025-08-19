---
title: NHibernate Persistence Scripts
summary: A collection of scripts for managing NHibernate persistence
component: NHibernate
reviewed: 2024-10-09
related:
 - nservicebus/operations
redirects:
 - nservicebus/nhibernate/scripting
---

> [!WARNING]
> Ensure there is a backup of the database before executing any of the scripts on this page.


## Remove subscriptions

Execute the following script against a database which is configured for NHibernate Persistence:

```sql
DELETE
FROM <subscriptionTable>
WHERE SubscriberEndpoint = '<subscriberAddress>'
```

Where:

 * `<subscriberAddress>` is the address of the subscriber. E.g. `My.Endpoint@subscriber-machine`.
 * `<subscriptionTable>` is the configured subscription table for NHibernate Persistence. By default this is `dbo.Subscription`

## Fix TimeoutEntity_EndpointIdx index

Older versions of NServiceBus had an issue which caused the `TimeoutEntity_EndpointIdx` index to be either missing or created incorrectly. Now there is a built-in check and log warnings in cases when
 * the `TimeoutEntity_EndpointIdx` index has an incorrect column order
 * the system can not find `TimeoutEntity_EndpointIdx` index

In cases where the index was created incorrectly, use the following script to drop the index:

```
DROP INDEX [TimeoutEntity_EndpointIdx] ON [dbo].[TimeoutEntity] WITH ( ONLINE = OFF )
GO
```

and the following script to create the correct one

```
CREATE CLUSTERED INDEX [TimeoutEntity_EndpointIdx] ON [dbo].[TimeoutEntity]
(
	[Endpoint] ASC,
	[Time] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
```

> [!NOTE]
> The scripts above assume usage of the default schema `dbo`.
