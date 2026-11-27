---
title: Error Instance Configuration Settings
summary: Categorized list of ServiceControl Error instance configuration settings.
component: ServiceControl
reviewed: 2026-10-27
redirects:
 - servicecontrol/creating-config-file
---

The configuration of a ServiceControl instance can be adjusted via the ServiceControl Management utility or by directly modifying the `ServiceControl.exe.config` file. The settings listed are applicable to the `appSettings` section of the configuration file unless otherwise specified.

The configuration of a ServiceControl Error instance is controlled by the `ServiceControl.exe.config` file or by setting environment variables. When a setting configuration exists as both an environment variable and in the application configuration file the environment variable setting takes precedence.

Deployments using the ServiceControl Management utility (SCMU) can use that application to make a subset of configuration settings which are read from and written to the application configuration file.

## Locating the configuration file using SCMU

![image](https://github.com/Particular/docs.particular.net/assets/88632084/0b04d82b-6a77-427d-81f3-6e450544ff90)

## Host settings

The following documents should be reviewed prior to modifying configuration settings:

* [Setting a Custom Hostname](/servicecontrol/setting-custom-hostname.md) for guidance and details.
* [Securing ServiceControl](/servicecontrol/securing-servicecontrol.md) for an overview of the security implications of changing the configuration.


include: sc-error-instancename


include: sc-error-hostname


include: sc-error-port


include: sc-error-virtualdirectory


include: sc-error-remoteinstances


include: sc-error-shutdowntimeout


include: sc-error-enableexternalintegrationspublishing


include: sc-error-enableintegratedservicepulse

## [Authentication](/servicecontrol/security/configuration/authentication.md)

These settings configure [authentication using OAuth 2.0 and OpenID Connect](/servicecontrol/security/). Refer to the [hosting and security guide](/servicecontrol/security/hosting-guide.md) or [authentication configuration examples](/servicecontrol/security/configuration/authentication.md#identity-provider-setup-configuration-examples) for additional information.


include: sc-error-authenticationenabled


include: sc-error-authenticationauthority


include: sc-error-authenticationaudience


include: sc-error-authenticationvalidateissuer


include: sc-error-authenticationvalidateaudience


include: sc-error-authenticationvalidatelifetime


include: sc-error-authenticationvalidateissuersigningkey


include: sc-error-authenticationrequirehttpsmetadata


include: sc-error-authenticationservicepulseclientid


include: sc-error-authenticationservicepulseauthority


include: sc-error-authenticationservicepulseapiscopes


include: sc-error-authenticationservicepulseofflineaccessscopeenabled


include: sc-error-authenticationrolebasedauthorizationenabled


include: sc-error-authenticationrolesclaim


include: sc-error-authenticationsubjectidclaim


include: sc-error-authenticationsubjectnameclaim

## [TLS](/servicecontrol/security/configuration/tls.md)

These settings configure HTTPS. Refer to the [hosting and security guide](/servicecontrol/security/hosting-guide.md) or [TLS configuration examples](/servicecontrol/security/configuration/tls.md#configuration-examples) for additional information.


include: sc-error-httpsenabled


include: sc-error-httpscertificatepath


include: sc-error-httpscertificatepassword


include: sc-error-httpsredirecthttptohttps


include: sc-error-httpsport


include: sc-error-httpsenablehsts


include: sc-error-httpshstsmaxageseconds


include: sc-error-httpshstsincludesubdomains

## [Forwarded headers](/servicecontrol/security/configuration/forward-headers.md)

These settings configure forwarded headers for reverse proxy scenarios. Refer to the [hosting and security guide](/servicecontrol/security/hosting-guide.md) or [forward headers configuration examples](/servicecontrol/security/configuration/forward-headers.md#configuration-examples) for additional information.


include: sc-error-forwardedheadersenabled


include: sc-error-forwardedheaderstrustallproxies


include: sc-error-forwardedheadersknownproxies


include: sc-error-forwardedheadersknownnetworks

## [CORS](/servicecontrol/security/configuration/cors.md)

These settings configure Cross-Origin Resource Sharing (CORS). Refer to the [hosting and security guide](/servicecontrol/security/hosting-guide.md) or [cors configuration examples](/servicecontrol/security/configuration/cors.md#configuration-examples) for additional information.


include: sc-error-corsallowanyorigin


include: sc-error-corsallowedorigins

## Storage

Each Error instance stores its data in RavenDB, SQL Server, or PostgreSQL. See the [storage overview](/servicecontrol/storage/) to choose a storage type. The settings for SQL Server and PostgreSQL are in [SQL Server and PostgreSQL storage](#sql-server-and-postgresql-storage) and [Message body storage](#message-body-storage). The settings for RavenDB are in [RavenDB storage](#ravendb-storage).


include: sc-error-storagetype

## SQL Server and PostgreSQL storage

SQL Server and PostgreSQL storage are available from ServiceControl 7. These settings apply when [`ServiceControl/StorageType`](#storage-servicecontrolstoragetype) is `SQLServer` or `PostgreSQL`. Prepare the database first; see [SQL Server storage](/servicecontrol/storage/sqlserver/) or [PostgreSQL storage](/servicecontrol/storage/postgresql/). SQL Server must have Full-Text Search installed. These storage types also need the [message body storage](#message-body-storage) settings.


include: sc-error-database-connectionstring


include: sc-error-database-schema


include: sc-error-database-commandtimeout

## Message body storage

SQL Server and PostgreSQL storage keep message bodies that are binary, or larger than [`ServiceControl/MaxBodySizeToStore`](#message-body-storage-servicecontrolmaxbodysizetostore), in an [external message body store](/servicecontrol/storage/external-body/): the file system, Azure Blob Storage, or Amazon S3. RavenDB storage does not use these settings.

The ServiceControl Management Utility configures file system storage, in the **MESSAGE BODY PATH**. The PowerShell module also supports Azure Blob Storage and Amazon S3. To change the store later, change these settings in the instance's configuration file, starting with [`ServiceControl/MessageBody/StorageType`](#message-body-storage-servicecontrolmessagebodystoragetype). Changing the store does not move existing message bodies.


include: sc-error-messagebody-storagetype


include: sc-error-maxbodysizetostore


include: sc-error-messagebody-filesystem-storagepath


include: sc-error-messagebody-filesystem-dataspaceremainingthreshold


include: sc-error-messagebody-azure-connectionstring


include: sc-error-messagebody-azure-serviceuri


include: sc-error-messagebody-azure-managedidentityclientid


include: sc-error-messagebody-azure-authorityhost


include: sc-error-messagebody-azure-containername


include: sc-error-messagebody-s3-bucketname


include: sc-error-messagebody-s3-keyprefix


include: sc-error-messagebody-s3-region


include: sc-error-messagebody-s3-serviceurl


include: sc-error-messagebody-s3-accesskeyid


include: sc-error-messagebody-s3-secretaccesskey


include: sc-error-messagebody-mincompressionsize

## RavenDB storage

These settings apply only when [`ServiceControl/StorageType`](#storage-servicecontrolstoragetype) is `RavenDB`, and to versions before 7, which always use RavenDB. `DbPath` and `RavenDBLogLevel` configure the embedded database and are not valid for instances hosted in a container.


include: sc-error-dbpath


include: sc-error-databasemaintenanceport


include: sc-error-maintenancemode


include: sc-error-ravendbloglevel


include: sc-error-expirationprocesstimerinseconds


include: sc-error-enablefulltextsearchonbodies


include: sc-error-dataspaceremainingthreshold


include: sc-error-minimumstorageleftrequiredforingestion

## Logging


include: sc-error-logpath


include: sc-error-loglevel

## Recoverability


include: sc-error-timetorestarterroringestionafterfailure


include: sc-error-ingesterrormessages


include: sc-error-allowmessageediting

## Data retention


include: sc-error-errorretentionperiod


include: sc-error-eventretentionperiod


include: sc-error-trackinstancesinitialvalue

## Performance tuning


include: sc-error-maximumconcurrencylevel


include: sc-error-erroringestionbatchsize


include: sc-error-erroringestionmaxparallelwriters


include: sc-error-erroringestionbatchtimeout


include: sc-error-querytimeoutinseconds


## Transport


include: sc-error-transporttype


include: sc-error-transport


include: sc-error-errorqueue


include: sc-error-forwarderrormessages


include: sc-error-errorlogqueue

## Usage Reporting when using ServiceControl


include: sc-error-servicecontrolthroughputdataqueue

## Usage Reporting when using the Azure Service Bus transport

The following settings are part of [Usage Reporting Setup when using the Azure Service Bus transport](/servicepulse/usage-reporting-setup.md#connection-setup-azure-service-bus)

include: sc-error-asb-subscriptionid


include: sc-error-asb-servicebusname


include: sc-error-asb-tenantid


include: sc-error-asb-clientid


include: sc-error-asb-clientsecret


include: sc-error-asb-managementurl

## Usage Reporting when using the Amazon SQS transport


include: sc-error-amazonsqs-accesskey


include: sc-error-amazonsqs-secretkey


include: sc-error-amazonsqs-profile


include: sc-error-amazonsqs-region


include: sc-error-amazonsqs-prefix

## Usage Reporting when using the RabbitMQ transport

> [!NOTE]
> Begining with version 6.5.0, these configuration settings are no longer used. Access to the management API is configured using [connection string options](/servicecontrol/transports.md#rabbitmq). See the [ServiceControl 6.4 to 6.5 upgrade guide](/servicecontrol/upgrades/6.4to6.5.md) for more information.


include: sc-error-rabbitmq-apiurl


include: sc-error-rabbitmq-username


include: sc-error-rabbitmq-password

## Usage Reporting when using the SqlServer transport


include: sc-error-sqlserver-connectionstring


include: sc-error-sqlserver-additionalcatalogs

## Usage Reporting when using the PostgreSQL transport


include: sc-error-postgresql-connectionstring

## Plugin-specific


include: sc-error-heartbeatgraceperiod
