---
title: Native integration
summary: Considerations when integrating NServiceBus endpoints with native Amazon SQS publishers and consumers.
reviewed: 2020-12-18
component: sqs
versions: '[5.3,]'
---

This document describes how to consume messages from and send messages to non-NServiceBus endpoints via Amazon SQS in integration scenarios.

### Access to the native Amazon SQS message details

It can sometimes be useful to access the native Amazon SQS message from behaviors and handlers. When a message is received, the transport adds the native message [`Amazon.SQS.Model.Message`](https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/SQS/TMessage.html) to the message processing context. Use the code below to access the message details from a [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md):

snippet: sqs-access-to-native-message
