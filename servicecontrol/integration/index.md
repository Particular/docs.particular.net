---
title: ServiceControl Integration
reviewed: 2017-07-10
component: ServiceControl
---

[ServiceControl](/servicecontrol) gathers information from endpoints and exposes that information to [ServicePulse](/servicepulse) and [ServiceInsight](/serviceinsight). 

To enable ServiceControl to gather this information, configure the solution appropriately:

 * [enable auditing](/nservicebus/operations/auditing.md) to collect data on individual messages;
 * configure [recoverability](/nservicebus/recoverability) to store information on messages failures;
 * [install plugins on the endpoints](/servicecontrol/plugins/) to monitor their health and sagas and use custom checks.

NOTE: All endpoints in a single environment should be configured to forward to the same audit, error, and ServiceControl plugin queues.

ServiceControl will detect important system occurrances and raise [integration events](/servicecontrol/contracts.md) to notify subscribing endpoints about them.

The [ServiceControl Transport Adapter](/servicecontrol/transport-adapter/) decouples ServiceControl from the specifics of the business endpoint's transport to support scenarios where the endpoint's transport uses physical routing features [not compatible with ServiceControl](/servicecontrol/transport-adapter/incompatible-features.md) or where endpoints use mixed transports or multiple instances of a message broker.