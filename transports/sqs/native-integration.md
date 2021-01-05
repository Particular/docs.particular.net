---
title: Native integration
summary: Considerations when integrating NServiceBus endpoints with native Amazon SQS publishers and consumers.
reviewed: 2020-12-18
component: sqs
versions: '[5.3,]'
---

This document describes how to consume messages from and send messages to non-NServiceBus endpoints via Amazon SQS in integration scenarios.

### Accessing the native Amazon SQS message

It is sometimes useful to access the native Amazon SQS message from behaviors and handlers. When a message is received, the transport adds the native message, an instance of [`Amazon.SQS.Model.Message`](https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/SQS/TMessage.html), to the message processing context. For example, the native message may be accessed from a [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md):

snippet: sqs-access-to-native-message

### Message type detection

The main requirement NServiceBus has to process a message, is [the message type](/nservicebus/messaging/message-type-detection.md).
When the SQS transport detects that the incoming message is not originating from an NServiceBus endpoint, it will inspect the native message's attributes for an attribute with name `MessageTypeFullName`.
If that attribute is available, the SQS transport can try to deserialize the message into that type and process it.

The transport will also verify if there's an attribute available with key `S3BodyKey`, when available, it can use its value to retrieve the body of the message from an SQS S3 bucket.