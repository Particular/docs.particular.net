---
title: Audit Instance Configuration Settings
summary: Categorized list of ServiceControl Audit instance configuration settings.
component: ServiceControl
reviewed: 2024-06-24
redirects:
 - servicecontrol/audit-instances/creating-config-file
---
The configuration of a ServiceControl Audit instance is controlled by the `ServiceControl.Audit.exe.config` file or by setting environment variables. When a setting configuration exists as both an environment variable and in the application configuration file the environment variable setting takes precedence.

Deployments using the ServiceControl Management utility (SCMU) can use that application to make a subset of configuration settings which are read from and written to the application configuration file.

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

### ServiceControl.Audit/InstanceName

_Added in version 5.5.0_

The name to be used by the audit instance and the name of the input queue.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_INSTANCENAME` |
| **App config key** | `ServiceControl.Audit/InstanceName` |
| **SCMU field** | Instance/Queue Name |

| Type | Default value |
| --- | --- |
| string | `Particular.ServiceControl.Audit` |

> [!NOTE]
> In versions prior to 5.5.0, the `InternalQueueName` setting can be used instead.

### ServiceControl.Audit/HostName

The hostname to bind the embedded HTTP API server to; modify this setting to bind to a specific hostname, eg. `sc.mydomain.com` and make the machine remotely accessible.

This field can also contain a `*` as a wildcard to allow remote connections that use any hostname.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_HOSTNAME` |
| **App config key** | `ServiceControl.Audit/HostName` |
| **SCMU field** | `HOST NAME` |

| Type | Default value |
| --- | --- |
| string | `localhost` |

> [!WARNING]
> If the `ServiceControl.Audit/HostName` setting is changed, and the `ServiceControl.Audit/DbPath` setting is not set, the path of the embedded RavenDB is changed. Refer to [Customize RavenDB Embedded Location](/servicecontrol/configure-ravendb-location.md).

#if-version [5,)
> [!NOTE]
> Changing the `ServiceControl.Audit/HostName` setting does not affect how the embedded RavenDB database is exposed. The embedded RavenDB database remains accessible only via `localhost`.
#end-if

### ServiceControl.Audit/Port

The port to bind the embedded HTTP API server.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_PORT` |
| **App config key** | `ServiceControl.Audit/Port` |
| **SCMU field** | `PORT NUMBER` |

| Type | Default value |
| --- | --- |
| int | `44444` |

> [!WARNING]
> If the `ServiceControl.Audit/Port` setting is changed, and the `ServiceControl.Audit/DbPath` setting is not set, the path of the embedded RavenDB is changed. Refer to [Customize RavenDB Embedded Location](/servicecontrol/configure-ravendb-location.md).

### ServiceControl.Audit/DatabaseMaintenancePort

The port to expose the RavenDB database.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_DATABASEMAINTENANCEPORT` |
| **App config key** | `ServiceControl.Audit/DatabaseMaintenancePort` |
| **SCMU field** | `DATABASE MAINTENANCE PORT NUMBER (1 - 49151)` |

| Type | Default value |
| --- | --- |
| int | `44445` |

> [!NOTE]
> This setting is not relevant when running an audit instance in a container.

### ServiceControl.Audit/VirtualDirectory

The virtual directory to bind the embedded HTTP server to; modify this setting to bind to a specific virtual directory.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_VIRTUALDIRECTORY` |
| **App config key** | `ServiceControl.Audit/VirtualDirectory` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | _None_ |

### ServiceControl.Audit/ShutdownTimeout

_Added in version 6.5.0_

The maximum allowed time for the process to gracefully complete the shutdown after which the process will try to terminate.

> [!NOTE]
> An ungraceful shutdown could result in the next start to require a lengthy database recovery operation.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_SHUTDOWNTIMEOUT` |
| **App config key** | `ServiceControl.Audit/ShutdownTimeout` |
| **SCMU field** | N/A |

| Environment/Installation type            | Type     | Default value |
| --- | --- | --- |
| Containers | TimeSpan | `00:00:05` (5 seconds) |
| Installation via PowerShell (on Windows) | TimeSpan | `00:02:00` (2 minutes) |
| Installation via ServiceControl Management Utility (SCMU) (on Windows) | TimeSpan | `00:02:00` (2 minutes) |

### ServiceControl.Audit/MaintenanceMode

Run [ServiceControl audit instance in maintenance mode](/servicecontrol/ravendb/accessing-database.md) in order to do database maintenance.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MAINTENANCEMODE` |
| **App config key** | `ServiceControl.Audit/MaintenanceMode` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| boolean | `False` |

## [Authentication](/servicecontrol/security/configuration/authentication.md)

These settings configure [authentication using OAuth 2.0 and OpenID Connect](/servicecontrol/security/). Refer to the [hosting and security guide](/servicecontrol/security/hosting-guide.md), or [authentication configuration examples](/servicecontrol/security/configuration/authentication.md#identity-provider-setup-configuration-examples) for additional information.

### ServiceControl.Audit/Authentication.Enabled

_Added in version 6.11.0_

Enables or disables authentication. This is a **Global switch** and all other authentication settings are ignored unless this is `true`.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_AUTHENTICATION_ENABLED` |
| **App config key** | `ServiceControl.Audit/Authentication.Enabled` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `false` |

### ServiceControl.Audit/Authentication.Authority

_Added in version 6.11.0_

The URL of the OpenID Connect authority (identity provider) used to authenticate tokens.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_AUTHENTICATION_AUTHORITY` |
| **App config key** | `ServiceControl.Audit/Authentication.Authority` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |

### ServiceControl.Audit/Authentication.Audience

_Added in version 6.11.0_

The expected audience value in the JWT token, typically the application ID or URI of the API.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_AUTHENTICATION_AUDIENCE` |
| **App config key** | `ServiceControl.Audit/Authentication.Audience` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |

### ServiceControl.Audit/Authentication.ValidateIssuer

_Added in version 6.11.0_

Controls whether the token issuer is validated against the authority.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_AUTHENTICATION_VALIDATEISSUER` |
| **App config key** | `ServiceControl.Audit/Authentication.ValidateIssuer` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

### ServiceControl.Audit/Authentication.ValidateAudience

_Added in version 6.11.0_

Controls whether the token audience is validated.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_AUTHENTICATION_VALIDATEAUDIENCE` |
| **App config key** | `ServiceControl.Audit/Authentication.ValidateAudience` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

### ServiceControl.Audit/Authentication.ValidateLifetime

_Added in version 6.11.0_

Controls whether the token expiration is validated.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_AUTHENTICATION_VALIDATELIFETIME` |
| **App config key** | `ServiceControl.Audit/Authentication.ValidateLifetime` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

### ServiceControl.Audit/Authentication.ValidateIssuerSigningKey

_Added in version 6.11.0_

Controls whether the token signing key is validated.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_AUTHENTICATION_VALIDATEISSUERSIGNINGKEY` |
| **App config key** | `ServiceControl.Audit/Authentication.ValidateIssuerSigningKey` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

### ServiceControl.Audit/Authentication.RequireHttpsMetadata

_Added in version 6.11.0_

Controls whether HTTPS is required when retrieving metadata from the authority.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_AUTHENTICATION_REQUIREHTTPSMETADATA` |
| **App config key** | `ServiceControl.Audit/Authentication.RequireHttpsMetadata` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

> [!WARNING]
> Setting this to `false` is not recommended for production environments.

## [TLS](/servicecontrol/security/configuration/tls.md)

These settings configure HTTPS. Refer to the [hosting and security guide](/servicecontrol/security/hosting-guide.md), or [TLS configuration examples](/servicecontrol/security/configuration/tls.md#configuration-examples) for additional information.

### ServiceControl.Audit/Https.Enabled

_Added in version 6.11.0_

Enables Kestrel HTTPS with a certificate.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_HTTPS_ENABLED` |
| **App config key** | `ServiceControl.Audit/Https.Enabled` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `false` |

### ServiceControl.Audit/Https.CertificatePath

_Added in version 6.11.0_

The path to the PFX or PEM certificate file.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_HTTPS_CERTIFICATEPATH` |
| **App config key** | `ServiceControl.Audit/Https.CertificatePath` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |

### ServiceControl.Audit/Https.CertificatePassword

_Added in version 6.11.0_

The password for the certificate file, if required.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_HTTPS_CERTIFICATEPASSWORD` |
| **App config key** | `ServiceControl.Audit/Https.CertificatePassword` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |

### ServiceControl.Audit/Https.RedirectHttpToHttps

_Added in version 6.11.0_

Redirects HTTP requests to HTTPS. This is intended for use with a reverse proxy that handles both HTTP and HTTPS traffic.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_HTTPS_REDIRECTHTTPTOHTTPS` |
| **App config key** | `ServiceControl.Audit/Https.RedirectHttpToHttps` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `false` |

> [!NOTE]
> When running ServiceControl directly without a reverse proxy, the application only listens on a single protocol (HTTP or HTTPS). This setting is intended for use with a reverse proxy that handles both HTTP and HTTPS traffic.

### ServiceControl.Audit/Https.Port

_Added in version 6.11.0_

The HTTPS port to use in redirect URLs. Required when `RedirectHttpToHttps` is enabled in reverse proxy scenarios.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_HTTPS_PORT` |
| **App config key** | `ServiceControl.Audit/Https.Port` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | None |

### ServiceControl.Audit/Https.EnableHsts

_Added in version 6.11.0_

Enables HTTP Strict Transport Security (HSTS).

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_HTTPS_ENABLEHSTS` |
| **App config key** | `ServiceControl.Audit/Https.EnableHsts` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `false` |

> [!NOTE]
> Review the implications of [enabling HSTS](/servicecontrol/security/configuration/tls.md#security-considerations-hsts) before doing so.

### ServiceControl.Audit/Https.HstsMaxAgeSeconds

_Added in version 6.11.0_

The max-age value in seconds for the HSTS header.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_HTTPS_HSTSMAXAGESECONDS` |
| **App config key** | `ServiceControl.Audit/Https.HstsMaxAgeSeconds` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `31536000` (1 year) |

### ServiceControl.Audit/Https.HstsIncludeSubDomains

_Added in version 6.11.0_

Includes subdomains in the HSTS policy.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_HTTPS_HSTSINCLUDESUBDOMAINS` |
| **App config key** | `ServiceControl.Audit/Https.HstsIncludeSubDomains` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `false` |

## [Forwarded headers](/servicecontrol/security/configuration/forward-headers.md)

These settings configure forwarded headers for reverse proxy scenarios. Refer to the [hosting and security guide](/servicecontrol/security/hosting-guide.md), or [forward headers configuration examples](/servicecontrol/security/configuration/forward-headers.md#configuration-examples) for additional information.

### ServiceControl.Audit/ForwardedHeaders.Enabled

_Added in version 6.11.0_

Enables processing of forwarded headers (X-Forwarded-For, X-Forwarded-Proto, etc.).

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_FORWARDEDHEADERS_ENABLED` |
| **App config key** | `ServiceControl.Audit/ForwardedHeaders.Enabled` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

### ServiceControl.Audit/ForwardedHeaders.TrustAllProxies

_Added in version 6.11.0_

Trusts forwarded headers from any source. Set to `false` when using `KnownProxies` or `KnownNetworks`.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_FORWARDEDHEADERS_TRUSTALLPROXIES` |
| **App config key** | `ServiceControl.Audit/ForwardedHeaders.TrustAllProxies` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

> [!WARNING]
> For production environments behind a reverse proxy, set this to `false` and configure `KnownProxies` or `KnownNetworks` to restrict which proxies are trusted.

### ServiceControl.Audit/ForwardedHeaders.KnownProxies

_Added in version 6.11.0_

A comma-separated list of trusted proxy IP addresses e.g., `127.0.0.1`

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_FORWARDEDHEADERS_KNOWNPROXIES` |
| **App config key** | `ServiceControl.Audit/ForwardedHeaders.KnownProxies` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |

### ServiceControl.Audit/ForwardedHeaders.KnownNetworks

_Added in version 6.11.0_

A comma-separated list of trusted CIDR network ranges e.g., `10.0.0.0/8,172.16.0.0/12`

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_FORWARDEDHEADERS_KNOWNNETWORKS` |
| **App config key** | `ServiceControl.Audit/ForwardedHeaders.KnownNetworks` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |

## [CORS](/servicecontrol/security/configuration/cors.md)

These settings configure Cross-Origin Resource Sharing (CORS). Refer to the [hosting and security guide](/servicecontrol/security/hosting-guide.md), or [cors configuration examples](/servicecontrol/security/configuration/cors.md#configuration-examples) for additional information.

### ServiceControl.Audit/Cors.AllowAnyOrigin

_Added in version 6.11.0_

Allows requests from any origin.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_CORS_ALLOWANYORIGIN` |
| **App config key** | `ServiceControl.Audit/Cors.AllowAnyOrigin` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

> [!WARNING]
> For production environments, set this to `false` and configure `AllowedOrigins` to restrict which origins can access the API.

### ServiceControl.Audit/Cors.AllowedOrigins

_Added in version 6.11.0_

A comma-separated list of allowed origins e.g., `https://servicepulse.example.com,https://admin.example.com`

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_CORS_ALLOWEDORIGINS` |
| **App config key** | `ServiceControl.Audit/Cors.AllowedOrigins` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |

## Embedded database

These settings are not valid for ServiceControl instances hosted in a container.

### ServiceControl.Audit/DbPath

The path where the internal RavenDB is located.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_DBPATH` |
| **App config key** | `ServiceControl.Audit/DbPath` |
| **SCMU field** | `DATABASE PATH` |

| Type | Default value |
| --- | --- | --- | --- |
| string | `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<instance_name>\DB` |

> [!NOTE]
> This setting is not relevant when the audit instance is [deployed using a container](/servicecontrol/audit-instances/deployment/containers.md).

### ServiceControl.Audit/RavenDBLogLevel

Controls the LogLevel of the RavenDB logs.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_RAVENDBLOGLEVEL` |
| **App config key** | `ServiceControl.Audit/RavenDBLogLevel` |
| **SCMU field** | N/A |

#if-version [5,)
| Type | Default value |
| --- | --- |
| string | `Operations` |

Valid settings are: `None`, `Information`, `Operations`.
#end-if
#if-version [,5)
| Type | Default value |
| --- | --- |
| string | `Warn` |

Valid settings are: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.
#end-if

#if-version [,5)
### Raven/IndexStoragePath
> [!NOTE]
> Only supported on the RavenDB 3.5 storage engine. Use [symbolic links (soft links) to map any RavenDB storage subfolder](https://ravendb.net/docs/article-page/5.4/csharp/server/storage/customizing-raven-data-files-locations) to other physical drives.

The path for the indexes on disk.

| Context | Name |
| --- | --- |
| **Environment variable** | `RAVEN_INDEXSTORAGEPATH` |
| **App config key** | `Raven/IndexStoragePath` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<instance_name>\DB\indexes` |

> [!NOTE]
> This setting is not relevant when running an audit instance in a container.

#end-if

## Logging

### ServiceControl.Audit/LogPath

The path for the ServiceControl logs.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_LOGPATH` |
| **App config key** | `ServiceControl.Audit/LogPath` |
| **SCMU field** | `LOG PATH` |

| Type | Default value |
| --- | --- |
| string | `%LOCALAPPDATA%\Particular\ServiceControl.Audit\logs` |

> [!NOTE]
> %LOCALAPPDATA% is a user-specific path on Windows.
>
> When hosted on containers, logs are sent to **stdout** and this setting is ignored.

### ServiceControl.Audit/LogLevel

Controls the LogLevel of the ServiceControl logs.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_LOGLEVEL` |
| **App config key** | `ServiceControl.Audit/LogLevel` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `Info` |

Valid settings are: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.

## Recoverability

### ServiceControl.Audit/TimeToRestartAuditIngestionAfterFailure

Controls the maximum time delay to wait before restarting the audit ingestion pipeline after detecting a connection problem.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_TIMETORESTARTAUDITINGESTIONAFTERFAILURE` |
| **App config key** | `ServiceControl.Audit/TimeToRestartAuditIngestionAfterFailure` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| timespan | 60 seconds |

Valid settings are between 5 seconds and 1 hour.

### ServiceControl.Audit/IngestAuditMessages

Set to `false` to disable ingesting new audit messages. Useful in some upgrade scenarios.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_INGESTAUDITMESSAGES` |
| **App config key** | `ServiceControl.Audit/IngestAuditMessages` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

### ServiceControl/IngestAuditMessages

> [!WARNING]
> This is the same setting as `ServiceControl.Audit/IngestAuditMessages` but kept for backward compatibility

Set to `false` to disable ingesting new audit messages. Useful in some upgrade scenarios.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_INGESTAUDITMESSAGES` |
| **App config key** | `ServiceControl/IngestAuditMessages` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |


## Data retention

### ServiceControl.Audit/ExpirationProcessTimerInSeconds

The number of seconds to wait between checking for expired messages.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_EXPIRATIONPROCESSTIMERINSECONDS` |
| **App config key** | `ServiceControl.Audit/ExpirationProcessTimerInSeconds` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `600` (10 minutes) |

Valid range is `0` to `10800` (3 Hours).

Setting the value to `0` will disable the expiration process. This is not recommended and it is only provided for fault finding.

#if-version [,5)
### ServiceControl.Audit/ExpirationProcessBatchSize

This controls the batch size used when deleting audit messages that have exceeded the audit retention period.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_EXPIRATIONPROCESSBATCHSIZE` |
| **App config key** | `ServiceControl.Audit/ExpirationProcessBatchSize` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `65512` |

The minimum allowed value for this setting is `10240`; there is no hard-coded maximum as this is dependent on system performance.

#end-if
### ServiceControl.Audit/AuditRetentionPeriod

The grace period to keep an audit message before it is deleted.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_AUDITRETENTIONPERIOD` |
| **App config key** | `ServiceControl.Audit/AuditRetentionPeriod` |
| **SCMU field** | `AUDIT RETENTION PERIOD` |

| Type | Default value |
| --- | --- |
| timespan | None (required) |

Valid range for this setting is from 1 hour to 365 days.

> [!NOTE]
> Starting with version 4.26.0, new audit instances using RavenDB 5 will use the built-in RavenDB expiration process. Changing the audit retention setting will affect only newly ingested messages. Already ingested messages will expire according to the previous retention setting value.

## Performance tuning

### ServiceControl.Audit/MaxBodySizeToStore

This setting specifies the upper limit on body size, in bytes, to be configured.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MAXBODYSIZETOSTORE` |
| **App config key** | `ServiceControl.Audit/MaxBodySizeToStore` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `102400` (100Kb) |

### ServiceControl.Audit/MaximumConcurrencyLevel

The maximum number of messages that can be concurrently pulled from the message transport.

It is important that the maximum concurrency level be incremented only if there are no verified bottlenecks in CPU, RAM, network I/O, storage I/O, and storage index lag. Higher numbers can result in faster audit message ingestion, but also consume more server resources, and can increase costs in the case of cloud transports that have associated per-operation costs. In some cases, the ingestion rate can be too high and the underlying database cannot keep up with indexing the new messages. In this case, consider lowering the maximum concurrency level to a value that still allows a suitable ingestion rate while easing the pressure on the database.

Cloud transports with higher latency can benefit from higher concurrency values, but costs can increase as well. Local transports using fast local SSD drives and low latency do not benefit as much.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MAXIMUMCONCURRENCYLEVEL` |
| **App config key** | `ServiceControl.Audit/MaximumConcurrencyLevel` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `32` in 4.12.0<br/>`10` in earlier versions |

### ServiceControl.Audit/EnableFullTextSearchOnBodies

_Added in 4.17.0_

Use this setting to configure whether the bodies of processed messages should be full-text indexed for searching.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_ENABLEFULLTEXTSEARCHONBODIES` |
| **App config key** | `ServiceControl.Audit/EnableFullTextSearchOnBodies` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

> [!NOTE]
> If the audit instance uses RavenDB 5 persistence (available starting 4.26.0), changing the full-text search setting will cause indexes to be redeployed and rebuilt. Depending on the number of documents stored, this operation might take a long time and search results won't be available until completed.

#if-version [5,)
### ServiceControl.Audit/BulkInsertCommitTimeoutInSeconds

Configures the maximum duration, in seconds, for processing a batch of audited messages.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_BULKINSERTCOMMITTIMEOUTINSECONDS` |
| **App config key** | `ServiceControl.Audit/BulkInsertCommitTimeoutInSeconds` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `60` (1 minute) |

#end-if
## Transport

### ServiceControl.Audit/TransportType

The transport type to run ServiceControl with.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_TRANSPORTTYPE` or `TRANSPORTTYPE` |
| **App config key** | `ServiceControl.Audit/TransportType` |
| **SCMU field** | `TRANSPORT` |

| Type | Default value |
| --- | --- |
| string | `MSMQ` |

Valid values are documented in the [ServiceControl transport configuration documentation](/servicecontrol/transports.md).

### NServiceBus/Transport

The connection string for the transport. This setting must be entered in the `connectionStrings` section of the configuration file when configured using the app config.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_CONNECTIONSTRING` OR `CONNECTIONSTRING` |
| **App config key** | `NServiceBus/Transport` in `connectionStrings` |
| **SCMU field** | `TRANSPORT CONNECTION STRING` |

| Type | Default value |
| --- | --- |
| string | None |

Valid values are documented in the [ServiceControl transport configuration documentation](/servicecontrol/transports.md).

### ServiceBus/AuditQueue

The name of the audit queue to ingest messages from.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICEBUS_AUDITQUEUE` |
| **App config key** | `ServiceBus/AuditQueue` |
| **SCMU field** | `AUDIT QUEUE NAME` |

| Type | Default value |
| --- | --- |
| string | `audit` |

### ServiceControl.Audit/ForwardAuditMessages

Use this setting to configure whether processed audit messages are forwarded to another queue or not. This entry should be set to `false` if there is no external process reading messages from the [`ServiceBus/AuditLogQueue`](#transport-servicebusauditlogqueue)

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_FORWARDAUDITMESSAGES` |
| **App config key** | `ServiceControl.Audit/ForwardAuditMessages` |
| **SCMU field** | `Forward audit messages?` |

| Type | Default value |
| --- | --- |
| bool | `false` (Off) |

### ServiceBus/AuditLogQueue

The audit queue name to use for forwarding audit messages. This setting is ignored unless `ServiceControl.Audit/ForwardAuditMessages` is enabled.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICEBUS_AUDITLOGQUEUE` |
| **App config key** | `ServiceBus/AuditLogQueue` |
| **SCMU field** | `AUDIT FORWARDING QUEUE NAME` |

| Type | Default value |
| --- | --- |
| string | `<AuditQueue>.log` |

> [!NOTE]
> Changing the configuration file or environment value directly will not result in the queue being created. If you are using the ServiceControl Management utility to manage your ServiceControl audit instance changing the value will create the forwarding queue if it has not been created.

### ServiceControl.Audit/ServiceControlQueueAddress

The ServiceControl primary instance queue name to use to send plugin messages (e.g. Heartbeats, Custom Checks, Saga Audit, etc ).

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_SERVICECONTROLQUEUEADDRESS` |
| **App config key** | `ServiceControl.Audit/ServiceControlQueueAddress` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `Particular.ServiceControl` |

## Troubleshooting

### ServiceControl.Audit/DataSpaceRemainingThreshold

The percentage threshold for the [Message database storage space](/servicecontrol/servicecontrol-instances/#notifications-health-monitoring-message-database-storage-space) check. If the remaining hard drive space drops below this threshold (as a percentage of the total space on the drive) then the check will fail, alerting the user.

| Type | Default value |
| --- | --- |
| int | `20` |

### ServiceControl.Audit/MinimumStorageLeftRequiredForIngestion

The percentage threshold for the [Critical message database storage space](/servicecontrol/servicecontrol-instances/#notifications-health-monitoring-message-database-storage-space) check. If the remaining hard drive space drops below this threshold (as a percentage of the total space on the drive), then the check will fail, alerting the user. The message ingestion will also be stopped to prevent data loss. Message ingestion will resume once more disk space is made available.

| Type | Default value |
| --- | --- |
| int | `5` |

#if-version [,5)

### Raven/Esent/LogsPath


This setting is applicable only on instances that use the RavenDB 3.5 storage engine.

The path for the Esent logs on disk.

| Type | Default value |
| --- | --- |
| string | `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<instance_name>\DB\Logs` |

#end-if
