---
title: NHibernate Persistence Scripts
summary: Collection of scripts for managing NHibernate Persistence
tags:
 - NHibernate
---

WARNING: Ensure to create a backup of the database before executing any of the listed scripts.


## Remove subscriptions

Execute the following script against the database which is configured for NHibernate Persistence:

```sql
DELETE
FROM <database>
WHERE SubscriberEndpoint = '<distributorAddress>'
```

where 
* `<distributorAddress>` is the address of the subscriber. E.g. `My.Endpoint@subscriber-machine`.
* `<database>` is the configured database for NHibernate Peristence. By default this is `dbo.Subscription`