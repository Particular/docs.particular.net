---
title: SQL Server / NHibernate / Entity Framework / Outbox
summary: 'How to integrate SQLServer transport with NHibernate persistence and EntityFramework user data store using outbox'
tags:
- SQL Server
- NHibernate
- EntityFramework
- Outbox
- Pipeline
- Behavior
- Database
- DependencyLifecycle
- Sample
related:
- nservicebus/nhibernate
- nservicebus/sqlserver
- nservicebus/outbox
---

 1. Make sure you have SQL Server Express installed and accessible as `.\SQLEXPRESS`. Create two databases: `sender` and `receiver`.
  * You can also use localdb by replacing `SQLEXPRESS` with `(localdb)\v11.0`.
 2. Start the Sender project (right-click on the project, select the `Debug > Start new instance` option). 
 3. Start the Receiver project.
 4. If you see `DtcRunningWarning` log message in the console, it means you have a Distributed Transaction Coordinator (DTC) service running. The Outbox feature is designed to provide *exactly once* delivery guarantees without DTC. We believe it is better to disable the DTC service to avoid confusion when you use Outbox.
 5. In the Sender's console you should see `Press <enter> to send a message` text when the app is ready. 
 6. Hit <enter>.
 7. On the Receiver console you should see that order was submitted.
 8. On the Sender console you should see that the order was accepted.
 9. Finally, after a couple of seconds, on the Receiver console you should see that the timeout message has been received.
 10. Open SQL Server Management Studio and go to the receiver database. Verify that there is a row in saga state table (`dbo.OrderLifecycleSagaData`) and in the orders table (`dbo.Orders`)

## Code walk-through

This sample contains three projects: 

 * Shared - A class library containing common code including the message definitions.
 * Sender - A console application responsible for sending the initial `OrderSubmitted` message and processing the follow-up `OrderAccepted` message.
 * Receiver - A console application responsible for processing the order message.

Sender and Receiver use different databases, just like in a production scenario where two systems are integrated using NServiceBus. Each database contains, apart from business data, queues for the NServiceBus endpoint and tables for NServiceBus persistence.

### Sender project
 
The Sender does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end. It is configured to use SQLServer transport with NHibernate persistence and Outbox.

<!-- import SenderConfiguration -->

The Sender uses a configuration file to tell NServiceBus where the messages 
addressed to the Receiver should be sent

<!-- import SenderConnectionStrings -->

### Receiver project

The Receiver mimics a back-end system. It is also configured to use SQLServer transport with NHibernate persistence and Outbox but uses V2.1 code-based connection information API. It uses EntityFramework to store business data (orders)in a seperate handler.

<!-- import ReceiverConfiguration -->

In order for the Outbox to work, the business data has to reuse the same connection string as NServiceBus' persistence

<!-- import EntityFramework -->

When the message arrives at the Receiver, it is dequeued using a native SQL Server transaction. Then a `TransactionScope` is created that encompasses
 * persisting business data via a Entity Framework DbContext,

<!-- import StoreUserData -->

 * persisting saga data of `OrderLifecycleSaga` ,
 * storing the reply message and the timeout request in the outbox.

<!-- import Reply -->

<!-- import Timeout -->

Finally the messages in the outbox are pushed to their destinations. The timeout message gets stored in NServiceBus timeout store and is sent back to the saga after requested delay of five seconds.

### How it works?

### Database connection sharing

All the data manipulations happen atomically because SQL Server 2008 and later allows multiple (but not overlapping) instances of `SqlConnection` to enlist in one `TransactionScope` without the need to escalate to DTC. The SQL Server manages these transactions like they were one `SqlTransaction`. This does not work for all databases. This implementation shares the same `IDbConnection` between the dependencies that require the connection resulting on all dependencies to read and write to the same database.

### NHibernateStorageContext

NServiceBus.NHibernate has an `NHibernateStorageContext` which has a `IDbConnection` when it is available. This is used internally to share the connection to all persisters.

We configure the container how it can obtain an `IDbConnection` and specify that this instance needs to be shared within the unit of work.

<!-- import NHibernateStorageContextConnection -->


### Custom DbContext pipeline behavior

We cannot use `IManageUnitsOfWork` as this is to early in the pipeline as `NHibernateStorageContext` is initialized after the pipeline behavior responsible for managing `IManageUnitsOfWork`, so the connection to share is not yet available. 

### Sharing the `IDbConnection`

To get an initialized connection we create a custom pipeline behavior  which creates a configured `DbContext` via its generic argument.

<!-- import DbContextBehavior -->

### Behavior implementation

When the behavior is called it simply create the `DbContext` and when the pipeline is finished it calls `DbContext.SaveChanges()`.

<!-- import DbContextBehaviorSaveChanges -->

The behavior itself is actually easier to implement that an `IManageUnitsOfWork`. The only drawback is that it needs additional code to register this behavior in the pipeline.

### Configure bus to include `DbContext` behavior

The only thing left is getting the behavior hooked in the pipeline. This is done by configuring the bus in a `INeedInitialization` created specific for our dbcontext behavior as we need to configure it to use `ReceiverDataContext` but also need to configure the container how it can resolve both the `DbContextBehavior<ReceiverDataContext>` behavior and `ReceiverDataContext` DbContext.

<!-- import DbContextBehaviorContainerRegistration -->

Register behavior in pipeline:

<!-- import DbContextBehaviorPipelineRegistration -->

## Alternatives

An alternative of sharing the `IDbConnection` instance is instructing the container how to create the `ReceiverDataContext` by first resolving the `NHibernateStorageContext` and use that to build it.

<!-- import ReceiverDataContextAlternative -->

The current implementation is usefull when you would like to use the `IDbConnection` with, for example, the [Dapper](https://github.com/StackExchange/dapper-dot-net) micro orm framework.
