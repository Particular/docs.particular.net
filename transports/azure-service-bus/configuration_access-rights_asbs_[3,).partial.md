### Access rights

The transport can be run without `Manage` rights when using a [shared access policy](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-sas) or without the [Azure Service Bus Data Owner](https://learn.microsoft.com/en-us/azure/role-based-access-control/built-in-roles#azure-service-bus-data-owner) role if authenticating using [Managed Identities](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-managed-service-identity).

To run without manage rights:

- Make sure that [installers are not configured to run](/nservicebus/operations/installers.md)
- Use [operational scripting](/transports/azure-service-bus/operational-scripting.md) to provision provision queues, topics and subscriptions
