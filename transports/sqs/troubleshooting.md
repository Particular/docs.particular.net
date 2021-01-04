---
title: Troubleshooting
summary: Tips on what to do when the SQS transport is not behaving as expected
component: SQS
reviewed: 2021-01-04
related:
 - transports/sqs
 - samples/sqs/simple
---

## Viewing the message ID on the SQS message

To assist with troubleshooting, the NServiceBus [message ID](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-messageid) is propagated to the underlying SQS message as a message attribute. That way, even if message deserialization fails, the NServiceBus message ID is still available to help diagnose problems.

## AmazonSQSException: Request is throttled

Amazon SQS can handle large, continuous throughput but if there are sudden spikes, the service may apply throttling.

When throttling happens, the following exception is logged:

```
2017-11-14 23:10:24,314|ERROR|18|NServiceBus.Transports.SQS.MessageDispatcher|Exception from Send.
Amazon.SQS.AmazonSQSException: Request is throttled. ---> Amazon.Runtime.Internal.HttpErrorResponseException: The remote server returned an error: (403) Forbidden. ---> System.Net.WebException: The remote server returned an error: (403) Forbidden.
```

Throttling is more likely to happen when sending a large number messages concurrently. For example, using a list of tasks when using async/await.

To avoid Amazon throttling errors, it is possible to [tune the client retry behavior](https://docs.aws.amazon.com/sdk-for-net/latest/developer-guide/retries-timeouts.html) or limit the maximum number of concurrent sends. For example, allow only a small amount of messages to be sent concurrently as outlined in the [sending large amount of messages](/nservicebus/handlers/async-handlers.md#concurrency-large-amount-of-concurrent-message-operations) guidelines or send messages sequentially.

Throttling can happen during any send or receive operation and can happen during the following scenarios:

- Incoming message (receiving)
- Sending from within a handler
- Sending outside of a handler


### Incoming message (receiving)

For incoming messages throttling errors can be safely ignored as the message pump will try to fetch the next available message again.

### Sending from within a handler

Failing message sends raise an exception when throttled. The exception will be handled by the [recoverability feature](/nservicebus/recoverability/) mechanism. An incoming message that continuously fails due to throttling errors will be moved to the error queue.

A throttling error could result in partial message delivery while the incoming message is not processed successfully and can occur regardless of using the default [batched message dispatch](/nservicebus/messaging/batched-dispatch.md) or when using [immediate dispatch](/nservicebus/messaging/send-a-message.md#dispatching-a-message-immediately).

Throttling errors are similar to any other technical error that can occur.

### Sending outside of a handler

As message sending does not happen within a handler context any failures during sending will not rely or be covered by the [recoverability feature](/nservicebus/recoverability/) mechanism. Any retry logic must be manually implemented.

When throttling occurs with no custom error logic implemented, one or more messages might not have been transmitted to Amazon SQS. The custom retry logic could either retry all messages to be sent again, including already succeeded messages or only retry individual messages that failed.

## Deduplication and outbox

Because the throttling errors are common when using SQS, it is very important to make sure that the whole message processing logic is idempotent (including both updating the business state and generating outgoing messages). The simplest way to achieve this is to use the [Outbox](/nservicebus/outbox/) feature.

## On endpoint shutdown messages might be only visible after the visibility timeout has expired

After an endpoint is shutdown messages that were in flight but not handled might not be visible in the input queue for up to 30 seconds (default message visibility timeout). Messages will not be lost and will reappear in the input queue once the visibility timeout is expired. This is due to an [issue in the AWS SDK](https://github.com/aws/aws-sdk-net/issues/796#issuecomment-375494537).
