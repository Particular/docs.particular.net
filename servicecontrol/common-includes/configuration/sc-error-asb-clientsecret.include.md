### LicensingComponent/ASB/ClientSecret

Version: 5.4.0+

The [client secret](https://learn.microsoft.com/en-us/entra/identity-platform/howto-create-service-principal-portal#option-3-create-a-new-client-secret) for an Azure service principal that has access to read metrics data for the Azure Service Bus namespace.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_ASB_CLIENTSECRET` |
| **App config key** | `LicensingComponent/ASB/ClientSecret` |

| Type | Required |
| --- | --- |
| string | yes |

> [!NOTE]
> Certificates and federated credentials are not supported at this time.
