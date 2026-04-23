ServiceControl is the backend for ServicePulse. It is a background process that collects useful information about an NServiceBus system. This includes:

- Messages that cannot be processed, along with their exceptions
- [Every message](/nservicebus/operations/auditing.md) flowing through the system
- [Saga](/nservicebus/sagas/saga-audit.md) state changes
- Endpoint [heartbeats](/monitoring/heartbeats/)
- [Detailed performance metrics](/monitoring/metrics/)

ServiceControl can also be used to perform [custom checks](/monitoring/custom-checks/). All this information is exposed to [ServicePulse](/servicepulse) via an HTTP API.
