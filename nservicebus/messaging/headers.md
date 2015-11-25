---
title: Message Headers
summary: List of built-in NServiceBus message headers.
tags: 
- Header
redirects:
- nservicebus/message-headers
- nservicebus/messaging/headers
related:
- samples/header-manipulation
- nservicebus/messaging/header-manipulation
---

Extra information about a message is communicated over the transport as secondary information to the message body. Message headers are very similar, in both implementation and usage, to http headers. This article covers all the headers that are used internally by NServiceBus. To learn more on how to use custom headers, see the [header manipulation](/nservicebus/messaging/header-manipulation.md) article.


## Timestamp format

For all timestamp message headers the format is `yyyy-MM-dd HH:mm:ss:ffffff Z` and are in UTC. There is a helper class `DateTimeExtensions` in the NServiceBus core that supports converting to (`ToWireFormattedString()`) and from (`ToUtcDateTime()`) this format. This class effectively does the following. 

```
const string Format = "yyyy-MM-dd HH:mm:ss:ffffff Z";

public static string ToWireFormattedString(DateTime dateTime)
{
    return dateTime.ToUniversalTime()
        .ToString(Format, CultureInfo.InvariantCulture);
}

public static DateTime ToUtcDateTime(string wireFormattedString)
{
    return DateTime.ParseExact(wireFormattedString, Format, CultureInfo.InvariantCulture)
       .ToUniversalTime();
}
```


## Serialization Headers

This set of headers contains information to control how messages are [de-serialized](/nservicebus/serialization/) by the receiving endpoint.

 * `NServiceBus.ContentType`: The type of serialization used for the message. For example ` text/xml` or `text/json`. The `NServiceBus.ContentType` header was added in version 4.0. In some cases it may be useful to use the `NServiceBus.Version` header to determine when to use the `NServiceBus.ContentType` header. 
 * `NServiceBus.EnclosedMessageTypes`: The fully qualified type name of the enclosed message(s).


## Messaging interaction headers

Several headers are used to enable messaging interaction patters

 * `NServiceBus.MessageId`: A unique id for the current message. Note that the value used for an outgoing message can be controlled by the endpoint, using an `IMutateOutgoingTransportMessages`. 
 * `NServiceBus.CorrelationId`: A string used to [correlate](./message-correlation.md) reply messages to their corresponding request message. 
 * `NServiceBus.ConversationId`: The conversation that this message is part of
 * `NServiceBus.RelatedTo`: The `MessageId` that caused this message to be sent
 * `NServiceBus.MessageIntent`: Can be one of the following:
	* `Send`: Regular point-to-point send. Note that messages sent to Error queue will also have a `Send` intent.
	* `Publish`: The message is an event that has been published and will be sent to all subscribers.
	* `Subscribe`: A control message indicating that the source endpoint would like to subscribe to a specific message. 
	* `Unsubscribe`: A control message indicating that the source endpoint would like to unsubscribe to a specific message.
	* `Reply`: The message has been initiated by doing a Reply or a Return from within a Handler or a Saga.
 * `NServiceBus.ReplyToAddress`: The queue address that instructs downstream handlers or sagas where to send to when doing a Reply or Return.


## Send Headers

When a message is sent the headers will be as follows:

snippet:HeaderWriterSend

The above example headers are for a Send and hence the `MessageIntent` header is `Send`. If this was a Publish the `MessageIntent` header would be `Publish`.


## Reply Headers

When doing a Reply to a Message

 * The `MessageIntent` is `Reply`.
 * The `RelatedTo` will be the same as the initiating `MessageID`.
 * The `ConversationId` will be the same as the initiating `ConversationId`.
 * The `CorrelationId` will be same as the initiating `CorrelationId`.


### Example Reply Headers

Given an initiating message with the following headers:

<!-- import HeaderWriterReply_Sending --> 

The headers of reply message will be as follows:

snippet:HeaderWriterReply_Replying


## Publish Headers

When a message is published the headers will be as follows:

snippet:HeaderWriterPublish


## Return from a Handler

When doing a Return:

 * The Return has the same points as the Reply example from above with some additions.
 * The `ReturnMessage.ErrorCode` contains the value that was supplied to the `Bus.Return` method.
 

### Example Return Headers

Given an initiating message with the following headers:

<!-- import HeaderWriterReturn_Sending --> 

The headers of reply message will be as follows:

snippet:HeaderWriterReturn_Returning


## Dispatching a message from a Saga 

When any message is dispatched from within a Saga the message will contain the following:

 * A `OriginatingSagaId` header which matches the id used as the index for the Saga Data stored in persistence.
 * A `OriginatingSagaType` which is the fully qualified type name of the saga that sent the message


### Example "Send from Saga" Headers

snippet:HeaderWriterSaga_Sending


## Replying to a Saga

A message Reply is performed from a Saga will have the following headers:

 * The send headers are basically the same as a normal Reply with a few additions. 
 * Since this reply is from a secondary Saga then `OriginatingSagaId` and `OriginatingSagaType` will match the second saga
 * Since this is a Reply to a the initial Saga then the headers will contain `SagaId` and `SagaType` headers that match the initial Saga.
 

### Example "Replying to a Saga" Headers


#### Via calling Bus.Reply

snippet:HeaderWriterSaga_Replying


#### Via calling Saga.ReplyToOriginator

snippet:HeaderWriterSaga_ReplyingToOriginator


## Timeout Headers

 * `NServiceBus.ClearTimeouts`: A marker header to indicate that the contained control message is requesting that timeouts be cleared for a given saga.
 * `NServiceBus.Timeout.Expire`: A timestamp that indicates when a timeout to be fired.
 * `NServiceBus.Timeout.RouteExpiredTimeoutTo`: The queue name where a timeout should be routed back to when it fires.
 * `NServiceBus.IsDeferredMessage`: A marker header to indicate that this message resulted from a Defer.


## Requesting a Timeout from a Saga

When requesting a Timeout from a Saga:
 
 * The `OriginatingSagaId`, `OriginatingSagaType`, `SagaId` and `SagaType` will all match the Saga that requested the Timeout.
 * The `Timeout.RouteExpiredTimeoutTo` header contains the queue name for where the callback for the timeout should be sent/
 * The `Timeout.Expire` header contains the timestamp for when the timeout should fire.


### Example Timeout Headers

snippet:HeaderWriterSaga_Timeout


### Defer a Message

When doing a Defer the message will have similar header to a Send with a few editions:

 * The message will have `IsDeferredMessage` with the value of `true`.
 * Since the Defer feature uses the Timeouts feature the Timeout headers will exist.
 * The `Timeout.RouteExpiredTimeoutTo` header contains the queue name for where the callback for the timeout should be sent.
 * The `Timeout.Expire` header contains the timestamp for when the timeout should fire.


### Example Defer Headers

snippet:HeaderWriterDefer


## Diagnostics and Informational Headers

Headers used to give visibility into "where", "when" and "by whom" Of a message. Used by [ServiceControl](/servicecontrol/), [ServiceInsight](/serviceinsight/) and [ServicePulse](/servicepulse/). 

 * `$.diagnostics`: The [host details](/nservicebus/hosting/override-hostid.md) of the endpoint where the message was being processed. This header contains three parts:
	 * `$.diagnostics.hostdisplayname`
	 * `$.diagnostics.hostid` 
	 * `$.diagnostics.originating.hostid` 
 * `NServiceBus.TimeSent`: The timestamp of when the message was sent. Used by the [Performance Counters](/nservicebus/operations/performance-counters.md).
 * `NServiceBus.OriginatingEndpoint`: The endpoint name where the message was sent from. 
 * `NServiceBus.OriginatingMachine`: The machine name where the message was sent from.
 * `NServiceBus.Version`: The NServiceBus version number.


## Audit Headers

Headers added when a message is [Audited](/nservicebus/operations/auditing.md)

 * `NServiceBus.ProcessingEnded`: The timestamp processing of a message ended.
 * `NServiceBus.ProcessingEndpoint`: Name of the endpoint where the message was processed.
 * `NServiceBus.ProcessingMachine`: Machine name of the endpoint where the message was processed.
 * `NServiceBus.ProcessingStarted`: The timestamp the processing of this message started.


### Example Audit Headers

Given an initiating message with the following headers:

snippet:HeaderWriterAudit_Send

When that message fails to be processed it will be sent to the Error queue with the following headers:

snippet:HeaderWriterAudit_Audit


## Retries handling headers

Headers used facilitate [Retries](/nservicebus/errors/automatic-retries.md).

 * `NServiceBus.FailedQ`: The queue at which the message processing failed.
 * `NServiceBus.FLRetries`: The number of first-level retries that has been performed for a message.
 * `NServiceBus.Retries`: Number of second-level retries that has been performed for a message.
 * `NServiceBus.Retries.Timestamp`: A timestamp used by Second Level Retries to determine if the maximum time for retrying has been reached.


## Error forwarding headers

When a message is sent to the Error queue it will have the following extra headers added to the existing headers. If retries occurred then those messages will be included with the exception of `NServiceBus.Retries`, which is removed.

 * `NServiceBus.ExceptionInfo.ExceptionType`: The [Type.FullName](https://msdn.microsoft.com/en-us/library/system.type.fullname.aspx) of the Exception. Obtained by calling `Exception.GetType().FullName`.
 * `NServiceBus.ExceptionInfo.InnerExceptionType`: The full Type name of the [InnerException](https://msdn.microsoft.com/en-us/library/system.exception.innerexception.aspx) if it exists. Obtained by calling `Exception.InnerException.GetType().FullName`.
 * `NServiceBus.ExceptionInfo.HelpLink`: The [Exception HelpLink](https://msdn.microsoft.com/en-us/library/system.exception.helplink.aspx).
 * `NServiceBus.ExceptionInfo.Message`: The [Exception Message](https://msdn.microsoft.com/en-us/library/system.exception.message.aspx).
 * `NServiceBus.ExceptionInfo.Source` The [Exception Source](https://msdn.microsoft.com/en-us/library/system.exception.source.aspx).
 * `NServiceBus.ExceptionInfo.StackTrace` The [Exception StackTrace](https://msdn.microsoft.com/en-us/library/system.exception.stacktrace.aspx).
 

### Example Error Headers

Given an initiating message with the following headers:

snippet:HeaderWriterError_Sending

When that message fails to be processed it will be sent to the Error queue with the following headers:

snippet:HeaderWriterError_Error


## Encryption Headers

Headers when using [Rijndael property encryption](/nservicebus/security/encryption.md).

 * `NServiceBus.RijndaelKeyIdentifier`: Identifies which encryption key used for encryption the message property fragments.


#### Example Headers

<!-- import HeaderWriterEncryption --> 


## FileShare DataBus Headers

When using the [FileShare DataBus](/nservicebus/messaging/databus.md) extra headers and serialized message information is necessary to correlate between the information on the queue and the data on the file system. 


### When using DataBusProperty

When using the `DataBusProperty` NServiceBus uses that property as a placeholder at serialization time. the the serialized value of that property will contain a key. This key maps to a named header. That header then provides the path suffix of where that binary data is stored on disk on the file system.


#### Example Headers

<!-- import HeaderWriterDataBusProperty --> 


#### Example Body

<!-- import HeaderWriterDataBusProperty_Body --> 


### When using Convention

When using a [Conventions](/nservicebus/messaging/messages-events-commands.md#defining-messages-conventions) there is no way to store a correlation value inside the serialized property. Instead each property has a matching header with the property name used as the header suffix. That header then provides the path suffix of where that binary data is stored on disk on the file system.


#### Example Headers

snippet:HeaderWriterDataBusConvention


#### Example Body

snippet:HeaderWriterDataBusConvention_Body
