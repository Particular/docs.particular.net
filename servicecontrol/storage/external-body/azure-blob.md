---
title: Azure Blob Storage Message Body Storage
summary: Configure Azure Blob Storage for message bodies in SQL-backed ServiceControl Error instances.
reviewed: 2026-06-01
component: ServiceControl
versions: '[7,)'
---

Azure Blob Storage saves message bodies in a blob container. Set `ServiceControl/MessageBody/StorageType` to `AzureBlob` on a [SQL Server](/servicecontrol/storage/sqlserver/) or [PostgreSQL](/servicecontrol/storage/postgresql/) Error instance.

## Authentication

Azure Blob Storage supports two mutually exclusive authentication methods. Configure exactly one:

- **Connection string** — authenticates with shared key or shared access signature (SAS) credentials.
- **Service URI with Microsoft Entra credentials** — authenticates using the Azure SDK's `DefaultAzureCredential`, which can use managed identity. Set `ManagedIdentityClientId` to select a user-assigned managed identity.

## Required settings

| App config key | Environment variable | Default | Required |
| --- | --- | --- | --- |
| `ServiceControl/MessageBody/Azure/ConnectionString` | `SERVICECONTROL_MESSAGEBODY_AZURE_CONNECTIONSTRING` | None | One of the two auth methods |
| `ServiceControl/MessageBody/Azure/ServiceUri` | `SERVICECONTROL_MESSAGEBODY_AZURE_SERVICEURI` | None | One of the two auth methods |

`ServiceUri` must be an absolute URI.

## Optional settings

| App config key | Environment variable | Default | Notes |
| --- | --- | --- | --- |
| `ServiceControl/MessageBody/Azure/ContainerName` | `SERVICECONTROL_MESSAGEBODY_AZURE_CONTAINERNAME` | `error-bodies` | The blob container used for message bodies. |
| `ServiceControl/MessageBody/Azure/ManagedIdentityClientId` | `SERVICECONTROL_MESSAGEBODY_AZURE_MANAGEDIDENTITYCLIENTID` | None | Client ID of a user-assigned managed identity. When not set, the default Azure credential chain is used. |
| `ServiceControl/MessageBody/Azure/AuthorityHost` | `SERVICECONTROL_MESSAGEBODY_AZURE_AUTHORITYHOST` | None | Azure authority host URI for sovereign clouds. When not set, the SDK uses `AZURE_AUTHORITY_HOST` or its default authority host. |

## Setup and runtime

Setup creates the configured container if it does not exist. The identity used for setup needs permission to create the container. The runtime identity needs read, write, and delete permissions on blobs in the container. The retention sweeper deletes body blobs before removing the corresponding database records.

Set these variables in the instance's environment to use a connection string. Replace the account and credential placeholders with values supplied by the deployment environment:

```text
SERVICECONTROL_MESSAGEBODY_STORAGETYPE=AzureBlob
SERVICECONTROL_MESSAGEBODY_AZURE_CONNECTIONSTRING="DefaultEndpointsProtocol=https;AccountName=storageaccount;AccountKey=..."
```

Example environment variables using a service URI with managed identity:

```text
SERVICECONTROL_MESSAGEBODY_STORAGETYPE=AzureBlob
SERVICECONTROL_MESSAGEBODY_AZURE_SERVICEURI=https://storageaccount.blob.core.windows.net
```

See the [external message body storage overview](/servicecontrol/storage/external-body/) for shared configuration, retention, and backup guidance.
