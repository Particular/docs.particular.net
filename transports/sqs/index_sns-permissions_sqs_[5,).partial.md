#### [SNS permissions](https://docs.aws.amazon.com/sns/latest/dg/sns-access-policy-language-api-permissions-reference.html)

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