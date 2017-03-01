### Delayed delivery

In Versions 4.3 and above, custom routing topologies can support [delayed delivery](delayed-delivery.md) without requiring the [timeout manager](/nservicebus/messaging/timeout-manager.md) by implementing `ISupportDelayedDelivery` in addition to `IRoutingTopology` in the routing topology class.

When the routing topology implements `ISupportDelayedDelivery`, the transport will call the `BindToDelayInfrastructure` method to allow the routing topology to create the appropriate bindings to the delivery exchange that is part of the native delayed delivery infrastructure. 

`IRoutingTopology` and `ISupportDelayedDelivery` will be combined into a single interface in Version 5.0.
