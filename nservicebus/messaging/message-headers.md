---
title: Message Headers
summary: Access and manipulate the built in NServiceBus headers or add custom headers.
tags: []
redirects:
- nservicebus/how-do-i-get-technical-information-about-a-message
- nservicebus/message-headers
---

Extra information about a message is communicated over the transport as secondary information to the message body. The mechanism for header communication is either native headers, if the transport supports that feature, or via a serialized collection of key value pairs. Message headers are very similar, in both implementation and usage, as http headers. NServiceBus uses them internally to manage messaging patters, eg pub/sub and request/response, and users of NServiceBus can use headers to implement a variety of features.


## All Message headers

The possible message headers are as follows:

 * `$.diagnostics`: The [host details](/nservicebus/hosting/override-hostid.md) of the endpoint where the message was being processed. This header contains three parts:
	 * `$.diagnostics.hostdisplayname`
	 * `$.diagnostics.hostid` 
	 * `$.diagnostics.originating.hostid` 
 * `NServiceBus.ContentType`: The type of [serialization](/nservicebus/serialization/) used for the message. For example ` text/xml` or `text/json`.
 * `NServiceBus.ConversationId`: The conversation that this message is part of
 * `NServiceBus.CorrelationId`: A guid used to correlate between handlers and/or Sagas when doing a Reply or a Return. 
 * `NServiceBus.EnclosedMessageTypes`: The fully qualified type name of the enclosed message(s).
 * `NServiceBus.MessageId`: A unique id for the current message.
 * `NServiceBus.MessageIntent`: Can be one of the following:
	 * `Send`: Regular point-to-point send. Note that messages sent to Error queue will also have a `Send` intent.
	 * `Publish`: The message is an event that has been published and will be sent to all subscribers.
	 * `Subscribe`: A control message indicating that the source endpoint would like to subscribe to a specifc message. 
	 * `Unsubscribe`: A control message indicating that the source endpoint would like to unsubscribe to a specifc message.
	 * `Reply`: The message has been initiated by doing a Reply or a Return from within a Handler or a Saga.
 * `NServiceBus.OriginatingEndpoint`: The endpoint name where the message was sent from. 
 * `NServiceBus.OriginatingMachine`: The machine name where the message was sent from.
 * `NServiceBus.ReplyToAddress`: The queue address that instructs downstream handlers or sagas where to send to when doing a Reply or Return.
 * `NServiceBus.TimeSent`: The UTC time-stamp of when the message was sent.
 * `NServiceBus.Version`: The NServiceBus version number.
 * `WinIdName`: The user name of the current user.
 * `NServiceBus.ClearTimeouts`: A marker header to indicate that the contained control message is requesting that timeouts be cleared for a given saga.
 * `NServiceBus.Retries.Timestamp`: A UTC time-stamp used by Second Level Retries to determine if the maximum time for retrying has been reached.
 * `NServiceBus.Timeout.Expire`: A UTC time-stamp that indicates when a timeout to be fired.
 * `NServiceBus.Timeout.RouteExpiredTimeoutTo`: The queue name where a timeout should be routed back to when it fires.
 * `NServiceBus.IsDeferredMessage`: A marker header to indicate that this message resulted from a Defer.


### Example Message Headers

When a message is sent the headers will be as follows:

<!-- import HeaderWriterSend -->

The above example headers are for a Send and hence the MessageIntent header is `Send`. If this was a Publish the MessageIntent header would be `Publish`.


## Reply to a message

When doing a Reply to a Message

 * The MessageIntent is `Reply`.
 * The RelatedTo will be the same as the initiating MessageID.
 * The ConversationId will be the same as the initiating ConversationId.
 * The CorrelationId will be same as the initiating CorrelationId.


### Example Message Headers

Given an initiating message with the following headers:

<!-- import HeaderWriterReply_Sending --> 

The headers of reply message will be as follows:

<!-- import HeaderWriterReply_Replying -->


## Defer a message

When doing a Defer the message will have similar header to a Send with a few editions:

 * The message will have IsDeferredMessage with the value of `true`.
 * Since the Defer feature uses the Timeouts feature the Timeout headers will exist.
 * The Timeout.RouteExpiredTimeoutTo header contains the queue name for where the callback for the timeout should be sent/
 * The Timeout.Expire header contains the timestamp for when the timeout should fire.


### Example Message Headers

<!-- import HeaderWriterDefer -->


## Return from a Handler

When doing a Return:

 * The Return has the same points as the Reply example from above with some additions.
 * The ReturnMessage.ErrorCode contains the value that was supplied to the `Bus.Return` method.
 

### Example Message Headers

Given an initiating message with the following headers:

<!-- import HeaderWriterReturn_Sending --> 

The headers of reply message will be as follows:

<!-- import HeaderWriterReturn_Returning -->


## Dispatching a message from a Saga 

When any message is distracted from within a Saga the message will contain the following:

 * A OriginatingSagaId header which matches the id used as the index for the Saga Data stored in persistence.
 * A OriginatingSagaType which is the fully qualified type name of the saga that sent the message


### Example Message Headers

<!-- import HeaderWriterSaga_Sending -->


## Replying to a Saga

A message Reply is performed from a Saga will have the following headers:

 * The send headers are basically the same as a normal Reply with a few additions. 
 * Since this reply is from a secondary Saga then OriginatingSagaId and OriginatingSagaType will match the second saga
 * Since this is a Reply to a the initial Saga then the headers will contain SagaId and SagaType headers that match the initial Saga.
 

### Example Message Headers

<!-- import HeaderWriterSaga_Replying -->


## Requesting a Timeout from a Saga

When requesting a Timeout from a Saga:
 
 * The OriginatingSagaId, OriginatingSagaType, SagaId and SagaType will all match the Saga that requested the Timeout.
 * The Timeout.RouteExpiredTimeoutTo header contains the queue name for where the callback for the timeout should be sent/
 * The Timeout.Expire header contains the timestamp for when the timeout should fire.


### Example Message Headers

<!-- import HeaderWriterSaga_Timeout -->

## Forward to Error queue 

When a message is forwarded to the error queue:

 * The headers are the same as the headers from the initiating message with a few additions.
 * Several ExceptionInfo headers that give details of the exception that caused the error.
 * A TimeOfFailure header which gives the UTC time that the error occurred.
 * A FailedQ header that contains the queue name that the failed message was read from.
 * A FLRetries header that gives a count of how many First-Level-Retries occured
 

### Example Message Headers

Given an initiating message with the following headers:

<!-- import HeaderWriterError_Sending -->

When that message fails to be processed it will be sent to the Error queue with the following headers:

<!-- import HeaderWriterError_Error -->


## Reading incoming Headers

Headers can be read for an incoming message.


### From a Behavior

<!-- import header-incoming-behavior -->


### From a Mutator

<!-- import header-incoming-mutator -->


### From a Handler

<!-- import header-incoming-handler -->


## Writing outgoing Headers

Headers can be written for an outgoing message.


### From a Behavior

<!-- import header-outgoing-behavior -->


### From a Mutator

<!-- import header-outgoing-mutator -->


### From a Handler

<!-- import header-outgoing-handler -->
