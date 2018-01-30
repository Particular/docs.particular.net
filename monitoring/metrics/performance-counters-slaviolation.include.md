## SLA violation countdown

**Counter Name:** `SLA violation countdown`

**Added in:** Version 3

Acts as an early warning system to inform on the number of seconds left until the SLA for the endpoint is breached. The counter shows the number of seconds left until the configured SLA value for a given endpoint is breached. The higher the value the less risk there is to breach the SLA for a given endpoint. 


### Configuration

This counter can be enabled using the the following code:

snippet: enable-sla

See also [Performance Counters in the NServiceBus Host](/nservicebus/hosting/nservicebus-host/#performance-counters).
