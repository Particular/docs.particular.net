### LicensingComponent/ASB/ClientId

Version: 5.4.0+

The Client ID (aka Application ID) for an [Azure service principal](https://learn.microsoft.com/en-us/entra/identity-platform/app-objects-and-service-principals?tabs=browser#service-principal-object) that has access to read metrics data for the Azure Service Bus namespace.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_ASB_CLIENTID` |
| **App config key** | `LicensingComponent/ASB/ClientId` |

| Type | Required |
| --- | --- |
| string | yes, unless the transport connection string [uses Microsoft Entra ID authentication](/servicecontrol/transports.md#azure-service-bus-enabling-managed-identity) |

Example Client ID from an Azure App Registration:
![Screenshot showing where the Client ID appears in an App Registration](/servicecontrol/asb-app-service-principal.png)
