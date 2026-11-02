#if-version [7,)
### ServiceControl.Audit/MessageBody/Azure/ServiceUri

The Azure Blob Storage service URI used with managed identity when `StorageType` is `AzureBlob`. Configure either this setting or `ServiceControl.Audit/MessageBody/Azure/ConnectionString`, but not both.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MESSAGEBODY_AZURE_SERVICEURI` |
| **App config key** | `ServiceControl.Audit/MessageBody/Azure/ServiceUri` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |
#end-if
