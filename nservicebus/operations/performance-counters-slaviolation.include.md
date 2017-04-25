## SLA violation countdown

**Counter Name:** `SLA violation countdown`

**Added in:** Version 3

Acts as an early warning system to inform on the number of seconds left until the SLA for the endpoint is breached. The counter display the number of seconds that is left between to breach configured SLA value for given endpoint. The higher the value the less risk there is to breach SLA for given endpoint. 

### Configuration

This counter can be enabled using the the following code:

snippet: enable-sla

See also [Performance Counters in the NServiceBus Host](/nservicebus/hosting/nservicebus-host/#performance-counters).