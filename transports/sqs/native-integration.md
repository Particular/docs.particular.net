---
title: AmazonSQS native integration
summary: Considerations when integrating NServiceBus endpoints with native Amazon SQS publishers and consumers.
reviewed: 2026-11-02
component: sqs
versions: '[5,]'
related:
 - samples/aws/sqs-native-integration
---

This document describes how to consume messages from non-NServiceBus endpoints via Amazon SQS in integration scenarios.

> [!NOTE]
> To send messages to non-NServiceBus endpoints, configure the sender endpoint so that it does not wrap outgoing messages in a transport envelope. For more information refer to the [transport configuration options](configuration-options.md#do-not-wrap-message-payload-in-a-transport-envelope).

## Accessing the native Amazon SQS message

It is sometimes useful to access the native Amazon SQS message from behaviors and handlers. When a message is received, the transport adds the native message, an instance of [`Amazon.SQS.Model.Message`](https://docs.aws.amazon.com/sdkfornet/v4/apidocs/items/SQS/TMessage.html), to the message processing context. For example, the native message may be accessed from a [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md):

snippet: sqs-access-to-native-message

partial: messagetypedetection

## Retrying failed messages

Native messages that failed processing can be retried using ServicePulse and ServiceControl, but the native message attributes that might have been present in the original message are lost when the message is retried.

#if-version [7,)
Because the transport [promotes native message attributes to headers](#native-message-attributes-promotion), their values are still available as message headers when the retried message is processed.
#end-if

partial: native-message-attributes-promotion
