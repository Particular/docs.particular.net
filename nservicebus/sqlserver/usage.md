---
title: SQL Server Transport usage scenarios
summary: The usage scenarios for SQLServer transport
tags:
- SQL Server
---

SQL Server transport can be used with any supported persistence, e.g. RavenDB. However, because of reliability considerations the most common combination is SQL Server transport and NHibernate persistence. The detailed description of delivery guarantees is [here](index.md).


## Popular usage scenarios


### Messaging framework for the enterprise

The SQL Server transport throughput is on par with the Microsoft Message Queueing (MSMQ) service. The exact numbers depend on the hardware and software configuration but are in the range of few thousands messages per second per endpoint instance. If the enterprise has a solid high-performance SQL Server infrastructure, taking advantage of that infrastructure to implement messaging backbone is the natural choice when moving from isolated applications to connected services. In the enterprise scenario there are probably multiple systems, each having their own database. SQL Server transport [multi database](multiple-databases.md) has been designed exactly for such scenarios. It can be used in two transaction modes
 * **DTC** The receive operation, all data access and all the send operations potentially targeting different databases are conducted in a single distributed transaction coordinated by the DTC. This mode requires DTC service to be configured properly (including the high availability configuration matching the one of the SQL Server). The downside is that if a destination database is brought down for maintenance, the transactions that try to send messages to it are going to fail and this might cause the domino effect in other systems.
 * **Outbox** The *exactly-once* delivery guarantee is provided by the outbox. No DTC service is required. Messages addressed to a database that is temporarily down will by retried by means of [Second-Level Retries](/nservicebus/errors/automatic-retries.md) so SLR timeout larger than maintenance windows should be configured. The downside is that all the systems need to have Outbox enabled which requires them to use NServiceBus 5.x or later.


### Background worker replacement

SQL Server transport is ideal for small web applications that grew to require reliable background processing capabilities. In this case the web app, instead of firing a background thread or doing `asyc` processing, would send a message on the bus. The message would be picked up by the backend service, processed and the response returned. SQL Server transport will work out-of-the-box in that scenario without requiring DTC.


### Pilot project

The SQL Server transport is ideal for a pilot project which goal is to prove feasibility of of NServiceBus in a given organization. It does not require anything more than a database. Such a project would usually be small so it is not likely to use more than one physical database in which case no DTC is required.


### Messaging framework for an application using SQL Azure

The SQL Azure's limitations include no support distributed transactions and relatively small database size. This makes it an ideal candidate for a multi-database mode SQL Server transport with outbox. Each endpoint can have its own database that fits in the limit and outbox guarantees *exactly-once* processing of messages.