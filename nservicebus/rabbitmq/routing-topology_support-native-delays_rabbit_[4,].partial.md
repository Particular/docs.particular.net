### Delayed delivery

Starting with Version 4.**TBD**, it is possible to support native [delayed delivery](delayed-delivery.md) by implementing `ISupportNativeDelays` in addition to `IRoutingTopology` in the routing topology class.

When the routing topology implements `ISupportNativeDelays`, the transport will call the `BindToDelayInfrastructure` method to allow the routing topology to create the appropriate bindings to the delivery exchange that is part of the native delayed delivery infrastructure. 

`IRoutingTopology` and `ISupportNativeDelays` will be combined into a single interface in Version 5.0.
