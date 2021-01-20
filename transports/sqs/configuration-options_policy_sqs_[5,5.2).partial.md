## Policy

include: configuration-options-policy-intro

The transport creates a policy statement per event it subscribes to. An example policy for the event subscriptions described here above would look as follows:

```json
{
  ...
  "Statement": [
    {
      ...
      "Action": "sqs:SendMessage",
      "Resource": "arn:aws:sqs:some-region:someaccount:endpoint",
      "Condition": {
        "ArnLike": {
          "aws:SourceArn": "arn:aws:sns:some-region:someaccount:Sales-OrderAccepted"
        }
      }
    },
    {
...
      "Action": "SQS:SendMessage",
      "Resource": "arn:aws:sqs:some-region:someaccount:endpoint",
      "Condition": {
        "ArnLike": {
          "aws:SourceArn": "arn:aws:sns:some-region:someaccount:Sales-OrderPaid"
        }
      }
    }
  ]
}
```

Policy statements are also added when an endpoint manually subscribes to an event type by calling [`session.Subscribe<CustomEvent>()`](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md). Unsubscribing does not modify the policy on the endpoint's input queue.
