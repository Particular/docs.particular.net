---
title: Serilog
summary: Logging to Serilog
reviewed: 2018-11-04
component: Serilog
tags:
 - Logging
related:
 - nservicebus/logging
 - samples/logging/serilog-custom
 - samples/logging/serilog-seq
---

Support for writing all NServiceBus log entries to [Serilog](https://serilog.net/).


## Usage

snippet: SerilogInCode


## Seq

To log to [Seq](https://getseq.net/):

snippet: SerilogSeq


## Filtering

NServiceBus can write a significant amount of information to the log. To limit this information use the filtering features of the underlying logging framework.

For example to limit log output to a specific namespace.

Here is a code configuration example for adding a [Filter](https://github.com/serilog/serilog/wiki/Configuration-Basics#filters).

snippet: SerilogFiltering


## Tracing

Writing diagnostic log entries to [Serilog](https://serilog.net/). Plugs into the low level [pipeline](/nservicebus/pipeline) to give more detailed diagnostics.

When using Serilog for tracing it is optional to use Serilog as the main NServiceBus logger. i.e. no need to include `LogManager.Use<SerilogFactory>();`.


### Create an instance of a Serilog Logger

snippet: SerilogTracingLogger


### Configure the Tracing feature to use that logger

snippet: SerilogTracingPassLoggerToFeature


### Contextual Logger

Serilog Tracing injects a contextual `Serilog.Ilogger` into the NServiceB pipeline. This logger is [enriched](https://github.com/serilog/serilog/wiki/Enrichment) with the following values:

There are several layers of enrichment based on the pipeline phase.

NOTE: Saga and message tracing will use the current contextual logger.


#### Endpoint enrichment

All loggers for an endpoint will have the the property `ProcessingEndpoint` added that contains the current [endpoint name](/nservicebus/endpoints/specify-endpoint-name.md).


#### Incoming message enrichment

When a message is received the following enrichment properties are added:

 * [SourceContext](https://github.com/serilog/serilog/wiki/Writing-Log-Events#source-contexts) will be the the message type [FullName](https://docs.microsoft.com/de-de/dotnet/api/system.type.fullname) extracted from the [EnclosedMessageTypes header](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes). `UnknownMessageType` will be used if no header exists. The same value will be added to a property named `MessageType`.
 * `MessageId` will be the value of the [MessageId header](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-messageid).
 * `CorrelationId` will be the value of the [CorrelationId header](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-correlationid) if it exists.
 * `ConversationId` will be the value of the [ConversationId header](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-conversationid) if it exists.


#### Handler enrichment

When a handler is invoked the a new logger is forked from the above enriched physical logger with a new enriched property named `Handler` that contains the the [FullName](https://docs.microsoft.com/de-de/dotnet/api/system.type.fullname) of the current handler.


#### Outgoing message enrichment

When a send the same properties as described in "Incoming message enrichment" will be added to the outgoing pipeline. Note that if a handler sends a message, the logger injected into the outgoing pipeline will be forked from the logger instance as described in "Handler enrichment". As such it will contain a property `Handler` for the handler that sent the message.


#### Accessing the logger

The contextual logger instance can be access from anywhere in the pipeline via `SerilogTracingExtensions.Logger(this IPipelineContext context)`.

snippet: ContextualLoggerUsage


### Exception enrichment

When an exception occurs in the message processing pipeline, the exception is amended with the current with the current pipeline state. When that exception is logged that state can be add to the log entry.

The type added to the exception data is `ExceptionLogState`. It contains the following data:

 * `ProcessingEndpoint` added that contains the current [endpoint name](/nservicebus/endpoints/specify-endpoint-name.md).
 * `MessageId` will be the value of the [MessageId header](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-messageid).
 * `MessageType` will be the the message type [FullName](https://docs.microsoft.com/de-de/dotnet/api/system.type.fullname) extracted from the [EnclosedMessageTypes header](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes). `UnknownMessageType` will be used if no header exists.
 * `CorrelationId` will be the value of the [CorrelationId header](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-correlationid) if it exists.
 * `ConversationId` will be the value of the [ConversationId header](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-conversationid) if it exists.

The instance of `ExceptionLogState` can be access using the following.

snippet: ExceptionLogState

If routing NServiceBus log event suing `LogManager.Use<SerilogFactory>();` then the above properties will be promoted to the log event.


### Saga Tracing

snippet: EnableSagaTracing


### Message Tracing

Both incoming and outgoing messages will be logged at the [Information level](https://github.com/serilog/serilog/wiki/Writing-Log-Events#the-role-of-the-information-level). The current message will be included in a property named `Message`. For outgoing messages any unicast routes will be included in a property named `UnicastRoutes`.

snippet: EnableMessageTracing


## Seq

To log to [Seq](https://getseq.net/):

snippet: SerilogTracingSeq
