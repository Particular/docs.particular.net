### ServiceControl.Audit/MessageBody/Azure/ContainerName

_Added in version 7.0.0_

The Azure Blob Storage container used to store message bodies. Every Audit instance has the same default, so set a different container name for each Audit instance that uses the same storage account. The exception is Audit instances that [share a database](/servicecontrol/audit-instances/scaling-out-ingestion.md#sql-storage-shared-database), which must all use the same container.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MESSAGEBODY_AZURE_CONTAINERNAME` |
| **App config key** | `ServiceControl.Audit/MessageBody/Azure/ContainerName` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `audit-bodies` |
