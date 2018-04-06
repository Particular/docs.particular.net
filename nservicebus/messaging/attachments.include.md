

## Data Cleanup

Attachment cleanup is enabled by default. It can be disabled using the following:

snippet: DisableCleanupTask


## Controlling attachment lifetime 

When the cleanup task runs it uses the `Expiry` column to determine if a given attachment should be deleted. This column is populated when an attachment is written. When adding an attachment to an outgoing message, all methods accept an optional parameter `timeToKeep` of the type `GetTimeToKeep`. `GetTimeToKeep` is defined as:

```
public delegate TimeSpan GetTimeToKeep(TimeSpan? messageTimeToBeReceived);
```

Where `messageTimeToBeReceived` is value of [TimeToBeReceived](/nservicebus/messaging/discard-old-messages.md). If no `timeToKeep` parameter for a specific attachment is defined then the endpoint level `timeToKeep` is used.

The result of `timeToKeep` is then added to the current date and persisted to the `Expiry` column.

The method `NServiceBus.Attachments.TimeToKeep.Default` provides a recommended default for for attachment lifetime calculation: 

 * If [TimeToBeReceived](/nservicebus/messaging/discard-old-messages.md) is defined then keep attachment for twice that time.
 * Else; keep for 10 days.


## Reading and writing attachments


### Writing attachments to an outgoing message

Approaches to using attachments for an outgoing message.

Note: [Stream.Dispose](https://msdn.microsoft.com/en-us/library/ms227422.aspx) is call after the data has been persisted. As such it is not necessary for any code using attachments to perform this cleanup. 

While the below examples illustrate adding an attachment to `SendOptions`, equivalent operations can be performed on `PublishOptions` and `ReplyOptions`


#### Factory Approach

The recommended approach for adding an attachment is by providing a delegate that constructs the stream. The execution of this delegate is then deferred until later in the outgoing pipeline, when the instance of the stream is required to be persisted. 

There are both async and sync variants.

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

snippet: TestIncomingHandler

snippet: TestIncoming