---
title: Audit Instance Configuration Settings
summary: Categorized list of ServiceControl Audit instance configuration settings.
component: ServiceControl
reviewed: 2026-10-27
redirects:
 - servicecontrol/audit-instances/creating-config-file
---

ServiceControl Audit instance can be configured using environment variables and/or the `ServiceControl.Audit.exe.config` file. Configuration settings provided using environment variables take precedence over the application configuration file entries.

When used, the ServiceControl Management utility (SCMU) enables controlling a subset of configuration settings that are read from and written to the application configuration file.

>[!NOTE]
>Configuration settings in the application configuration file are applicable to the `appSettings` section unless otherwise specified.

## Locating the configuration file using SCMU

![image](https://github.com/Particular/docs.particular.net/assets/88632084/c9b160ba-03a5-4c73-9812-c942af6657da)

## Host settings

The following documents should be reviewed prior to modifying configuration settings:

* [Setting a Custom Hostname](/servicecontrol/setting-custom-hostname.md) for guidance and details.
* [Securing ServiceControl](/servicecontrol/securing-servicecontrol.md) for an overview of the security implications of changing the configuration.

> [!WARNING]
> Changing the host name or port number of an existing ServiceControl Audit instance will break the link from the ServiceControl Error instance. See [Moving a remote instance](/servicecontrol/servicecontrol-instances/remotes.md) for guidelines on changing these settings.


include: sc-audit-instancename


include: sc-audit-hostname


include: sc-audit-port


include: sc-audit-virtualdirectory


include: sc-audit-shutdowntimeout


## [Authentication](/servicecontrol/security/configuration/authentication.md)

These settings configure [authentication using OAuth 2.0 and OpenID Connect](/servicecontrol/security/). Refer to the [hosting and security guide](/servicecontrol/security/hosting-guide.md) or [authentication configuration examples](/servicecontrol/security/configuration/authentication.md#identity-provider-setup-configuration-examples) for additional information.


include: sc-audit-authenticationenabled


include: sc-audit-authenticationauthority


include: sc-audit-authenticationaudience


include: sc-audit-authenticationvalidateissuer


include: sc-audit-authenticationvalidateaudience


include: sc-audit-authenticationvalidatelifetime


include: sc-audit-authenticationvalidateissuersigningkey


include: sc-audit-authenticationrequirehttpsmetadata


include: sc-audit-authenticationrolebasedauthorizationenabled


include: sc-audit-authenticationrolesclaim


include: sc-audit-authenticationsubjectidclaim


include: sc-audit-authenticationsubjectnameclaim

## [TLS](/servicecontrol/security/configuration/tls.md)

These settings configure HTTPS. Refer to the [hosting and security guide](/servicecontrol/security/hosting-guide.md) or [TLS configuration examples](/servicecontrol/security/configuration/tls.md#configuration-examples) for additional information.


include: sc-audit-httpsenabled


include: sc-audit-httpscertificatepath


include: sc-audit-httpscertificatepassword


include: sc-audit-httpsredirecthttptohttps


include: sc-audit-httpsport


include: sc-audit-httpsenablehsts


include: sc-audit-httpshstsmaxageseconds


include: sc-audit-httpshstsincludesubdomains

## [Forwarded headers](/servicecontrol/security/configuration/forward-headers.md)

These settings configure forwarded headers for reverse proxy scenarios. Refer to the [hosting and security guide](/servicecontrol/security/hosting-guide.md), or [forward headers configuration examples](/servicecontrol/security/configuration/forward-headers.md#configuration-examples) for additional information.


include: sc-audit-forwardedheadersenabled


include: sc-audit-forwardedheaderstrustallproxies


include: sc-audit-forwardedheadersknownproxies


include: sc-audit-forwardedheadersknownnetworks

## [CORS](/servicecontrol/security/configuration/cors.md)

These settings configure Cross-Origin Resource Sharing (CORS). Refer to the [hosting and security guide](/servicecontrol/security/hosting-guide.md) or [cors configuration examples](/servicecontrol/security/configuration/cors.md#configuration-examples) for additional information.


include: sc-audit-corsallowanyorigin


include: sc-audit-corsallowedorigins


## Storage

Each Audit instance stores its data in RavenDB, SQL Server, or PostgreSQL. See the [storage overview](/servicecontrol/storage/) to choose a storage type. The settings for SQL Server and PostgreSQL are in [SQL Server and PostgreSQL storage](#sql-server-and-postgresql-storage) and [Message body storage](#message-body-storage). The settings for RavenDB are in [RavenDB storage](#ravendb-storage).


include: sc-audit-storagetype

## SQL Server and PostgreSQL storage

SQL Server and PostgreSQL storage are available from ServiceControl 7. These settings apply when [`ServiceControl.Audit/StorageType`](#storage-servicecontrol-auditstoragetype) is `SQLServer` or `PostgreSQL`. Prepare the database first; see [SQL Server storage](/servicecontrol/storage/sqlserver/) or [PostgreSQL storage](/servicecontrol/storage/postgresql/). SQL Server must have Full-Text Search installed. These storage types also need the [message body storage](#message-body-storage) settings.


include: sc-audit-database-connectionstring


include: sc-audit-database-schema


include: sc-audit-database-commandtimeout

## Message body storage

SQL Server and PostgreSQL storage keep message bodies that are not text, or larger than [`ServiceControl.Audit/MaxBodySizeToStore`](#performance-tuning-servicecontrol-auditmaxbodysizetostore), in an [external message body store](/servicecontrol/storage/external-body/): the file system, Azure Blob Storage, or Amazon S3. RavenDB storage does not use these settings.

The ServiceControl Management Utility configures file system storage, in the **MESSAGE BODY PATH**. The PowerShell module also supports Azure Blob Storage and Amazon S3. To change the store later, change these settings in the instance's configuration file, starting with [`ServiceControl.Audit/MessageBody/StorageType`](#message-body-storage-servicecontrol-auditmessagebodystoragetype). Changing the store does not move existing message bodies.


include: sc-audit-messagebody-storagetype


include: sc-audit-messagebody-filesystem-storagepath


include: sc-audit-messagebody-filesystem-dataspaceremainingthreshold


include: sc-audit-messagebody-azure-connectionstring


include: sc-audit-messagebody-azure-serviceuri


include: sc-audit-messagebody-azure-managedidentityclientid


include: sc-audit-messagebody-azure-authorityhost


include: sc-audit-messagebody-azure-containername


include: sc-audit-messagebody-s3-bucketname


include: sc-audit-messagebody-s3-keyprefix


include: sc-audit-messagebody-s3-region


include: sc-audit-messagebody-s3-serviceurl


include: sc-audit-messagebody-s3-accesskeyid


include: sc-audit-messagebody-s3-secretaccesskey


include: sc-audit-messagebody-mincompressionsize

## RavenDB storage

These settings apply only when [`ServiceControl.Audit/StorageType`](#storage-servicecontrol-auditstoragetype) is `RavenDB`, and to versions before 7, which always use RavenDB. `DbPath` and `RavenDBLogLevel` configure the embedded database and are not valid for instances hosted in a container.


include: sc-audit-dbpath


include: sc-audit-databasemaintenanceport


include: sc-audit-maintenancemode


include: sc-audit-ravendbloglevel


include: sc-audit-expirationprocesstimerinseconds


include: sc-audit-enablefulltextsearchonbodies


include: sc-audit-bulkinsertcommittimeoutinseconds


include: sc-audit-dataspaceremainingthreshold


include: sc-audit-minimumstorageleftrequiredforingestion

## Logging


include: sc-audit-logpath


include: sc-audit-loglevel

## Recoverability


include: sc-audit-timetorestartauditingestionafterfailure


include: sc-audit-ingestauditmessages


include: sc-audit-servicecontrol-ingestauditmessages

## Data retention


include: sc-audit-auditretentionperiod

> [!NOTE]
> Starting with version 4.26.0, new audit instances using RavenDB 5 will use the built-in RavenDB expiration process. Changing the audit retention setting will affect only newly ingested messages. Already ingested messages will expire according to the previous retention setting value.

## Performance tuning


include: sc-audit-maxbodysizetostore


include: sc-audit-maximumconcurrencylevel


include: sc-audit-auditingestionbatchsize


include: sc-audit-auditingestionmaxparallelwriters


include: sc-audit-auditingestionbatchtimeout


include: sc-audit-querytimeoutinseconds

## Transport


include: sc-audit-transporttype


include: sc-audit-transport


include: sc-audit-auditqueue


include: sc-audit-forwardauditmessages


include: sc-audit-auditlogqueue


include: sc-audit-servicecontrolqueueaddress
