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

NServiceBus requires the [the message type](/nservicebus/messaging/message-type-detection.md) to be available as part of the message metadata to process a message successfully. 

During message processing, the SQS transport inspects the native message attributes for an attribute with the name `MessageTypeFullName` and a value representing a full type name (i.ex `Sales.OrderAccepted`). If the attribute is present, the message is treated as a native message, and the body is deserialized into the target type represented by `MessageTypeFullName`. 

The native message body is loaded from the configured [S3 bucket](transports/sqs/configuration-options.md#s3bucketforlargemessages) when the message attribute contains an attribute with the key `S3BodyKey` and the value representing an S3 object key, including the [necessary prefix](/transports/sqs/configuration-options.md#s3bucketforlargemessages-s3keyprefix) as configured by the receiving endpoint.
