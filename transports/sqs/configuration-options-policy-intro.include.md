INFO: When an endpoint is starting the [auto-subscribe mechanism](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md#automatic-subscriptions) ensures the necessary SNS topics for the events are created and all subscriptions are set up to receive the events published to the topics. AWS [IAM policies](https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html) offer very fine-grained control of access to services and resources.

NServiceBus automatically subscribes to all event types an endpoint has handlers for. For example, an endpoint may have two handlers:

```c#
public class OrderAcceptedHandler : IHandleMessages<OrderAccepted> { ... }
public class OrderPaidHandler : IHandleMessages<OrderPaid> { ... }
```