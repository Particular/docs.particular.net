## Running the sample

1. Start first the `NativeSubscriberA` and `NativeSubscriberB` projects. This will create all the necessary publish/subscribe infrastructure in Azure Service Bus, including the event topics.
2. In the `Publisher` window, press any key to publish an event.
    * The endpoint in the `NativeSubscriberA` window will receive `EventOne`.
    * The endpoint in the `NativeSubscriberB` window will receive both `EventOne` and `EventTwo`.

## Setting up namespace entities

Each subscriber requires topic subscription(s) to receive the events published by the `Publisher`. The subscriptions are created on the `EventOne` or `EventTwo` topic, which follow the the default topic naming convention used by NServiceBus endpoints.

snippet: SubscriptionCreation

### Subscription filters

Subscriptions created by `NativeSubscriberA` and `NativeSubscriberB`. `NativeSubscriberA` subscribes to the `EventOne` events by adding a subscription with the default [`TrueFilter`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.messaging.truefilter?view=azure-dotnet) and forwarding to its input queue:

snippet: EventOneSubscription

The other subscriber creates two subscriptions following the same subscription pattern under both the `EventOne` and `EventTwo` topics.
