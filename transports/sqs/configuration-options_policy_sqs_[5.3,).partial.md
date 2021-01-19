## Policy

include: configuration-options-policy-intro

The transport creates one policy statement with a combined condition for all events it subscribes to. An example policy for the above two subscribed events would look like the following:

```json
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

The combined condition is extended when an endpoint manually subscribes to an event type by calling [`session.Subscribe<CustomEvent>()`](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md). Unsubscribing does not modify the policy on the endpoint's input queue.

### Wildcards

#### Acount condition

Allow all messages from any topic in the account to pass. No value needs to be provided, the account name will be derived from the subscribed topic ARN.

snippet: wildcard-account-condition

#### Prefix condition

Allow all messages from any topic with prefix to pass. No value needs to be provided, prefix from the [topic name prefix option](#topicnameprefix) will be used.

snippet: wildcard-prefix-condition

#### Namespace condition

Allow all messages from any message in the specified namespaces to pass.

snippet: wildcard-namespace-condition

### Disable policy modifications

In cases where the policy on the endpoint queue is managed via deployment it might be desirable to opt-out from the policy modification.

snippet: assume-permissions