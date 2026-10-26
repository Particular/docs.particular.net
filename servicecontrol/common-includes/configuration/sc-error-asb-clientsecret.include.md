### LicensingComponent/ASB/ClientSecret

Version: 5.4.0+

The [client secret](https://learn.microsoft.com/en-us/entra/identity-platform/howto-create-service-principal-portal#option-3-create-a-new-client-secret) for an Azure service principal that has access to read metrics data for the Azure Service Bus namespace.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_ASB_CLIENTSECRET` |
| **App config key** | `LicensingComponent/ASB/ClientSecret` |

| Type | Required |
| --- | --- |
| string | yes, unless the transport connection string [uses Microsoft Entra ID authentication](/servicecontrol/transports.md#azure-service-bus-enabling-managed-identity) |

> [!NOTE]
> The `ClientId` and `ClientSecret` settings support only a [service principal](https://learn.microsoft.com/en-us/entra/identity-platform/app-objects-and-service-principals?tabs=browser#service-principal-object) that authenticates with a client secret. To use a [managed identity](/servicecontrol/transports.md#azure-service-bus-enabling-managed-identity), or a service principal that authenticates with a [certificate](/servicecontrol/transports.md#azure-service-bus-enabling-certificate-authentication) or a [federated identity credential](https://learn.microsoft.com/en-us/entra/workload-id/workload-identity-federation), configure the transport to connect with it instead. ServiceControl then reads metrics with the same identity. See [usage reporting setup](/servicepulse/usage-reporting-setup.md#connection-setup-azure-service-bus).
