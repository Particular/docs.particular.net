#if-version [7,)
### ServiceControl/MessageBody/Azure/AuthorityHost

The Microsoft Entra authority host URI used for managed identity authentication in sovereign clouds. When this setting is not specified, the Azure SDK uses the `AZURE_AUTHORITY_HOST` environment variable or its default authority host.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MESSAGEBODY_AZURE_AUTHORITYHOST` |
| **App config key** | `ServiceControl/MessageBody/Azure/AuthorityHost` |
| **SCMU field** | `AUTHORITY HOST (OPTIONAL)` |

| Type | Default value |
| --- | --- |
| string | None |
#end-if
