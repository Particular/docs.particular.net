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