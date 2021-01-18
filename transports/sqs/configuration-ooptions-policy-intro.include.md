NServiceBus automatically subscribes to all event types an endpoint has handlers for. So given an endpoint with the following two handlers:

```
public class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
public class OrderPaidHandler : IHandleMessages<OrderPaid>
```

When starting the endpoint the [auto-subscribe mechanism](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md#automatic-subscriptions) will make sure the necessary SNS topics for the events are created and all subscriptions are set up to receive the events published to the topics. AWS offers very fine-grained possibilities to control access management to services and resources by using [IAM policies](https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html). 