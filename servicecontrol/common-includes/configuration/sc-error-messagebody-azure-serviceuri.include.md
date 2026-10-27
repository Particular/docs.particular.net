#if-version [7,)
### ServiceControl/MessageBody/Azure/ServiceUri

The Azure Blob Storage service URI used with managed identity when `StorageType` is `AzureBlob`. Configure either this setting or `ServiceControl/MessageBody/Azure/ConnectionString`, but not both.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MESSAGEBODY_AZURE_SERVICEURI` |
| **App config key** | `ServiceControl/MessageBody/Azure/ServiceUri` |
| **SCMU field** | `BLOB SERVICE URI` |

| Type | Default value |
| --- | --- |
| string | None |
#end-if
