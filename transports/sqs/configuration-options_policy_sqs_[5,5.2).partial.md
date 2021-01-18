## Policy

include: configuration-options-policy-intro

The transport creates one policity statement with a combined condition for all events it subscribes to. An example policy for the above two subscribed events would look like the following:

```
{
    "Version" : "2012-10-17",
    "Statement" : [
        {
            ...
            "Action"    : "sqs:SendMessage",
            "Resource"  : "arn:aws:sqs:some-region:someaccount:endpoint",
            "Condition" : {
                "ArnLike" : {
                    "aws:SourceArn" : [
                        "arn:aws:sns:some-region:someaccount:Sales-OrderAccepted",
                        "arn:aws:sns:some-region:someaccount:Sales-OrderPaid"
                    ]
                }
            }
        },
    ]
}
```

### Wildcards

#### Acount condition

Allow all messages from any topic in the account to pass. No value needs to be provided, the account name will be derived from the subscribed topic ARN.

TBD snippets

#### Prefix condition

Allow all messages from any topic with prefix to pass. No value needs to be provided, prefix from the [topic name prefix option](#topicnameprefix) will be used.

TBD snippets

#### Namespace condition

Allow all messages from any message in the specified namespaces to pass.

TBD snippets