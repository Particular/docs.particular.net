---
title: SQL Server Transport
summary: High-level description of NServiceBus SQL Server Transport.
reviewed: 2016-03-22
tags:
- SQL Server
redirects:
- nservicebus/sqlserver/usage
related:
- samples/outbox/sqltransport-nhpersistence
- samples/sqltransport-nhpersistence
- samples/outbox/sqltransport-nhpersistence-ef
---

The SQL Server transport implements a message queueing mechanism on top of Microsoft SQL Server. It provides support for sending messages over [SQL Server](http://www.microsoft.com/en-au/server-cloud/products/sql-server/) tables. It does **not** make any use of ServiceBroker, a messaging technology built into the SQL Server.


## How it works

The SQL Server transport is a hybrid queueing system which is neither store-and-forward (like MSMQ) nor a broker (like RabbitMQ). It treats the SQL Server as a storage infrastructure for the queues. The queueing logic itself is implemented and executed as part of the transport code running in an NServiceBus endpoint.


## Advantages and Disadvantages of choosing SQL Server Transport


### Advantages

 * No additional licensing and training costs, as majority of Microsoft stack organizations already have SQL Server installed and have knowledge required to run it
 * Great tooling (SSMS)
 * Maximum throughput for any given endpoint is on par with MSMQ
 * Queues support competing consumers (multiple instances of same endpoint feeding off of same queue) so scale-out doesn't require using [distributor](/nservicebus/scalability-and-ha/distributor/)


### Disadvantages

 * No local store-and-forward mechanism, meaning that when SQL Server instance is down the endpoint cannot send nor receive messages
 * In centralized deployment maximum throughput applies for the whole system, not individual endpoints. If SQL Server on a given hardware can handle 2000 msg/s, each one of the 10 endpoints deployed on this machine can only receive 200 msg/s (on average).


## Usage Scenarios


### Messaging framework for the enterprise

The SQL Server transport throughput is on par with the Microsoft Message Queueing (MSMQ) service. The actual value is dependent on the hardware and software configuration but is in the range few thousands messages per second per endpoint instance. If the enterprise has a solid high-performance SQL Server infrastructure, taking advantage of that infrastructure to implement messaging backbone is a natural choice when moving from isolated applications to connected services. In the enterprise scenario there are probably multiple systems and these systems have their own databases. SQL Server transport [multi database](multiple-databases.md) has been designed exactly for such scenarios. It can be used in two transaction modes

 * **DTC** The receive operation, all data access and all the send operations potentially targeting different databases are conducted in a single distributed transaction coordinated by the DTC. This mode requires DTC service to be configured properly (including the high availability configuration matching the one of the SQL Server). The downside is that if a destination database is brought down for maintenance, the transactions that try to send messages to it are going to fail and this might cause the domino effect in other systems.
 * [**Outbox**](/nservicebus/outbox/) The *exactly-once* delivery guarantee is provided by the outbox. No DTC service is required. Messages addressed to a database that is temporarily down will by retried by means of [Second-Level Retries](/nservicebus/errors/automatic-retries.md) so SLR timeout larger than maintenance windows should be configured. The downside is that all the systems need to have Outbox enabled which requires them to use NServiceBus 5.x or later.


### Background worker replacement

SQL Server transport works well for small web applications that require reliable background processing capabilities. Instead of firing a background thread or doing `async` processing the web application would send a message on the bus. The message would be picked up by the backend service, processed and the response returned. SQL Server transport will work out-of-the-box in that scenario without requiring DTC. 

Using messaging in that scenario is more reliable than using additional threads. Even if the processing of the message fails, the message itself is not lost and it can be retired at a later time. 


### Pilot project

The SQL Server transport is a good choice for a pilot project to prove feasibility of NServiceBus in a given organization. It does not require anything more than a database. Such a project would usually be small so it is not likely to use more than one physical database in which case no DTC is required.


### Messaging framework for an application using SQL Azure

The SQL Azure's limitations include no support for distributed transactions and relatively small database size. This makes it an ideal candidate for a [multi-database mode](/nservicebus/sqlserver/multiple-databases.md) SQL Server transport with [Outbox](/nservicebus/outbox/). Each endpoint could have its own database that meets the size limit. Outbox would guarantee *exactly-once* processing of messages.