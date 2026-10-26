#if-version [7,)
### ServiceControl/MessageBody/Azure/ManagedIdentityClientId

The client ID of the user-assigned managed identity used to access Azure Blob Storage. When this setting is not specified, the default Azure credential chain is used.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MESSAGEBODY_AZURE_MANAGEDIDENTITYCLIENTID` |
| **App config key** | `ServiceControl/MessageBody/Azure/ManagedIdentityClientId` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |
#end-if
