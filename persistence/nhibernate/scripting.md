---
title: NHibernate Persistence Scripts
summary: Collection of scripts for managing NHibernate Persistence
component: NHibernate
reviewed: 2017-04-05
related:
 - nservicebus/operations
redirects:
 - nservicebus/nhibernate/scripting
---

WARNING: Ensure to create a backup of the database before executing any of the listed scripts.


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
