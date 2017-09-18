## Performance Counters

### SLA violation countdown

In the NServiceBus Host the `SLA violation countdown` counter is enabled by default. But the value can be configured either by the above API or using a `EndpointSLAAttribute` on the instance of `IConfigureThisEndpoint`.

snippet: enable-sla-host-attribute