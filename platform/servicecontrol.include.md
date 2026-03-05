ServiceControl is the backend for ServicePulse. It is a background process that collects useful information about an NServiceBus system. This includes, where individually configured for an endpoint:

- messages that cannot be processed, along with their exceptions
- [every message](/nservicebus/operations/auditing.md) flowing through the system
- [saga](/nservicebus/sagas/) state changes
- endpoint [heartbeats](/monitoring/heartbeats/)
- [detailed performance metrics](/monitoring/metrics/)

ServiceControl can also be used to perform [custom checks](/monitoring/custom-checks/). All this information is exposed to [ServicePulse](/servicepulse) via an HTTP API.