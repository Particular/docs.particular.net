## SLA violation countdown

**Counter Name:** `SLA violation countdown`

**Added in:** Version 3

Acts as a early warning system to inform on the number of seconds left until the SLA for the endpoint is breached. This gives a system-wide counter that can be monitored without putting the SLA into the monitoring software. Just set that alarm to trigger when the counter goes below X, which is the time that the operations team needs to be able to take actions to prevent the SLA from being breached.


### Configuration

This counter can be enabled using the the following code:

snippet: enable-sla

See also [Performance Counters in the NServiceBus Host](/nservicebus/hosting/nservicebus-host/#performance-counters).