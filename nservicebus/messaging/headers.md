---
title: Message Headers
summary: List of built-in NServiceBus message headers.
reviewed: 2016-11-08
component: Core
versions: '[5.0,)'
tags:
 - Header
redirects:
 - nservicebus/message-headers
 - nservicebus/messaging/headers
 - nservicebus/messaging/message-correlation
related:
 - samples/header-manipulation
 - nservicebus/messaging/header-manipulation
---

The headers in a message contain information that is used by the messaging infrastructure to help with the message delivery. Message headers are very similar, in both implementation and usage, to HTTP headers. To learn more about how to use custom headers, see the [header manipulation](/nservicebus/messaging/header-manipulation.md) article.


## Timestamp format

For all timestamp message headers, the format is `yyyy-MM-dd HH:mm:ss:ffffff Z` where the time is UTC. The helper class `DateTimeExtensions` supports converting from UTC to wire format and vice versa by using the `ToWireFormattedString()` and `ToUtcDateTime()` methods.

```cs
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

The following headers include information for the receiving endpoint on the [message serialization](/nservicebus/serialization/) option that was used.


### NServiceBus.ContentType

The type of serialization used for the message, for example `text/xml` or `text/json`. This header was added in Version 4.0. In some cases, it may be useful to use the `NServiceBus.Version` header to determine how to use the value in this header appropriately.


### NServiceBus.EnclosedMessageTypes

The fully qualified .NET type name of the enclosed message(s). The receiving endpoint will use this type when deserializing an incoming message. Depending on the [versioning strategy](/samples/versioning/) the type can be specified in the following ways:

 * Full type name: `Namespace.ClassName`.
 * Assembly qualified name: `Namespace.ClassName, AssemblyName, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null`.

NOTE: In integration scenarios, this header can be safely omitted if the endpoint uses [XmlSerialization](/nservicebus/serialization/xml.md) and the root node happens to be the message type.


## Messaging interaction headers

The following headers are used in various ways to enable different messaging interaction patterns such as Request-Response, etc.


### NServiceBus.MessageId

A [unique ID for the current message](/nservicebus/messaging/message-identity.md).


### NServiceBus.CorrelationId

NServiceBus implements the [Correlation Identifier](http://www.enterpriseintegrationpatterns.com/patterns/messaging/CorrelationIdentifier.html) pattern by using a `Correlation Id` header.

Message correlation connects request messages with their corresponding response messages. The `Correlation Id` of the response message is the `Correlation Id` of its corresponding request message. Each outgoing message which is sent outside of a message handler will have it's `Correlation Id` set to it's `Message Id`.

An example usage of Correlation Identifier within NServiceBus is [callbacks](/nservicebus/messaging/callbacks.md).

Messages sent from a Saga using the `ReplyToOriginator` method will have their `Correlation Id` set based on the message which caused the saga to be created. See [Notifying callers of status](/nservicebus/sagas/#notifying-callers-of-status) for more information about the `ReplyToOriginator` method.


### NServiceBus.ConversationId

Identifier of the conversation that this message is part of. It enables the tracking of message flows that span more than one message exchange. `Conversation Id` and `RelatedTo` fields allow [ServiceInsight](/serviceinsight/#flow-diagram) to reconstruct the entire message flow.

The first message that is sent in a new flow is automatically assigned a unique `Conversation Id` that is then propagated to all the messages that are subsequently sent, thus forming a _conversation_. Each message that is sent within a conversation also has a `RelatedTo` value that identifies the originating message that caused it to be sent. 

NOTE: `Conversation Id` is very similar to `Correlation Id`. Both headers are sticky and are copied to each new message that an endpoint produces. Whereas `Conversation Id` is always copied from the incoming message being handled, `Correlation Id` can come from another source (such as when replying from a Saga using `ReplyToOriginator(...)`).


### NServiceBus.RelatedTo

The `MessageId` that caused this message to be sent. Whenever a message is sent or published from inside a message handler, it's `RelatedTo` header is set to the `MessageId` of the incoming message that was being handled.

NOTE: For a single request-response interaction `Correlation Id` and `RelatedTo` are very similar. Both headers are able to correlate the response message back to the request message. Once a _conversation_ is longer than a single request-response interaction, `Correlation Id` can be used to correlate a response to the original request. `RelatedTo` can only correlate a message back to the previous message in the same _conversation_.


### NServiceBus.MessageIntent

Message intent can have one of the following values:

| Value         | Description |
| ------------- |-------------|
| Send |Regular point-to-point send. Note that messages sent to Error queue will also have a `Send` intent|
| Publish |The message is an event that has been published and will be sent to all subscribers.|
| Subscribe |A control message indicating that the source endpoint would like to subscribe to a specific message.|
| Unsubscribe |A control message indicating that the source endpoint would like to unsubscribe to a specific message.|
| Reply | The message has been initiated by doing a Reply or a Return from within a Handler or a Saga. |


### NServiceBus.ReplyToAddress

Downstream message [Handlers](/nservicebus/handlers) or [Sagas](/nservicebus/sagas) use this value as the reply queue address when replying or returning a message.


## Send Headers

When a message is sent the headers will be as follows:

snippet: HeaderWriterSend

The above example headers are for a Send and hence the `MessageIntent` header is `Send`. If the message was published instead, the `MessageIntent` header would be `Publish`.


## Reply Headers

When replying to a message:

 * The `MessageIntent` is `Reply`.
 * The `RelatedTo` will be the same as the initiating `MessageID`.
 * The `ConversationId` will be the same as the initiating `ConversationId`.
 * The `CorrelationId` will be same as the initiating `CorrelationId`.


### Example Reply Headers

Given an initiating message with the following headers:

snippet: HeaderWriterReplySending

The headers of reply message will be as follows:

snippet: HeaderWriterReplyReplying


## Publish Headers

When a message is published the headers will be as follows:

snippet: HeaderWriterPublish


## Return from a Handler

When returning a message instead of replying:

 * The Return has the same points as the Reply example from above with some additions.
 * The `ReturnMessage.ErrorCode` contains the value that was supplied to the `Bus.Return` method.


### Example Return Headers

Given an initiating message with the following headers:

snippet: HeaderWriterReturnSending

The headers of reply message will be as follows:

snippet: HeaderWriterReturnReturning


## Timeout Headers


### NServiceBus.ClearTimeouts

A marker header to indicate that the contained control message is requesting that timeouts be cleared for a given saga.


### NServiceBus.Timeout.Expire

A timestamp that indicates when a timeout to be fired.


### NServiceBus.Timeout.RouteExpiredTimeoutTo

The queue name where a timeout should be routed back to when it fires.


### NServiceBus.IsDeferredMessage

A marker header to indicate that this message resulted from a Defer.


## Saga Related Headers

When a message is dispatched from within a Saga the message will contain the following:

 * A `OriginatingSagaId` header which matches the ID used as the index for the Saga Data stored in persistence.
 * A `OriginatingSagaType` which is the fully qualified type name of the saga that sent the message.


### Example "Send from Saga" Headers

snippet: HeaderWriterSagaSending


### Replying to a Saga

A message Reply is performed from a Saga will have the following headers:

 * The send headers are the same as a normal Reply headers with a few additions.
 * Since this reply is from a secondary Saga then `OriginatingSagaId` and `OriginatingSagaType` will match the second saga.
 * Since this is a Reply to a the initial Saga then the headers will contain `SagaId` and `SagaType` headers that match the initial Saga.


### Example "Replying to a Saga" Headers


#### Via calling Bus.Reply

snippet: HeaderWriterSagaReplying


#### Via calling Saga.ReplyToOriginator

snippet: HeaderWriterSagaReplyingToOriginator


### Requesting a Timeout from a Saga

When requesting a Timeout from a Saga:

 * The `OriginatingSagaId`, `OriginatingSagaType`, `SagaId` and `SagaType` will all match the Saga that requested the Timeout.
 * The `Timeout.RouteExpiredTimeoutTo` header contains the queue name for where the callback for the timeout should be sent.
 * The `Timeout.Expire` header contains the timestamp for when the timeout should fire.


#### Example Timeout Headers

snippet: HeaderWriterSagaTimeout


## Defer a Message

When doing a Defer the message will have similar header to a Send with a few editions:

 * The message will have `IsDeferredMessage` with the value of `true`.
 * Since the Defer feature uses the Timeouts feature the Timeout headers will exist.
 * The `Timeout.RouteExpiredTimeoutTo` header contains the queue name for where the callback for the timeout should be sent.
 * The `Timeout.Expire` header contains the timestamp for when the timeout should fire.


### Example Defer Headers

snippet: HeaderWriterDefer


## Diagnostic and Informational Headers

Headers used to give visibility into "where", "when" and "by whom" Of a message. Used by [ServiceControl](/servicecontrol/), [ServiceInsight](/serviceinsight/) and [ServicePulse](/servicepulse/).


### $.diagnostics

The [host details](/nservicebus/hosting/override-hostid.md) of the endpoint where the message was being processed. This header contains three parts:

 * `$.diagnostics.hostdisplayname`
 * `$.diagnostics.hostid`
 * `$.diagnostics.originating.hostid`


### NServiceBus.TimeSent

The timestamp of when the message was sent. Used by the [Performance Counters](/nservicebus/operations/performance-counters.md).


### NServiceBus.OriginatingEndpoint

The endpoint name where the message was sent from.


### NServiceBus.OriginatingMachine

The machine name where the message was sent from.


### NServiceBus.Version

The NServiceBus version number.


## Audit Headers

Headers added when a message is [Audited](/nservicebus/operations/auditing.md)


### NServiceBus.ProcessingEnded

The timestamp processing of a message ended.


### NServiceBus.ProcessingEndpoint

Name of the endpoint where the message was processed.


### NServiceBus.ProcessingMachine

The machine name of the endpoint where the message was processed.


### NServiceBus.ProcessingStarted

The timestamp the processing of this message started.


### Example Audit Headers

Given an initiating message with the following headers:

snippet: HeaderWriterAuditSend

When that message fails to be processed it will be sent to the Error queue with the following headers:

snippet: HeaderWriterAuditAudit


## Retries handling headers

Headers used to facilitate [Retries](/nservicebus/recoverability/).

Note: These headers will only exist after the initial retry has occurred.


### NServiceBus.FailedQ

The queue at which the message processing failed.


### NServiceBus.FLRetries

The number of [Immediate Retries](/nservicebus/recoverability/#immediate-retries) that has been performed for a message. 


### NServiceBus.Retries

The number of [Delayed Retries](/nservicebus/recoverability/#delayed-retries) that has been performed for a message.


### NServiceBus.Retries.Timestamp

A timestamp used by [Delayed Retries](/nservicebus/recoverability/#delayed-retries) to determine if the maximum time for retrying has been reached.


## Error forwarding headers

When a message handled by [recoverability](/nservicebus/recoverability/), it will have the following extra headers added to the existing headers. If retries occurred, then those messages will be included with the exception of `NServiceBus.Retries`, which is removed.


### NServiceBus.ExceptionInfo.ExceptionType

The [Type.FullName](https://msdn.microsoft.com/en-us/library/system.type.fullname.aspx) of the Exception. Obtained by calling `Exception.GetType().FullName`.


### NServiceBus.ExceptionInfo.InnerExceptionType

The full Type name of the [InnerException](https://msdn.microsoft.com/en-us/library/system.exception.innerexception.aspx) if it exists. Obtained by calling `Exception.InnerException.GetType().FullName`.


### NServiceBus.ExceptionInfo.HelpLink

The [Exception HelpLink](https://msdn.microsoft.com/en-us/library/system.exception.helplink.aspx).


### NServiceBus.ExceptionInfo.Message

The [Exception Message](https://msdn.microsoft.com/en-us/library/system.exception.message.aspx).


### NServiceBus.ExceptionInfo.Source

The [Exception Source](https://msdn.microsoft.com/en-us/library/system.exception.source.aspx).


### NServiceBus.ExceptionInfo.StackTrace

The [Exception StackTrace](https://msdn.microsoft.com/en-us/library/system.exception.stacktrace.aspx).


### Example Error Headers

Given an initiating message with the following headers:

snippet: HeaderWriterErrorSending

When that message fails to be processed it will be sent to the Error queue with the following headers:

snippet: HeaderWriterErrorError


## Encryption Headers

Headers when using [message property encryption](/nservicebus/security/property-encryption.md).


### NServiceBus.RijndaelKeyIdentifier

Identifies which encryption key used for encryption the message property fragments.


#### Example Headers

snippet: HeaderWriterEncryption


#### Example Body

snippet: HeaderWriterEncryptionBody


## FileShare DataBus Headers

When using the [FileShare DataBus](/nservicebus/messaging/databus/file-share.md) extra headers and serialized message information is necessary to correlate between the information on the queue and the data on the file system.


### When using DataBusProperty

When using the `DataBusProperty`, NServiceBus uses that property as a placeholder at serialization time. The serialized value of that property will contain a key. This key maps to a named header. That header then provides the path suffix of where that binary data is stored on disk on the file system.


#### Example Headers

snippet: HeaderWriterDataBusProperty


#### Example Body

snippet: HeaderWriterDataBusPropertyBody


### When using Convention

When using [Conventions](/nservicebus/messaging/conventions.md) there is no way to store a correlation value inside the serialized property. Instead, each property has a matching header with the property name used as the header suffix. That header then provides the path suffix of where that binary data is stored on disk on the file system.


#### Example Headers

snippet: HeaderWriterDataBusConvention


#### Example Body

snippet: HeaderWriterDataBusConventionBody
