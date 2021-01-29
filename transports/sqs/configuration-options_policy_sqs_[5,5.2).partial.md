## Policy

include: configuration-options-policy-intro

The transport creates a policy statement for each event type it subscribes to:

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
          "aws:SourceArn": "arn:aws:sns:some-region:some-account:Sales-OrderAccepted"
        }
      }
    },
    {
      ...
      "Action": "sqs:SendMessage",
      "Resource": "arn:aws:sqs:some-region:some-account:endpoint",
      "Condition": {
        "ArnLike": {
          "aws:SourceArn": "arn:aws:sns:some-region:some-account:Sales-OrderPaid"
        }
      }
    }
  ]
}
```

A policy statement is also created when an endpoint explicitly subscribes to an event type using [`session.Subscribe<CustomEvent>()`](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md). Unsubscribing does not modify the policy.
