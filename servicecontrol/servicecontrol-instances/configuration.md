---
title: Error Instance Configuration Settings
summary: Categorized list of ServiceControl Error instance configuration settings.
component: ServiceControl
reviewed: 2026-10-09
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


include: sc-error-disableexternalintegrationspublishing


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

RavenDB is the default ServiceControl storage type. ServiceControl 7 and later can also use SQL Server or PostgreSQL storage.


include: sc-error-storage-sql-intro


include: sc-error-persistencetype


include: sc-error-database-connectionstring


include: sc-error-database-schema


include: sc-error-database-commandtimeout


include: sc-error-storage-sql-interstitial


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


The following settings apply only when using RavenDB storage.


include: sc-error-databasemaintenanceport


include: sc-error-maintenancemode


include: sc-error-dataspaceremainingthreshold


include: sc-error-minimumstorageleftrequiredforingestion

The following embedded RavenDB settings are not valid for ServiceControl instances hosted in a container.


include: sc-error-dbpath


include: sc-error-ravendbloglevel


include: sc-error-indexstoragepath


include: sc-error-esent-logspath


## Logging


include: sc-error-logpath


include: sc-error-loglevel

## Recoverability


include: sc-error-timetorestarterroringestionafterfailure


include: sc-error-ingesterrormessages


include: sc-error-allowmessageediting

## Data retention


include: sc-error-expirationprocesstimerinseconds


include: sc-error-expirationprocessbatchsize


include: sc-error-errorretentionperiod


include: sc-error-eventretentionperiod


include: sc-error-trackinstancesinitialvalue

## Performance tuning


include: sc-error-maximumconcurrencylevel


include: sc-error-enablefulltextsearchonbodies


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
