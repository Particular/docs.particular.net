---
title: AmazonSQS native integration
summary: Considerations when integrating NServiceBus endpoints with native Amazon SQS publishers and consumers.
reviewed: 2025-04-16
component: sqs
versions: '[5.3,]'
related:
 - samples/aws/sqs-native-integration
---

This document describes how to consume messages from non-NServiceBus endpoints via Amazon SQS in integration scenarios.

> [!NOTE]
> To send messages to non-NServiceBus endpoints, configure the sender endpoint so that it does not wrap outgoing messages in a transport envelope. For more information refer to the [transport configuration options](configuration-options.md#do-not-wrap-message-payload-in-a-transport-envelope).

### Accessing the native Amazon SQS message

It is sometimes useful to access the native Amazon SQS message from behaviors and handlers. When a message is received, the transport adds the native message, an instance of [`Amazon.SQS.Model.Message`](https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/SQS/TMessage.html), to the message processing context. For example, the native message may be accessed from a [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md):

snippet: sqs-access-to-native-message

partial: messagetypedetection

### Retrying failed messages

Native messages that failed processing can be retried using ServicePulse and ServiceControl but the native message attributes that might have been present in the original message are lost when the message is retried.

partial: native-message-attributes-promotion
