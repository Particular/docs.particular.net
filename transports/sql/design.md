---
title: Design
summary: The design and implementation details of SQL Server Transport
reviewed: 2016-08-08
component: SqlTransport
tags:
- Transactions
- Transport
redirects:
 - nservicebus/sqlserver/design
 - transports/sqlserver/design
---

### Primary queue

Each endpoint has a single table representing the primary queue. The name of the primary queue matches the name of the endpoint.

In a scale out scenario this single queue is shared by all instances.


### Other queues

Each endpoint also has queues required by timeout (the exact names and number of queues created depends on the version of the transport) and retry mechanisms.

Error and audit queues are usually shared among multiple endpoints.


Receiving messages is conducted by a `DELETE` statement from the top of the table (the oldest row according to the `[RowVersion]` column).


partial: indexes
