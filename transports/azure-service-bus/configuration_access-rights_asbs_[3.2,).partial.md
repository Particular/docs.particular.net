### Access rights

The transport can be run without the `Manage` claim for shared access policy and without the [Azure Service Bus Data Owner](https://learn.microsoft.com/en-us/azure/role-based-access-control/built-in-roles#azure-service-bus-data-owner) role if authenticating using [Managed Identities](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-managed-service-identity).

For this the following needs to be done:

- Make sure that [installers are not configured to run](/nservicebus/operations/installers.md)
- Use [operational scripting](/transports/azure-service-bus/operational-scripting.md) to provision entities queues and topics
