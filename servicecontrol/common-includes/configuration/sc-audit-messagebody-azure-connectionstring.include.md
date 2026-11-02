#if-version [7,)
### ServiceControl.Audit/MessageBody/Azure/ConnectionString

The Azure Storage connection string used to authenticate with shared key or shared access signature (SAS) credentials when `MessageBody/StorageType` is `AzureBlob`. Configure either this setting or `ServiceControl.Audit/MessageBody/Azure/ServiceUri`, but not both.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MESSAGEBODY_AZURE_CONNECTIONSTRING` |
| **App config key** | `ServiceControl.Audit/MessageBody/Azure/ConnectionString` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |
#end-if
