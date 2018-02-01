---
title: SQL Attachments
summary: Use SQL Server varbinary to store attachments for messages.
reviewed: 2018-01-29
component: AttachmentsSql
related:
 - samples/attachments-sql
---

Uses a SQL Server [varbinary](https://docs.microsoft.com/en-us/sql/t-sql/data-types/binary-and-varbinary-transact-sql) to store attachments for messages.


## Usage

Two settings are required as part of the default usage:

 * A connection factory that returns an open instance of a [SqlConnection](https://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.aspx). Note that any Exception that occurs during opening the connection should be handled by the factory.
 * A default time to keep for attachments.

snippet: EnableAttachments


### Recommended Usage

Extract out the connection factory to a helper method

snippet: OpenConnection

Also uses the `NServiceBus.Attachments.TimeToKeep.Default` method for attachment cleanup. See [Controlling the lifetime of attachments](#controlling-attachment-lifetime)

This usage results in the following:

snippet: EnableAttachmentsRecommended


## Installation


### Script execution runs by default at endpoint startup

To streamline development the attachment installer is, by default, executed at endpoint startup, in the same manner as all other [installers](/nservicebus/operations/installers.md).

snippet: ExecuteAtStartup

NOTE: Note that this is also a valid approach for higher level environments.


### Optionally take control of script execution

However in higher level environment scenarios, where standard installers are being run, but the SQL attachment installation has been executed as part of a deployment, it may be necessary to explicitly disable the attachment installer executing while leaving standard installers enabled.

snippet: DisableInstaller


## Data Cleanup

Attachment cleanup is enabled by default. It can be disabled using the following:

snippet: DisableCleanupTask


## Controlling attachment lifetime 

When the cleanup task runs it uses the `Expiry` column to determine if a given attachment should be deleted. This column is populated when an attachment ti written. When adding an attachment to an outgoing message, all methods accept an optional parameter `timeToKeep` of the type `GetTimeToKeep`. `GetTimeToKeep` is defined as:

```
public delegate TimeSpan GetTimeToKeep(TimeSpan? messageTimeToBeReceived);
```

Where `messageTimeToBeReceived` is value of [TimeToBeReceived](/nservicebus/messaging/discard-old-messages.md). If no `timeToKeep` parameter for a specific attachment is defined then the endpoint level `timeToKeep` is used.

The result of `timeToKeep` is then added to the current date and persisted to the `Expiry` column.

The method `NServiceBus.Attachments.TimeToKeep.Default` provides a recommended default for for attachment lifetime calculation: 

 * If [TimeToBeReceived](/nservicebus/messaging/discard-old-messages.md) is defined then keep attachment for twice that time.
 * Else; keep for 10 days.


## Table Name

The default table name and schema is `dbo.Attachments`. It can be changed with the following:

snippet: UseTableName


## Reading and writing attachments


### Writing attachments to an outgoing message

Approaches to using attachments for an outgoing message.

Note: [Stream.Dispose](https://msdn.microsoft.com/en-us/library/ms227422.aspx) is call after the data has been persisted. As such it is not necessary for any code using attachments to perform this cleanup. 

While the below examples illustrate adding an attachment to `SendOptions`, equivalent operations can be performed on `PublishOptions` and `ReplyOptions`


#### Factory Approach

The recommended approach for adding an attachment is by providing a delegate that constructs the stream. The execution of this delegate is then deferred until later in the outgoing pipeline, when the instance of the stream is required to be persisted. 

There are both async ans sync variants.

snippet: OutgoingFactory

snippet: OutgoingFactoryAsync


#### Instance Approach

In some cases an instance of a stream is already available in scope and as such it can be passed directly.

snippet: OutgoingInstance


### Reading attachments for an incoming message

Approaches to using attachments for the current incoming message.


#### Process named with a delegate

Processes an attachment with a specific name.

snippet: ProcessStream


#### Process all with a delegate

Processes all attachments.

snippet: ProcessStreams


#### Copy to a stream

Copy an attachment with a specific name to another stream.

snippet: CopyTo


#### Get an instance of a stream

Get a stream for an attachment with a specific name.

snippet: GetStream


#### Get data as bytes

Get a byte array for an attachment with a specific name.

WARNING: This should only be used the the data size is know to be small as it causes the full size of the attachment to be allocated in memory.

snippet: GetBytes


### Reading attachments for a specific message

All of the above examples have companion methods that are suffixed with `ForMessage`. These methods allow a handler or saga to read any attachments as long as the message id for that attachment is known. For example processing all attachments for a specific message could be done as follows 

snippet: ProcessStreamsForMessage

This can be helpful in a saga that is operating in a [Scatter-Gather](http://www.enterpriseintegrationpatterns.com/patterns/messaging/BroadcastAggregate.html) mode. So instead of storing all binaries inside the saga persister, the saga can instead store the message ids and then, at a latter point in time, access those attachments. 


## Unit Testing

The below examples also use the [NServiceBus.Testing](/nservicebus/testing/) extension.


### Testing outgoing attachments

snippet: TestOutgoingHandler

snippet: TestOutgoing


### Testing incoming attachments


#### Injecting a custom instance 

To mock or verify incoming attachments is it necessary to inject a instance of `IMessageAttachments` into the current `IMessageHandlerContext`. This can be done using the `MockAttachmentHelper.InjectAttachmentsInstance()` extension method which exists in the `NServiceBus.Attachments.Testing` namespace.

snippet: InjectAttachmentsInstance

The implementation of `IMessageHandlerContext` can be a custom coded mock or constructed using any of the popular mocking/assertion frameworks.

There is a default implementation of `IMessageAttachments` named  `MockMessageAttachments`. This implementation stubs out all methods. All members are virtual so it can be used as simplified base class for custom mocks.

snippet: CustomMockMessageAttachments

Putting these parts together allows a handler, using incoming attachments, to be tested.

snippet: TestOutgoingHandler

snippet: TestIncoming


