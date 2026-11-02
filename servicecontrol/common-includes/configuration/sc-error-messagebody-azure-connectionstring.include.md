### ServiceControl/MessageBody/Azure/ConnectionString

_Added in version 7.0.0_

The Azure Storage connection string used to authenticate with shared key or shared access signature (SAS) credentials when `MessageBody/StorageType` is `AzureBlob`. Configure either this setting or `ServiceControl/MessageBody/Azure/ServiceUri`, but not both.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MESSAGEBODY_AZURE_CONNECTIONSTRING` |
| **App config key** | `ServiceControl/MessageBody/Azure/ConnectionString` |
| **SCMU field** | `AZURE STORAGE CONNECTION STRING` |

| Type | Default value |
| --- | --- |
| string | None |
