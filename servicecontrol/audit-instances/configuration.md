---
title: Audit Instance Configuration Settings
summary: Categorized list of ServiceControl Audit instance configuration settings.
component: ServiceControl
reviewed: 2026-06-01
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


include: sc-audit-databasemaintenanceport


include: sc-audit-virtualdirectory


include: sc-audit-shutdowntimeout


include: sc-audit-maintenancemode

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

RavenDB is the default Audit instance storage type.


include: sc-audit-storage-sql-intro


include: sc-audit-persistencetype


include: sc-audit-database-connectionstring


include: sc-audit-database-schema


include: sc-audit-database-commandtimeout


include: sc-audit-storage-sql-outro


## Embedded database

These settings apply only to RavenDB storage and are not valid for ServiceControl instances hosted in a container.


include: sc-audit-dbpath


include: sc-audit-ravendbloglevel


include: sc-audit-indexstoragepath


## Logging


include: sc-audit-logpath


include: sc-audit-loglevel

## Recoverability


include: sc-audit-timetorestartauditingestionafterfailure


include: sc-audit-ingestauditmessages


include: sc-audit-servicecontrol-ingestauditmessages

## Data retention


include: sc-audit-expirationprocesstimerinseconds


include: sc-audit-expirationprocessbatchsize


include: sc-audit-auditretentionperiod

## Performance tuning


include: sc-audit-maxbodysizetostore


include: sc-audit-maximumconcurrencylevel


include: sc-audit-auditingestionbatchsize


include: sc-audit-auditingestionmaxparallelwriters


include: sc-audit-auditingestionbatchtimeout


include: sc-audit-enablefulltextsearchonbodies


include: sc-audit-bulkinsertcommittimeoutinseconds


include: sc-audit-querytimeoutinseconds

## Transport


include: sc-audit-transporttype


include: sc-audit-transport


include: sc-audit-auditqueue


include: sc-audit-forwardauditmessages


include: sc-audit-auditlogqueue


include: sc-audit-servicecontrolqueueaddress

## Troubleshooting


include: sc-audit-dataspaceremainingthreshold


include: sc-audit-minimumstorageleftrequiredforingestion


include: sc-audit-esent-logspath
