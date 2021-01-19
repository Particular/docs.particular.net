## Policy

include: configuration-options-policy-intro

The transport creates a policy statement with per events it subscribes to. An example policy for the above two subscribed events would look like the following:

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