---
title: RavenDb License
component: raven
tags:
 - Persistence
reviewed: 2020-04-06
---
[RavenDb](https://ravendb.net/) is a choice as one of several persisters to store NServiceBus specific information.

[Hibernating Rhinos Ltd](https://hibernatingrhinos.com/) and [Particular Software Ltd](https://particular.net/) have reached an agreement whereby all NServiceBus users are granted a license to use RavenDB for NServiceBus' purposes and such use of RavenDB will be governed by the NServiceBus license agreement.

You may

- Install RavenDB for the purpose of storing NServiceBus on the same number of cores/machines as your NServiceBus license.
- Or with the same number of developers for which they have purchased an NServiceBus license.
- Run RavenDB using Windows Clustering (in accordance with Particular Software Licensing requirements)
- You cannot use RavenDB to store business data, only NServiceBus'.

### Can I store my own data on the RavenDB instance that is licensed via NServiceBus?
No. The license covers only NServiceBus own use of RavenDB. For example, storing subscriptions or sagas in RavenDB is covered. Storing your orders inside RavenDB isn't covered.

### What if I want to use RavenDB to store my own data (aside from NServiceBus data)?
You need to purchase a license for RavenDB to do so.
