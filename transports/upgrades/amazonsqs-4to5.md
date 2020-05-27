---
title: Upgrade AmazonSQS Transport Version 4 to 5
summary: Instructions on how to upgrade the AmazonSQS transport from version 4 to 5
component: SQS
isUpgradeGuide: true
reviewed: 2020-04-27
upgradeGuideCoreVersions:
 - 7
---

Upgrading from AmazonSQS transport version 4 to version 5 is a major upgrade and requires careful planning. Read the entire upgrade guide before beginning the upgrade process.

## Native publish subscribe

AmazonSQS transport version 5 introduces [native support for the publish subscribe pattern](/transports/sqs/topology.md) leveraging Amazon Simple Notification Service (SNS). Endpoints running on version 5 and above are able to publish and subscribe to events without requiring a separate persistence.

Before they are upgraded, endpoints running on older versions of the transport are not able to access the subscription data provided by native publish-subscribe. They will continue to send subscribe and unsubscribe control messages and will manage their own subscription storage, as described in [Message-driven publish-subscribe](/nservicebus/messaging/publish-subscribe/#mechanics-message-driven-persistence-based).

The transport provides a compatibility mode that allows the endpoint to use both forms of publish-subscribe at the same time. When it is enabled and the endpoint publishes an event, the native susbcription leveraging SNS and the message-driven subscription persistence are checked for subscriber information. This subscriber information is deduplicated before the event is published, so even if a subscriber appears in both places it will only receive a single copy of each event.

### Required [SNS permissions](https://docs.aws.amazon.com/sns/latest/dg/sns-access-policy-language-api-permissions-reference.html)

 * ListTopics
 * Unsubscribe
 * SetEndpointAttributes
 * ListSubscriptions
 * GetSubscriptionAttributes
 * SetSubscriptionAttributes

In addition to the above permissions the queue subscribing to a topic needs `sqs:SendMessage` permission to enable the topics delivering messages to the subscribing queue.

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Sid": "SomeSid",
      "Effect": "Allow",
      "Principal": {
        "AWS": "yourPrincipal"
      },
      "Action": "sqs:SendMessage",
      "Resource": "arn:aws:sqs:yourQueueArn",
      "Condition": {
        "ArnLike": {
          "aws:SourceArn": "arn:aws:sns:yourTopicArn"
        }
      }
    },
  ]
}
```

### Upgrading

Upgrade a single endpoint to version 5 at a time. Each upgraded endpoint should be configured to run in backwards compatibility mode and be deployed into production before upgrading the next endpoint. At startup, the upgraded endpoint will create the necessary topics on SNS and subscribe to them. It will also send subscribe control messages to each of it's configured publishers.

Once all endpoints in the system have been upgraded to version 5, the code that enables compatibility mode can be safely removed from each endpoint. It is recommended to run the entire system in backwards compatibility mode for a day or two before beginning to remove backwards compatibility mode. This allows all of the subscription control messages sent at endpoint startup to reach their destination and be fully processed.

#### Backwards compatibility configuration

snippet: 4to5-enable-message-driven-pub-sub-compatibility-mode

Message-driven publish-subscribe works by having the subscriber send control messages to the publisher to subscribe (or unsubscribe) from a type of event. When the publisher receives one of these subscription related control messages, it updates its private subscription persistence. When a publisher publishes an event, it checks its private subscription storage for a list of subscribers for that event type and sends a copy of the event to each subscriber.

A subscriber endpoint running in backwards compatibility mode will send subscription related control messages when subscribing to or unsubscribing from event types. The feature must be configured to associate each event type with the endpoint that publishes it. The configuration APIs to do this have been moved from the routing component to the compatibility component.

snippet: 4to5-configure-message-driven-pub-sub-routing

NOTE: Once the publishing endpoint has been upgraded, this configuration can optionally be removed.

A publisher endpoint running backwards compatibility mode will also handle incoming subscription related control messages to update both the native subscription table and the private subscription persistence.

### Operations

Alongside with the transport a new .NET Core tool `NServiceBus.AmazonSQS.CommandLine` was released, for setting up the required SQS queues, SNS topics/subscriptions, SQS policies for SNS, and the optional S3 buckets for large message support. For information, see the [documentation](/transports/sqs/operations-scripting.md).
