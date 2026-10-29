---
title: Azure Blob Storage Message Body Storage
summary: Configure Azure Blob Storage for message bodies in SQL-backed ServiceControl Error instances.
reviewed: 2026-10-29
component: ServiceControl
versions: '[7,)'
---

Azure Blob Storage saves message bodies in a blob container. Set `ServiceControl/MessageBody/StorageType` to `AzureBlob` on a [SQL Server](/servicecontrol/storage/sqlserver/) or [PostgreSQL](/servicecontrol/storage/postgresql/) Error instance.

## Authentication

Azure Blob Storage supports two mutually exclusive authentication methods. Configure exactly one:

- **Connection string** — authenticates with shared key or shared access signature (SAS) credentials.
- **Service URI with Microsoft Entra credentials** — authenticates using the Azure SDK's `DefaultAzureCredential`, which can use managed identity. Set `ManagedIdentityClientId` to select a user-assigned managed identity.

## Required settings

include: sc-error-messagebody-azure-connectionstring

include: sc-error-messagebody-azure-serviceuri

`ServiceUri` must be an absolute URI.

## Optional settings

include: sc-error-messagebody-azure-containername

> [!WARNING]
> Audit instances with different retention periods must use separate containers, even when they share an Azure Storage account. Audit instances use the same default container name, so explicitly configure a different `ServiceControl.Audit/MessageBody/Azure/ContainerName` for each instance. Retention cleanup deletes blobs by time prefix across the container. An instance with shorter retention can delete bodies that another instance still needs, leaving message records visible but their bodies unavailable.

include: sc-error-messagebody-azure-managedidentityclientid

include: sc-error-messagebody-azure-authorityhost

## Setup and runtime

Setup creates the configured container if it does not exist. The identity used for setup needs permission to create the container, and the runtime identity needs read, write, and delete permissions on blobs in the container:

- **Service URI with Microsoft Entra ID:** assign the `Storage Blob Data Contributor` role on the storage account, or on the container if it is created in advance. This role covers both setup and normal operation.
- **Connection string:** the account key, or the shared access signature's permissions, must allow creating the container during setup, and reading, writing, and deleting blobs at runtime. Setup always sends a create request, even when the container already exists, so a SAS scoped to a single container cannot be used for setup; use the account key or an account SAS that allows creating containers. The retention sweeper deletes body blobs before removing the corresponding database records.

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
