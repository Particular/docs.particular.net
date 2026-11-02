#if-version [7,)
### ServiceControl.Audit/MessageBody/Azure/ContainerName

The Azure Blob Storage container used to store message bodies. Every Audit instance has the same default, so set a different container name for each Audit instance that uses the same storage account.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MESSAGEBODY_AZURE_CONTAINERNAME` |
| **App config key** | `ServiceControl.Audit/MessageBody/Azure/ContainerName` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `audit-bodies` |
#end-if
