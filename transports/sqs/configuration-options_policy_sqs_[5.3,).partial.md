## Policy

include: configuration-options-policy-intro

The transport creates a policy statement for the event types it subscribes to:

```json
{
  ...
  "Statement": [
    {
      ...
      "Action": "sqs:SendMessage",
      "Resource": "arn:aws:sqs:some-region:some-account:endpoint",
      "Condition": {
        "ArnLike": {
          "aws:SourceArn": [
            "arn:aws:sns:some-region:some-account:Sales-OrderAccepted",
            "arn:aws:sns:some-region:some-account:Sales-OrderPaid"
          ]
        }
      }
    }
  ]
}
```

The policy statement is updated when an endpoint explicitly subscribes to an event type using [`session.Subscribe<CustomEvent>()`](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md). Unsubscribing does not modify the policy.

### Wildcards

#### Account condition

Allow all messages from any topic in the account. The account name is extracted from the subscribed topic ARN.

snippet: wildcard-account-condition

#### Prefix condition

Allow all messages from any topic with the specified [topic name prefix](#topicnameprefix).

snippet: wildcard-prefix-condition

#### Namespace condition

Allow all messages in specific namespaces.

snippet: wildcard-namespace-condition

### Disabling runtime policy modification

If the policy is modified during deployment it may be better to disable runtime policy modification.

snippet: assume-permissions
