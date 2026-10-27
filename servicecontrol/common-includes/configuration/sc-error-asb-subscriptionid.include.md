### LicensingComponent/ASB/SubscriptionId

Version: 5.4.0+

The Azure [subscription ID](https://learn.microsoft.com/en-us/azure/azure-portal/get-subscription-tenant-id#find-your-azure-subscription) that contains the Azure Service Bus namespace.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_ASB_SUBSCRIPTIONID` |
| **App config key** | `LicensingComponent/ASB/SubscriptionId` |

| Type | Default value |
| --- | --- |
| string | the first subscription the identity can access |

Setting this is recommended. If it is not set and the namespace is in a different subscription from the first one the identity can access, ServiceControl will report that it cannot find the namespace.
