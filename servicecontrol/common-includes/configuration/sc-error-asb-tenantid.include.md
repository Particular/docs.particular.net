### LicensingComponent/ASB/TenantId

Version: 5.4.0+

The Azure [Tenant ID](https://learn.microsoft.com/en-us/azure/azure-portal/get-subscription-tenant-id#find-your-microsoft-entra-tenant).

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_ASB_TENANTID` |
| **App config key** | `LicensingComponent/ASB/TenantId` |

| Type | Required |
| --- | --- |
| string | yes, unless the transport connection string [uses Microsoft Entra ID authentication](/servicecontrol/transports.md#azure-service-bus-enabling-managed-identity) |
