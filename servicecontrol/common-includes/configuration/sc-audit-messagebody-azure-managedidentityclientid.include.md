#if-version [7,)
### ServiceControl.Audit/MessageBody/Azure/ManagedIdentityClientId

The client ID of the user-assigned managed identity used to access Azure Blob Storage. When this setting is not specified, the default Azure credential chain is used.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MESSAGEBODY_AZURE_MANAGEDIDENTITYCLIENTID` |
| **App config key** | `ServiceControl.Audit/MessageBody/Azure/ManagedIdentityClientId` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |
#end-if
