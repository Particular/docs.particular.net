---
title: Licensing
summary: Outlines RavenDB license agreement
component: raven
reviewed: 2017-08-21
related:
 - nservicebus/operations
 - nservicebus/licensing
 - persistence/ravendb/installation
redirects:
 - nservicebus/ravendb/licensing
---

include: dtc-warning

## License Agreements

Particular Software has a special licensing agreement with [Hibernating Rhinos](https://hibernatingrhinos.com/) (makers of RavenDB) to allow certain usage of RavenDB without having to purchase a license. This license agreements only allows storage of NServiceBus related data in RavenDB. This includes subscriptions, saga data, timeouts, etc. The agreement is accessible on [this RavenDB web page](https://ravendb.net/nservicebus-and-ravendb).

If additional data needs to be stored in the RavenDB, aside from what is managed by NServiceBus, purchasing a suitabe RavenDB license is required.


## Running RavenDB externally

Since NServiceBus Version 5.x no longer installs and manages RavenDB automatically, a license file is required to register the RavenDB instance. If the usage is limited to the license agreement above, [email the support team](mailto://support@particular.net) to recieve a RavenDB license file. 
