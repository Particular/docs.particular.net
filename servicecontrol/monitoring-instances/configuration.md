---
title: Monitoring Instance Configuration Settings
summary: Categorized list of ServiceControl Monitoring instance configuration settings.
component: ServiceControl
reviewed: 2024-06-24
redirects:
 - servicecontrol/monitoring-instances/installation/creating-config-file
---
The configuration of a ServiceControl Monitoring instance is controlled by the `ServiceControl.Monitoring.exe.config` file or by setting environment variables. When a setting configuration exists as both an environment variables and in the application configuration file, the environment variable setting takes precedence.

Deployments using the ServiceControl Management utility (SCMU) can use that application to make a subset of configuration settings which are read from and written to the application configuration file.

>[!NOTE]
>Configuration settings in the application configuration file are applicable to the `appSettings` section unless otherwise specified.

## Locating the configuration file using SCMU

![image](https://github.com/Particular/docs.particular.net/assets/88632084/c9b160ba-03a5-4c73-9812-c942af6657da)

## Host Settings

Prior to modifying these configuration settings review [Setting a Custom Hostname](configure-the-uri.md):

### Monitoring/InstanceName

_Added in version 5.5.0_

The name to be used by the monitoring instance and the name of the monitoring queue.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_INSTANCENAME` |
| **App config key** | `Monitoring/InstanceName` |
| **SCMU field** | Instance/Queue Name |

| Type | Default value |
| --- | --- |
| string | `Particular.Monitoring` |

> [!WARNING]
> After changing this setting it's necessary to run the monitoring instance setup procedure by executing `ServiceControl.Monitoring.exe -s` in the command prompt. This ensures that all necessary queues are created and properly configured.

### Monitoring/HttpHostname

The hostname to bind the embedded HTTP server to, modify to bind to a specific hostname, eg. `monitoring.mydomain.com`.

_Not applicable to container deployments. Containers bind to any hostname._

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_HTTPHOSTNAME` |
| **App config key** | `Monitoring/HttpHostname` |
| **SCMU field** | Host Name |

| Type | Default value |
| --- | --- |
| string | `localhost` |

> [!NOTE]
> This setting must have a value for the Monitoring instance API to be available from remote machines.

### Monitoring/HttpPort

The port to bind the embedded HTTP server.

_Not applicable to container deployments. Containers always expose port `33633`._

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_HTTPPORT` |
| **App config key** | `Monitoring/HttpPort` |
| **SCMU field** | Port Number |

| Type | Default value |
| --- | --- |
| int | `33633` |

### Monitoring/ShutdownTimeout

_Added in version 6.5.0_

The maximum allowed time for the process to complete the shutdown.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_SHUTDOWNTIMEOUT` |
| **App config key** | `Monitoring/ShutdownTimeout` |
| **SCMU field** | N/A |

| Environment/Installation type            | Type     | Default value |
| --- | --- |---|
| Containers | TimeSpan | `00:00:05` (5 seconds) |
| Installation via PowerShell (on Windows) | TimeSpan | `00:02:00` (2 minutes) |
| Installation via ServiceControl Management Utility (SCMU) (on Windows) | TimeSpan | `00:02:00` (2 minutes) |

## [Authentication](/servicecontrol/security/configuration/authentication.md)

These settings configure [authentication using OAuth 2.0 and OpenID Connect](/servicecontrol/security/). Refer to the [hosting and security guide](/servicecontrol/security/hosting-guide.md), or [authentication configuration examples](/servicecontrol/security/configuration/authentication.md#identity-provider-setup-configuration-examples) for additional information.

### Monitoring/Authentication.Enabled

_Added in version 6.10.0_

Enables or disables authentication. This is a **Global switch** and all other authentication settings are ignored unless this is `true`.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_AUTHENTICATION_ENABLED` |
| **App config key** | `Monitoring/Authentication.Enabled` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `false` |

### Monitoring/Authentication.Authority

_Added in version 6.10.0_

The URL of the OpenID Connect authority (identity provider) used to authenticate tokens.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_AUTHENTICATION_AUTHORITY` |
| **App config key** | `Monitoring/Authentication.Authority` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |

### Monitoring/Authentication.Audience

_Added in version 6.10.0_

The expected audience value in the JWT token, typically the application ID or URI of the API.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_AUTHENTICATION_AUDIENCE` |
| **App config key** | `Monitoring/Authentication.Audience` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |

### Monitoring/Authentication.ValidateIssuer

_Added in version 6.10.0_

Controls whether the token issuer is validated against the authority.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_AUTHENTICATION_VALIDATEISSUER` |
| **App config key** | `Monitoring/Authentication.ValidateIssuer` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

### Monitoring/Authentication.ValidateAudience

_Added in version 6.10.0_

Controls whether the token audience is validated.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_AUTHENTICATION_VALIDATEAUDIENCE` |
| **App config key** | `Monitoring/Authentication.ValidateAudience` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

### Monitoring/Authentication.ValidateLifetime

_Added in version 6.10.0_

Controls whether the token expiration is validated.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_AUTHENTICATION_VALIDATELIFETIME` |
| **App config key** | `Monitoring/Authentication.ValidateLifetime` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

### Monitoring/Authentication.ValidateIssuerSigningKey

_Added in version 6.10.0_

Controls whether the token signing key is validated.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_AUTHENTICATION_VALIDATEISSUERSIGNINGKEY` |
| **App config key** | `Monitoring/Authentication.ValidateIssuerSigningKey` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

### Monitoring/Authentication.RequireHttpsMetadata

_Added in version 6.10.0_

Controls whether HTTPS is required when retrieving metadata from the authority.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_AUTHENTICATION_REQUIREHTTPSMETADATA` |
| **App config key** | `Monitoring/Authentication.RequireHttpsMetadata` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

> [!WARNING]
> Setting this to `false` is not recommended for production environments.

## [TLS](/servicecontrol/security/configuration/tls.md)

These settings configure HTTPS. Refer to the [hosting and security guide](/servicecontrol/security/hosting-guide.md), or [TLS configuration examples](/servicecontrol/security/configuration/tls.md#configuration-examples) for additional information.

### Monitoring/Https.Enabled

_Added in version 6.10.0_

Enables Kestrel HTTPS with a certificate.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_HTTPS_ENABLED` |
| **App config key** | `Monitoring/Https.Enabled` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `false` |

### Monitoring/Https.CertificatePath

_Added in version 6.10.0_

The path to the PFX or PEM certificate file.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_HTTPS_CERTIFICATEPATH` |
| **App config key** | `Monitoring/Https.CertificatePath` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |

### Monitoring/Https.CertificatePassword

_Added in version 6.10.0_

The password for the certificate file, if required.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_HTTPS_CERTIFICATEPASSWORD` |
| **App config key** | `Monitoring/Https.CertificatePassword` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |

### Monitoring/Https.RedirectHttpToHttps

_Added in version 6.10.0_

Redirects HTTP requests to HTTPS. This is intended for use with a reverse proxy that handles both HTTP and HTTPS traffic.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_HTTPS_REDIRECTHTTPTOHTTPS` |
| **App config key** | `Monitoring/Https.RedirectHttpToHttps` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `false` |

> [!NOTE]
> When running ServiceControl directly without a reverse proxy, the application only listens on a single protocol (HTTP or HTTPS). This setting is intended for use with a reverse proxy that handles both HTTP and HTTPS traffic.

### Monitoring/Https.Port

_Added in version 6.10.0_

The HTTPS port to use in redirect URLs. Required when `RedirectHttpToHttps` is enabled in reverse proxy scenarios.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_HTTPS_PORT` |
| **App config key** | `Monitoring/Https.Port` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | None |

### Monitoring/Https.EnableHsts

_Added in version 6.10.0_

Enables HTTP Strict Transport Security (HSTS).

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_HTTPS_ENABLEHSTS` |
| **App config key** | `Monitoring/Https.EnableHsts` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `false` |

> [!NOTE]
> Review the implications of [enabling HSTS](/servicecontrol/security/configuration/tls.md#security-considerations-hsts) before doing so.

### Monitoring/Https.HstsMaxAgeSeconds

_Added in version 6.10.0_

The max-age value in seconds for the HSTS header.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_HTTPS_HSTSMAXAGESECONDS` |
| **App config key** | `Monitoring/Https.HstsMaxAgeSeconds` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `31536000` (1 year) |

### Monitoring/Https.HstsIncludeSubDomains

_Added in version 6.10.0_

Includes subdomains in the HSTS policy.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_HTTPS_HSTSINCLUDESUBDOMAINS` |
| **App config key** | `Monitoring/Https.HstsIncludeSubDomains` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `false` |

## [Forwarded headers](/servicecontrol/security/configuration/forward-headers.md)

These settings configure forwarded headers for reverse proxy scenarios. Refer to the [hosting and security guide](/servicecontrol/security/hosting-guide.md), or [forward headers configuration examples](/servicecontrol/security/configuration/forward-headers.md#configuration-examples) for additional information.

### Monitoring/ForwardedHeaders.Enabled

_Added in version 6.10.0_

Enables processing of forwarded headers (X-Forwarded-For, X-Forwarded-Proto, etc.).

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_FORWARDEDHEADERS_ENABLED` |
| **App config key** | `Monitoring/ForwardedHeaders.Enabled` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

### Monitoring/ForwardedHeaders.TrustAllProxies

_Added in version 6.10.0_

Trusts forwarded headers from any source. Set to `false` when using `KnownProxies` or `KnownNetworks`.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_FORWARDEDHEADERS_TRUSTALLPROXIES` |
| **App config key** | `Monitoring/ForwardedHeaders.TrustAllProxies` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

> [!WARNING]
> For production environments behind a reverse proxy, set this to `false` and configure `KnownProxies` or `KnownNetworks` to restrict which proxies are trusted.

### Monitoring/ForwardedHeaders.KnownProxies

_Added in version 6.10.0_

A comma-separated list of trusted proxy IP addresses e.g., `127.0.0.1`

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_FORWARDEDHEADERS_KNOWNPROXIES` |
| **App config key** | `Monitoring/ForwardedHeaders.KnownProxies` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |

### Monitoring/ForwardedHeaders.KnownNetworks

_Added in version 6.10.0_

A comma-separated list of trusted CIDR network ranges e.g., `10.0.0.0/8,172.16.0.0/12`

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_FORWARDEDHEADERS_KNOWNNETWORKS` |
| **App config key** | `Monitoring/ForwardedHeaders.KnownNetworks` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None | 

## [CORS](/servicecontrol/security/configuration/cors.md)

These settings configure Cross-Origin Resource Sharing (CORS). Refer to the [hosting and security guide](/servicecontrol/security/hosting-guide.md), or [cors configuration examples](/servicecontrol/security/configuration/cors.md#configuration-examples) for additional information.

### Monitoring/Cors.AllowAnyOrigin

_Added in version 6.10.0_

Allows requests from any origin.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_CORS_ALLOWANYORIGIN` |
| **App config key** | `Monitoring/Cors.AllowAnyOrigin` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

> [!WARNING]
> For production environments, set this to `false` and configure `AllowedOrigins` to restrict which origins can access the API.

### Monitoring/Cors.AllowedOrigins

_Added in version 6.10.0_

A comma-separated list of allowed origins e.g., `https://servicepulse.example.com,https://admin.example.com`

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_CORS_ALLOWEDORIGINS` |
| **App config key** | `Monitoring/Cors.AllowedOrigins` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |

## Logging

### Monitoring/LogPath

The path for the Monitoring instance logs.

_Not applicable to container deployments. Containers always log to stdout._

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_LOGPATH` |
| **App config key** | `Monitoring/LogPath` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | The folder that contains the Monitoring instance executable. |

### Monitoring/LogLevel

Controls the LogLevel of the Monitoring instance logs.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_LOGLEVEL` |
| **App config key** | `Monitoring/LogLevel` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `Warn` |

Valid settings are: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.

This setting will default to `Warn` if an invalid value is assigned.

## Transport

### Monitoring/TransportType

The transport type to run ServiceControl Monitor with.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_TRANSPORTTYPE` |
| **App config key** | `Monitoring/TransportType` |
| **SCMU field** | Transport |

| Type | Default value |
| --- | --- |
| string | None |

Valid values are documented in the [ServiceControl transport configuration documentation](/servicecontrol/transports.md).

### NServiceBus/Transport

The connection string for the transport.

| Context | Name |
| --- | --- |
| **Environment variable** | `NSERVICEBUS_TRANSPORT` |
| **App config key** | `NServiceBus/Transport` in `connectionStrings` |
| **SCMU field** | Connection String |

| Type | Default value |
| --- | --- |
| string | None |

Valid values are documented in the [ServiceControl transport configuration documentation](/servicecontrol/transports.md).

### Monitoring/ErrorQueue

The error queue name.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_ERRORQUEUE` |
| **App config key** | `Monitoring/ErrorQueue` |
| **SCMU field** | Error Queue Name |

| Type | Default value |
| --- | --- |
| string | `error` |

### Monitoring/MaximumConcurrencyLevel

The maximum concurrency that will be used for ingesting metric messages.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_MAXIMUMCONCURRENCYLEVEL` |
| **App config key** | `Monitoring/MaximumConcurrencyLevel` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `32` |

### Monitoring/EndpointUptimeGracePeriod

The time after which the endpoint is considered stale if it stops sending messages.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_ENDPOINTUPTIMEGRACEPERIOD` |
| **App config key** | `Monitoring/EndpointUptimeGracePeriod` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| TimeSpan | `00:00:40` (40 seconds) |

## Usage Reporting

### Monitoring/ServiceControlThroughputDataQueue

_Added in version 5.4.0_

The queue on which throughput data is received by ServiceControl error instance. This setting must match the equivalent [`LicensingComponent/ServiceControlThroughputDataQueue`](/servicecontrol/servicecontrol-instances/configuration.md#usage-reporting-when-using-servicecontrol-licensingcomponentservicecontrolthroughputdataqueue) setting on the ServiceControl error instance.

In most instances these settings do not need to be modified.

If running multiple setups of the Platform Tools (i.e. multiple versions of ServiceControl Error and monitoring instances) then modify these settings so that the queue on each Monitoring instance is matched to the queue of its error instance.

If using [MSMQ transport](/transports/msmq) and the monitoring instance is installed on a different machine to the ServiceControl error instance, then only the monitoring instance setting needs to be modified to include the machine name of the error instance in the queue address.

If using [PostgreSQL transport](/transports/postgresql/), and a schema other than `public` is required, then the schema name needs to be included in the `Monitoring/ServiceControlThroughputDataQueue` setting

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_SERVICECONTROLTHROUGHPUTDATAQUEUE` |
| **App config key** | `Monitoring/ServiceControlThroughputDataQueue` |

| Type | Default value |
| --- | --- |
| string | `ServiceControl.ThroughputData` |
