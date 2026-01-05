---
title: Message Headers
summary: List of built-in NServiceBus message headers.
reviewed: 2025-06-16
component: Core
versions: '[5.0,)'
redirects:
 - nservicebus/message-headers
 - nservicebus/messaging/message-correlation
related:
 - samples/header-manipulation
 - nservicebus/messaging/header-manipulation
---

The headers of a message are similar to HTTP headers and contain metadata about the message being sent over the queueing system. This document describes the headers used by NServiceBus. To learn more about how to use custom headers, see the documentation on [manipulating message headers](/nservicebus/messaging/header-manipulation.md).


## Timestamp format

#if-version [8,)
For all timestamp message headers, the format is `yyyy-MM-dd HH:mm:ss:ffffff Z`, where the time is UTC. The helper class `DateTimeOffsetHelper` supports converting from UTC to wire format and vice versa by using the `ToWireFormattedString(DateTimeOffset)` and `ToDateTimeOffset(string)` methods.
#end-if

#if-version [,8)
For all timestamp message headers, the format is `yyyy-MM-dd HH:mm:ss:ffffff Z` where the time is UTC. The helper class `DateTimeExtensions` supports converting from UTC to wire format and vice versa by using the `ToWireFormattedString(DateTime)` and `ToUtcDateTime(string)` methods.
#end-if

### ISO 8601 format

This is NOT the [ISO 8601 format](https://nl.wikipedia.org/wiki/ISO_8601) but a custom format.

Differences:

1. a `T` between date and time
2. uses a `.` between seconds and milli/microseconds
3. no space between the timestamp and the `Z`

```
ISO 8601:    yyyy-MM-ddTHH:mm:ss.ffffffZ
NServiceBus: yyyy-MM-dd HH:mm:ss:ffffff Z
```

When doing native intergration and there is a need to parse the timestamp the `.` as second and milli/microsecond separator is where regular timestamp parsers often fail.

## Transport headers

### NServiceBus.NonDurableMessage

The `NonDurableMessage` header controls [non-durable messaging](non-durable-messaging.md) persistence behavior of in-flight messages. The behavior is transport-specific, but the intent is to not store the message durably on disk and only keep it in memory.

### NServiceBus.TimeToBeReceived

The `TimeToBeReceived` header [controls when a message becomes obsolete and can be discarded](discard-old-messages.md). The behavior is transport-dependent.

### NServiceBus.Transport.Encoding

States what type of body serialization is used.

It is used only by the legacy Azure Service Bus transport (`NServiceBus.Azure.Transports.WindowsAzureServiceBus` package), which is no longer supported.

## Serialization headers

The following headers include information for the receiving endpoint on the [message serialization](/nservicebus/serialization/) option that was used.

### NServiceBus.ContentType

The type of serialization used for the message, for example, `text/xml`, `text/json`, `application/json`, or `application/json; systemjson`.

In some cases, the `NServiceBus.Version` header may be useful for determining how to use the value in this header appropriately.

> [!WARNING]
> Although this header mimicks the [HTTP Content-Type header](https://www.rfc-editor.org/rfc/rfc9110.html#name-content-type) the values are case-sensitive. The header value does not behave like HTTP headers where everything after `;` is used to order and match the best qualified (application/json) serializer. Adding a suffix like `; systemjson` requires **all** endpoints involved to use this full key (for example: `application/json; systemjson`).

### NServiceBus.EnclosedMessageTypes

The fully qualified .NET type name of the enclosed message(s). The receiving endpoint will use this type when deserializing an incoming message. Depending on the [versioning strategy](/samples/versioning/) the type can be specified in the following ways:

* Full type name: `Namespace.ClassName`.
* Assembly qualified name: `Namespace.ClassName, AssemblyName, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null`.

See the [message type detection documentation](/nservicebus/messaging/message-type-detection.md) for more details.

## Messaging interaction headers

The following headers enable different messaging interaction patterns, such as Request-Response.

### NServiceBus.MessageId

A [unique ID for the current message](/nservicebus/messaging/message-identity.md).

### NServiceBus.CorrelationId

NServiceBus implements the [Correlation Identifier](https://www.enterpriseintegrationpatterns.com/patterns/messaging/CorrelationIdentifier.html) pattern by using a `Correlation Id` header.

Message correlation connects request messages with their corresponding response messages. The `Correlation Id` of the response message is the `Correlation Id` of its corresponding request message. Each outgoing message which is sent outside of a message handler will have its `Correlation Id` set to its `Message Id`.

An example of Correlation Identifier usage within NServiceBus can be found in [callbacks](/nservicebus/messaging/callbacks.md).

Messages sent from a [saga](/nservicebus/sagas/) using the `ReplyToOriginator` method will have their `Correlation Id` set based on the message which caused the saga to be created. See [Notifying callers of status](/nservicebus/sagas/#notifying-callers-of-status) for more information about the `ReplyToOriginator` method.

### CorrId

`CorrId` is an MSMQ specific header semantically identical to `NServiceBus.CorrelationId`. It is included only for backward compatibility with endpoints running version 3 or older of NServiceBus.

### NServiceBus.ConversationId

The identifier of the conversation that this message is part of. It enables the tracking of message flows that span more than one message exchange. `ConversationId`, `RelatedTo`, `OriginatingEndpoint`, and `ProcessingEndpoint` fields allow [ServicePulse](/servicepulse/message-details.md#messages-with-audited-conversation-data-flow-diagram) to reconstruct the entire message flow.

The first message sent in a new flow is automatically assigned a unique `ConversationId` that gets propagated to all the messages that are sent afterward, forming a _conversation_. Each message sent within a conversation has a `RelatedTo` value that identifies the message that caused it to be sent.

The `ConversationId` must be assigned manually in cases where NServiceBus can't infer when messages belong to the same conversation. For example, when a `CancelOrder` message must be part of an existing order conversation, the Order ID can be used as the Conversation ID. Manually assigning a `ConversationId` can be achieved by overriding the header with a custom value:

snippet: override-conversation-id

partial: conversationid

> [!WARNING]
> Attempting to override an existing Conversation ID is not supported and will produce the following error:
> ```
> Cannot set the NServiceBus.ConversationId header to 'XXXXX' as it cannot override the incoming header value ('2f4076a0-d8de-4297-9d18-a830015dd42a').
> ```

> [!NOTE]
> `Conversation Id` is very similar to `Correlation Id`. Both headers are copied to each new message that an endpoint produces. Whereas `Conversation Id` is always copied from the incoming message being handled, `Correlation Id` can come from another source (such as when replying from a saga using `ReplyToOriginator(...)`).

partial: newconversationid


### NServiceBus.RelatedTo

The `MessageId` that caused the current message to be sent. Whenever a message is sent or published from inside a message handler, its `RelatedTo` header is set to the `MessageId` of the incoming message being handled.

> [!NOTE]
> For a single request-response interaction, `Correlation Id` and `RelatedTo` are very similar. Both headers can correlate the response message back to the request message. Once a _conversation_ is longer than a single request-response interaction, `Correlation Id` can correlate a response to the original request. `RelatedTo` can only correlate a message to the previous message in the same _conversation_.


### NServiceBus.MessageIntent

Message intent can have one of the following values:

| Value       | Description                                                                                                                                                        |
|-------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Send        | Regular point-to-point send. Note that messages sent to the error queue will also have a `Send` intent                                                             |
| Publish     | The message is an event that has been published and will be sent to all subscribers.                                                                               |
| Subscribe   | A [control message](#messaging-interaction-headers-nservicebus-messageintent) indicating that the source endpoint wants to subscribe to a specific message.   |
| Unsubscribe | A [control message](#messaging-interaction-headers-nservicebus-messageintent) indicating that the source endpoint wants to unsubscribe to a specific message. |
| Reply       | The message has been initiated by doing a Reply or a Return from within a Handler or a Saga.                                                                       |

### NServiceBus.ControlMessage

Indicates that the message is a control message, i.e., it has no body, and the intent of the message and any data are transmitted in the message headers.

### NServiceBus.ReplyToAddress

Downstream message [handlers](/nservicebus/handlers) or [sagas](/nservicebus/sagas) use this value as the reply queue address when replying or returning a message.

## Send headers

When a message is sent, the headers will be as follows:

snippet: HeaderWriterSend

In the above example, the headers are for a Send operation, and hence, the `MessageIntent` header is `Send`. If the message were published instead, the `MessageIntent` header would be `Publish`.

## Reply headers

When replying to a message:

* The `MessageIntent` is `Reply`.
* The `RelatedTo` will be the same as the initiating `MessageID`.
* The `ConversationId` will be the same as the initiating `ConversationId`.
* The `CorrelationId` will be the same as the initiating `CorrelationId`.

### Example reply headers

Given an initiating message with the following headers:

snippet: HeaderWriterReplySending

The headers of the reply message will be:

snippet: HeaderWriterReplyReplying

## Publish headers

When a message is published, the headers will be as follows:

snippet: HeaderWriterPublish

## Return from a handler

When returning a message instead of replying:

* The Return has the same points as the Reply example above but with some additions.
* The `ReturnMessage.ErrorCode` contains the value supplied to the `Bus.Return` method.

### Example return headers

Given an initiating message with the following headers:

snippet: HeaderWriterReturnSending

The headers of the reply message will be:

snippet: HeaderWriterReturnReturning

## Timeout headers

### NServiceBus.ClearTimeouts

This is a header to indicate that the contained [control message](#messaging-interaction-headers-nservicebus-messageintent) requests that timeouts be cleared for a given saga.

### NServiceBus.Timeout.Expire

A timestamp that indicates when a timeout should be fired.

### NServiceBus.Timeout.RouteExpiredTimeoutTo

The queue name to which a timeout should be routed back to when it expires.

### NServiceBus.IsDeferredMessage

A header to indicate that this message resulted from a message deferral.

## Saga-related headers

When a message is dispatched from within a saga, the message will contain the following headers:

* `OriginatingSagaId`: matches the ID used as the index for the saga data stored in persistence.
* `OriginatingSagaType`: the fully qualified type name of the saga that sent the message.

### Example "send from saga" headers

snippet: HeaderWriterSagaSending

### Replying to a saga

A message reply is performed from a saga will have the following headers:

* The send headers are the same as a standard reply header with a few additions.
* Since this reply is from a secondary saga, then `OriginatingSagaId` and `OriginatingSagaType` will match the second saga.
* Since this is a reply to the initial saga, the headers will contain `SagaId` and `SagaType` headers matching the initial saga.

### Example "replying to a saga" headers

#### Via calling Bus.Reply

snippet: HeaderWriterSagaReplying

#### Via calling Saga.ReplyToOriginator

snippet: HeaderWriterSagaReplyingToOriginator

### Requesting a timeout from a saga

When requesting a timeout from a saga:

* The `OriginatingSagaId`, `OriginatingSagaType`, `SagaId` and `SagaType` will all match the Saga that requested the Timeout.
* The `Timeout.RouteExpiredTimeoutTo` header contains the queue name for where the callback for the timeout should be sent.
* The `Timeout.Expire` header contains the timestamp for when the timeout should fire.

#### Example timeout headers

snippet: HeaderWriterSagaTimeout

## Defer a message

partial: defer

### Example defer headers

snippet: HeaderWriterDefer

## Diagnostic and informational headers

Headers used to give visibility into the "where," "when," and "by whom" of a message. They are used by [ServiceControl](/servicecontrol/) and [ServicePulse](/servicepulse/).

### $.diagnostics

The [host details](/nservicebus/hosting/override-hostid.md) of the endpoint where the message was being processed. This header contains three parts:

* `$.diagnostics.hostdisplayname`
* `$.diagnostics.hostid`
* `$.diagnostics.originating.hostid`

### NServiceBus.TimeSent

The timestamp when the message was sent, used by the [Performance Counters](/monitoring/metrics/performance-counters.md).

### NServiceBus.DeliverAt

The timestamp when the message should be delivered. Used for more accurate calculation of [critical time](/monitoring/metrics/definitions.md#metrics-captured-critical-time).

### NServiceBus.OriginatingEndpoint

The endpoint name the message was sent from.

> [!NOTE]
> Used for linking messages in ServicePulse. See [NServiceBus.ConversationId](#messaging-interaction-headers-nservicebus-conversationid)

### NServiceBus.OriginatingMachine

The machine name the message was sent from.

### NServiceBus.Version

The NServiceBus version number.

## OpenTelemetry-related headers

These headers are added when [OpenTelemetry](/nservicebus/operations/opentelemetry.md) is enabled for an endpoint, in accordance with the [W3C Trace Context specification](https://www.w3.org/TR/trace-context):

* [`traceparent`](https://www.w3.org/TR/trace-context/#traceparent-header)
* [`tracestate`](https://www.w3.org/TR/trace-context/#tracestate-header)
* [`baggage`](https://www.w3.org/TR/baggage/#baggage-http-header-format)

## Audit headers

Headers added when a message is [audited](/nservicebus/operations/auditing.md).

### NServiceBus.ProcessingEnded

The timestamp when the processing of a message ended.

### NServiceBus.ProcessingEndpoint

Name of the endpoint where the message was processed.

> [!NOTE]
> Used for linking messages in ServicePulse. See [NServiceBus.ConversationId](#messaging-interaction-headers-nservicebus-conversationid)

### NServiceBus.ProcessingMachine

The machine name of the endpoint where the message was processed.

### NServiceBus.ProcessingStarted

The timestamp when the processing of this message started.

### Example audit headers

Given an initiating message with the following headers:

snippet: HeaderWriterAuditSend

when that message fails to be processed, it will be sent to the error queue with the following headers:

snippet: HeaderWriterAuditAudit


## Retries handling headers

Headers used to facilitate [retries](/nservicebus/recoverability/).

> [!NOTE]
> These headers only exist after the first round of immediate reties has finished and are removed before sending a message to the error queue after all allowed retry attempts are exhausted.

### NServiceBus.Retries

The number of [delayed retries](/nservicebus/recoverability/#delayed-retries) that have been performed for a message.

### NServiceBus.Retries.Timestamp

A timestamp used by [delayed retries](/nservicebus/recoverability/#delayed-retries) to determine if the maximum time for retrying has been reached.

## Error forwarding headers

When a message exhausts the configured number of retry attempts and is moved to the error queue by the [recoverability](/nservicebus/recoverability/) component, it will have the following extra headers added to the existing headers.

### NServiceBus.FailedQ

The queue at which the message processing failed.

### NServiceBus.ExceptionInfo.ExceptionType

The [Type.FullName](https://msdn.microsoft.com/en-us/library/system.type.fullname.aspx) of the Exception. It is obtained by calling `Exception.GetType().FullName`.

### NServiceBus.ExceptionInfo.InnerExceptionType

The full type name of the [InnerException](https://msdn.microsoft.com/en-us/library/system.exception.innerexception.aspx) if it exists. It is obtained by calling `Exception.InnerException.GetType().FullName`.

### NServiceBus.ExceptionInfo.HelpLink

The [exception help link](https://msdn.microsoft.com/en-us/library/system.exception.helplink.aspx).

### NServiceBus.ExceptionInfo.Message

The [exception message](https://msdn.microsoft.com/en-us/library/system.exception.message.aspx).

### NServiceBus.ExceptionInfo.Source

The [exception source](https://msdn.microsoft.com/en-us/library/system.exception.source.aspx).

### NServiceBus.ExceptionInfo.StackTrace

The [exception stack trace](https://msdn.microsoft.com/en-us/library/system.exception.stacktrace.aspx).

### Example error headers

Given an initiating message with the following headers:

snippet: HeaderWriterErrorSending

when that message fails to be processed, it will be sent to the error queue with the following headers:

snippet: HeaderWriterErrorError

## Encryption headers

Headers when using [message property encryption](/nservicebus/security/property-encryption.md).

### NServiceBus.RijndaelKeyIdentifier

Identifies the encryption key used for encryption of the message property fragments.

#### Example headers

snippet: HeaderWriterEncryption

#### Example body

snippet: HeaderWriterEncryptionBody

## File share data bus headers

When using the [file share data bus](/nservicebus/messaging/claimcheck/file-share.md), extra headers and serialized message information are necessary to correlate between the information on the queue and the data on the file system.

### Using DataBusProperty

When using the `DataBusProperty`, NServiceBus uses that property as a placeholder at serialization time. The serialized value of that property will contain a key. This key maps to a named header. That header then provides the path suffix of where that binary data is stored on disk on the file system.

partial: databuscontenttype

#### Example headers

snippet: HeaderWriterDataBusProperty

#### Example body

snippet: HeaderWriterDataBusPropertyBody

### Using conventions

When using [conventions](/nservicebus/messaging/conventions.md) there is no way to store a correlation value inside the serialized property. Instead, each property has a matching header with the property name used as the header suffix. That header then provides the path suffix of where that binary data is stored on disk on the file system.

#### Example headers

snippet: HeaderWriterDataBusConvention

#### Example body

snippet: HeaderWriterDataBusConventionBody

## ServiceControl

### ServiceControl.RetryTo

Value: Queue name

When present in a failed message to ServiceControl, ServiceControl will send the message to this queue instead of the queue name value from [NServiceBus.FailedQ](#error-forwarding-headers-nservicebus-failedq)

This is used by the ServiceControl transport adapter to bridge failed messages between different transports.

### ServiceControl.TargetEndpointAddress

Value: Queue name

Used by the [messaging bridge](/nservicebus/bridge) to return failed messages back to the correct queue

### ServiceControl.Retry.AcknowledgementQueue

Value: Queue name

The queue to send an acknowledgement system message back to a specific ServiceControl queue to mark a retried message as processed.

### ServiceControl.Retry.Successful

Contains a timestamp in the format `yyyy-MM-dd HH:mm:ss:ffffff Z` to indicate when a message was succesfully processed after a retry from ServiceControl.

Part of the control message sent to ServiceControl to signal that a message was manually retried in ServicePulse/Control and flagged as processed successfully.

### ServiceControl.Retry.UniqueMessageId

Contains the [NServiceBus.MessageId](#messaging-interaction-headers-nservicebus-messageid) value of the successfully processed message.

Part of the control message send back to ServiceControl to signal that a message that was manually retried in ServicePulse/Control and flagged as processed successfully.

The presence of any header key that starts with `ServiceControl.` indicates that the message has been retried from ServiceControl.
