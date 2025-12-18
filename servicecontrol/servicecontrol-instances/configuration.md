---
title: Error Instance Configuration Settings
summary: Categorized list of ServiceControl Error instance configuration settings.
component: ServiceControl
reviewed: 2025-10-22
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

### ServiceControl/InstanceName

_Added in version 5.5.0_

The name to be used by the error instance and the name of the input queue.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_INSTANCENAME` |
| **App config key** | `ServiceControl/InstanceName` |
| **SCMU field** | Instance/Queue Name |

| Type | Default value |
| --- | --- |
| string | `Particular.ServiceControl` |

> [!NOTE]
> In versions prior to 5.5.0, the `InternalQueueName` setting can be used instead.

### ServiceControl/HostName

The hostname to bind the embedded HTTP API server to; modify this setting to bind to a specific hostname, e.g. `sc.mydomain.com` and make the machine remotely accessible.

This field can also contain a `*` as a wildcard to allow remote connections that use any hostname.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_HOSTNAME` |
| **App config key** | `ServiceControl/HostName` |
| **SCMU field** | `HOST NAME` |

| Type | Default value |
| --- | --- |
| string | `localhost` |

> [!WARNING]
> If the `ServiceControl/HostName` setting is changed, and the `ServiceControl/DbPath` setting is not set, the path of the embedded RavenDB is changed. Refer to [Customize RavenDB Embedded Location](/servicecontrol/configure-ravendb-location.md).

partial: ravendb-hostname-exposure-warning

### ServiceControl/Port

The port to bind the embedded HTTP API server.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_PORT` |
| **App config key** | `ServiceControl/Port` |
| **SCMU field** | `PORT NUMBER` |

| Type | Default value |
| --- | --- |
| int | `33333` |

> [!WARNING]
> If the `ServiceControl/Port` setting is changed, and the `ServiceControl/DbPath` setting is not set, the path of the embedded RavenDB is changed. Refer to [Customize RavenDB Embedded Location](/servicecontrol/configure-ravendb-location.md).

### ServiceControl/DatabaseMaintenancePort

The port to expose the RavenDB database.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_DATABASEMAINTENANCEPORT` |
| **App config key** | `ServiceControl/DatabaseMaintenancePort` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `33334` |

> [!NOTE]
> This setting is not relevant when running an error instance in a container.

### ServiceControl/VirtualDirectory

The virtual directory to bind the embedded HTTP server to; modify this setting to bind to a specific virtual directory.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_VIRTUALDIRECTORY` |
| **App config key** | `ServiceControl/VirtualDirectory` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | _None_ |

### ServiceControl/RemoteInstances

A configuration that specifies one or more attached Audit instances. See also [ServiceControl Remote Instances](remotes.md).

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_REMOTEINSTANCES` |
| **App config key** | `ServiceControl/RemoteInstances` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |

### ServiceControl/ShutdownTimeout

_Added in version 6.5.0_

The maximum allowed time for the process to complete the shutdown.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_SHUTDOWNTIMEOUT` |
| **App config key** | `ServiceControl/ShutdownTimeout` |
| **SCMU field** | N/A |

| Environment/Installation type            | Type     | Default value |
|---|---|---|
| Containers | TimeSpan | `00:00:05` (5 seconds) |
| Installation via PowerShell (on Windows) | TimeSpan | `00:02:00` (2 minutes) |
| Installation via ServiceControl Management Utility (SCMU) (on Windows) | TimeSpan | `00:02:00` (2 minutes) |

### ServiceControl/MaintenanceMode

Run [ServiceControl error instance in maintenance mode](/servicecontrol/ravendb/accessing-database.md) in order to do database maintenance.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MAINTENANCEMODE` |
| **App config key** | `ServiceControl/MaintenanceMode` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| boolean | `False` |

### ServiceControl/DisableExternalIntegrationsPublishing

[ServiceControl publishes integration events](/servicecontrol/contracts.md), if no subscribers exist this can be disabled.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_DISABLEEXTERNALINTEGRATIONSPUBLISHING` |
| **App config key** | `ServiceControl/DisableExternalIntegrationsPublishing` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | False |

## Embedded database

These settings are not valid for ServiceControl instances hosted in a container.

### ServiceControl/DbPath

The path where the internal RavenDB is located.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_DBPATH` |
| **App config key** | `ServiceControl/DbPath` |
| **SCMU field** | `Database Path` |

| Type | Default value |
| --- | --- |
| string | `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<instance_name>\DB` |

> [!NOTE]
> This setting is not relevant when running an error instance in a container.

### ServiceControl/RavenDBLogLevel

Controls the LogLevel of the RavenDB logs. See [Logging](/servicecontrol/logging.md).

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_RAVENDBLOGLEVEL` |
| **App config key** | `ServiceControl/RavenDBLogLevel` |
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
| string | `Info` |

Valid settings are: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.

#end-if

#if-version [,5)

### Raven/IndexStoragePath

The indexes and Esent logs can be stored in a different path from the the RavenDB database data files by using the following [RavenDB configuration app settings](https://ravendb.net/docs/article-page/2.5/csharp/server/administration/configuration):

> [!NOTE]
> Only supported on RavenDB 3.5 storage engine (prior version 5). Use [symbolic links (soft links) to map any RavenDB storage subfolder](https://ravendb.net/docs/article-page/5.4/csharp/server/storage/customizing-raven-data-files-locations) to other physical drives.

The path for the indexes on disk.

Type: string

Default: `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<instance_name>\DB\indexes`

### Raven/Esent/LogsPath

The path for the Esent logs on disk.

Type: string

Default: `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<instance_name>\DB\Logs`

#end-if

## Logging

### ServiceControl/LogPath

The path for the ServiceControl logs.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_LOGPATH` |
| **App config key** | `ServiceControl/LogPath` |
| **SCMU field** | `LOG PATH` |

| Type | Default value |
| --- | --- |
| string | `%LOCALAPPDATA%\Particular\ServiceControl\logs` |

> [!NOTE]
> %LOCALAPPDATA% is a user-specific path on Windows.
>
> When hosted on containers, logs are sent to **stdout** and this setting is ignored.

### ServiceControl/LogLevel

Controls the LogLevel of the ServiceControl logs.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_LOGLEVEL` |
| **App config key** | `ServiceControl/LogLevel` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `Info` |

## Recoverability

### ServiceControl/TimeToRestartErrorIngestionAfterFailure

Controls the maximum time delay to wait before restarting the error ingestion pipeline after detecting a connection problem.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_TIMETORESTARTAUDITINGESTIONAFTERFAILURE` |
| **App config key** | `ServiceControl/TimeToRestartAuditIngestionAfterFailure` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| timespan | 60 seconds |

Valid settings are between 5 seconds and 1 hour.

### ServiceControl/IngestErrorMessages

Version: 4.33.0+

Set to `false` to disable ingesting new error messages. Useful in some upgrade scenarios.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_INGESTERRORMESSAGES` |
| **App config key** | `ServiceControl/IngestErrorMessages` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

## Data retention

### ServiceControl/ExpirationProcessTimerInSeconds

The number of seconds to wait between checking for expired messages.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_EXPIRATIONPROCESSTIMERINSECONDS` |
| **App config key** | `ServiceControl/ExpirationProcessTimerInSeconds` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `600` (10 minutes) |

Valid range is `0` to `10800` (3 Hours).

Setting the value to `0` will disable the expiration process. This is not recommended and it is only provided for fault finding.

#if-version [,5)
### ServiceControl/ExpirationProcessBatchSize

This controls the batch size used when deleting error messages that have exceeded the error retention period after they have been successfully retried or manually archived.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_EXPIRATIONPROCESSBATCHSIZE` |
| **App config key** | `ServiceControl/ExpirationProcessBatchSize` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `65512` |

The minimum allowed value for this setting is `10240`; there is no hard-coded maximum as this is dependent on system performance.

#end-if
### ServiceControl/ErrorRetentionPeriod

The grace period that errored messages are kept before they are deleted.

For a message to be considered for deletion, it needs to have a status of either `Archived`, `RetryIssued`, or `Resolved`.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_ERRORRETENTIONPERIOD` |
| **App config key** | `ServiceControl/ErrorRetentionPeriod` |
| **SCMU field** | `ERROR RETENTION PERIOD` |

| Type | Default value |
| --- | --- |
| timespan | None (required) |

Valid range for this setting is between 5 days and 45 days.

### ServiceControl/EventRetentionPeriod

The grace period to keep event logs before they are deleted.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_EVENTRETENTIONPERIOD` |
| **App config key** | `ServiceControl/EventRetentionPeriod` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| timespan | 14 days |

Valid range for this setting is from 1 hour to 200 days.

### ServiceControl/TrackInstancesInitialValue

The default value for whether to `Track` or `Do not Track` endpoint instance on newly discovered endpoints.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_TRACKINSTANCESINITIALVALUE` |
| **App config key** | `ServiceControl/TrackInstancesInitialValue` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` (`Track`) |

## Performance tuning

### ServiceControl/MaximumConcurrencyLevel

This setting controls how many messages can be processed concurrently (in parallel) by ServiceControl. The default value is 10.

In some cases, the ingestion rate can be too high and the underlying database cannot keep up with indexing the new messages. In this case, consider lowering the maximum concurrency level to a value that still allows a suitable ingestion rate while easing the pressure on the database.

> [!WARNING]
> The maximum concurrency level should be incremented only if there are no verified bottlenecks in CPU, RAM, network I/O, storage I/O, and storage index lag.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MAXIMUMCONCURRENCYLEVEL` |
| **App config key** | `ServiceControl/MaximumConcurrencyLevel` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `10` |

### ServiceControl/EnableFullTextSearchOnBodies

Use this setting to configure whether the bodies of processed error messages should be full-text indexed for searching.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_ENABLEFULLTEXTSEARCHONBODIES` |
| **App config key** | `ServiceControl/EnableFullTextSearchOnBodies` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

> [!NOTE]
> Changing the full-text search setting will cause indexes to be redeployed and rebuilt. Depending on the number of documents stored, this operation might take a long time and search results won't be available until completed.

## Transport

### ServiceControl/TransportType

The transport type to run ServiceControl with.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_TRANSPORTTYPE` or `TRANSPORTTYPE` |
| **App config key** | `ServiceControl/TransportType` |
| **SCMU field** | `TRANSPORT` |

| Type | Default value |
| --- | --- |
| string | `MSMQ` |

Valid values are documented in the [ServiceControl transport configuration documentation](/servicecontrol/transports.md).

### NServiceBus/Transport

The connection string for the transport. This setting must be entered in the `connectionStrings` section of the configuration file when configured using the app config.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_CONNECTIONSTRING` or `CONNECTIONSTRING` |
| **App config key** | `NServiceBus/Transport` in `connectionStrings` |
| **SCMU field** | `TRANSPORT CONNECTION STRING` |

| Type | Default value |
| --- | --- |
| string | None |

Valid values are documented in the [ServiceControl transport configuration documentation](/servicecontrol/transports.md).

### ServiceBus/ErrorQueue

The name of the error queue to ingest messages from.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICEBUS_ERRORQUEUE` |
| **App config key** | `ServiceBus/ErrorQueue` |
| **SCMU field** | `ERROR QUEUE NAME` |

| Type | Default value |
| --- | --- |
| string | `error` |

### ServiceControl/ForwardErrorMessages

Use this setting to configure whether processed error messages are forwarded to another queue or not. This entry should be set to `false` if there is no external process reading messages from the [`ServiceBus/ErrorLogQueue`](#transport-servicecontrolforwarderrormessages).

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_FORWARDERRORMESSAGES` |
| **App config key** | `ServiceControl/ForwardErrorMessages` |
| **SCMU field** | `ERROR FORWARDING` |

| Type | Default value |
| --- | --- |
| bool | `false` (Off) |

This entry should be set to `false` if there is no external process reading messages from the `Error Forwarding Queue`.

### ServiceBus/ErrorLogQueue

The error queue name to use for forwarding error messages. This setting is ignored unless `ServiceControl/ForwardErrorMessages` is enabled.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICEBUS_ERRORLOGQUEUE` |
| **App config key** | `ServiceBus/ErrorLogQueue` |
| **SCMU field** | `ERROR FORWARDING QUEUE NAME` |

| Type | Default value |
| --- | --- |
| string | `<ErrorQueue>.log` |

> [!NOTE]
> Changing the configuration file or environment value directly will not result in the queue being created. If you are using the ServiceControl Management utility to manage your ServiceControl error instance changing the value will create the forwarding queue if it has not been created.

## Usage Reporting when using ServiceControl

### LicensingComponent/ServiceControlThroughputDataQueue

Version: 5.4.0+

The queue on which throughput data is received by the ServiceControl Error instance. This setting must match the equivalent [`Monitoring/ServiceControlThroughputDataQueue`](/servicecontrol/monitoring-instances/configuration.md#usage-reporting-monitoringservicecontrolthroughputdataqueue) setting for the Monitoring instance.

In most instances these settings do not need to be modified.

If running multiple setups of the Platform Tools (i.e. multiple versions of ServiceControl error and monitoring instances), then modify these settings so that the queue on each monitoring instance is matched to the queue of its error instance.

If using [MSMQ transport](/transports/msmq) and the monitoring instance is installed on a different machine than the ServiceControl error instance, only the monitoring instance setting needs to be modified to include the machine name of the error instance in the queue address.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_SERVICECONTROLTHROUGHPUTDATAQUEUE` |
| **App config key** | `LicensingComponent/ServiceControlThroughputDataQueue` |

| Type | Default value |
| --- | --- |
| string | `ServiceControl.ThroughputData` |

## Usage Reporting when using the Azure Service Bus transport

The following settings are part of [Usage Reporting Setup when using the Azure Service Bus transport](/servicepulse/usage-config.md#connection-setup-azure-service-bus)

### LicensingComponent/ASB/ServiceBusName

Version: 5.4.0+

The Azure ServiceBus name.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_ASB_SERVICEBUSNAME` |
| **App config key** | `LicensingComponent/ASB/ServiceBusName` |

| Type | Default value |
| --- | --- |
| string | obtained from ServiceControl |

### LicensingComponent/ASB/TenantId

Version: 5.4.0+

The Azure [Tenant ID](https://learn.microsoft.com/en-us/azure/azure-portal/get-subscription-tenant-id#find-your-microsoft-entra-tenant).

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_ASB_TENANTID` |
| **App config key** | `LicensingComponent/ASB/TenantId` |

| Type | Required |
| --- | --- |
| string | yes |

### LicensingComponent/ASB/SubscriptionId

Version: 5.4.0+

The Azure [subscription ID](https://learn.microsoft.com/en-us/azure/azure-portal/get-subscription-tenant-id#find-your-azure-subscription).

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_ASB_SUBSCRIPTIONID` |
| **App config key** | `LicensingComponent/ASB/SubscriptionId` |

| Type | Required |
| --- | --- |
| string | yes |

### LicensingComponent/ASB/ClientId

Version: 5.4.0+

The Client ID (aka Application ID) for an [Azure service principal](https://learn.microsoft.com/en-us/entra/identity-platform/app-objects-and-service-principals?tabs=browser#service-principal-object) that has access to read metrics data for the Azure Service Bus namespace.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_ASB_CLIENTID` |
| **App config key** | `LicensingComponent/ASB/ClientId` |

| Type | Required |
| --- | --- |
| string | yes |

Example Client ID from an Azure App Registration:
![Screenshot showing where the Client ID appears in an App Registration](/servicecontrol/asb-app-service-principal.png)

### LicensingComponent/ASB/ClientSecret

Version: 5.4.0+

The [client secret](https://learn.microsoft.com/en-us/entra/identity-platform/howto-create-service-principal-portal#option-3-create-a-new-client-secret) for an Azure service principal that has access to read metrics data for the Azure Service Bus namespace.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_ASB_CLIENTSECRET` |
| **App config key** | `LicensingComponent/ASB/ClientSecret` |

| Type | Required |
| --- | --- |
| string | yes |

> [!NOTE]
> Certificates and federated credentials are not supported at this time.

### LicensingComponent/ASB/ManagementUrl

Version: 5.4.0+

The Azure ManagementUrl URL.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_ASB_MANAGEMENTURL` |
| **App config key** | `LicensingComponent/ASB/ManagementUrl` |

| Type | Default value |
| --- | --- |
| string | https://management.azure.com/ |

This setting only needs to be configured if not using the public AzureCloud environment.
For other environments:

- AzureGermanCloud - https://management.microsoftazure.de/
- AzureUSGovernment - https://management.usgovcloudapi.net/
- AzureChinaCloud - https://management.chinacloudapi.cn/

## Usage Reporting when using the Amazon SQS transport

### LicensingComponent/AmazonSQS/AccessKey

Version: 5.4.0+

The AWS Access Key ID to use to discover queue names and gather per-queue metrics.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_AMAZONSQS_ACCESSKEY` |
| **App config key** | `LicensingComponent/AmazonSQS/AccessKey` |

| Type | Default value |
| --- | --- |
| string | obtained from ServiceControl |

### LicensingComponent/AmazonSQS/SecretKey

Version: 5.4.0+

The AWS Secret Access Key to use to discover queue names and gather per-queue metrics.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_AMAZONSQS_SECRETKEY` |
| **App config key** | `LicensingComponent/AmazonSQS/SecretKey` |

| Type | Default value |
| --- | --- |
| string | obtained from ServiceControl |

### LicensingComponent/AmazonSQS/Profile

Version: 5.4.0+

The name of a local AWS credentials profile to use to discover queue names and gather per-queue metrics.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_AMAZONSQS_PROFILE` |
| **App config key** | `LicensingComponent/AmazonSQS/Profile` |

| Type | Default value |
| --- | --- |
| string | |

### LicensingComponent/AmazonSQS/Region

Version: 5.4.0+

The AWS region to use when accessing AWS services.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_AMAZONSQS_REGION` |
| **App config key** | `LicensingComponent/AmazonSQS/Region` |

| Type | Default value |
| --- | --- |
| string | obtained from ServiceControl |

### LicensingComponent/AmazonSQS/Prefix

Version: 5.4.0+

Report only on queues that begin with the specified prefix. This is commonly used when one AWS account must contain queues for multiple projects or multiple environments.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_AMAZONSQS_PREFIX` |
| **App config key** | `LicensingComponent/AmazonSQS/Prefix` |

| Type | Default value |
| --- | --- |
| string | obtained from ServiceControl |

## Usage Reporting when using the RabbitMQ transport

> [!NOTE]
> Begining with version 6.5.0, these configuration settings are no longer used. Access to the management API is configured using [connection string options](/servicecontrol/transports.md#rabbitmq). See the [ServiceControl 6.4 to 6.5 upgrade guide](/servicecontrol/upgrades/6.4to6.5.md) for more information.

### LicensingComponent/RabbitMQ/ApiUrl

Version: 5.4.0 to 6.4.0

The RabbitMQ management URL.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_RABBITMQ_APIURL` |
| **App config key** | `LicensingComponent/RabbitMQ/ApiUrl` |

| Type | Default value |
| --- | --- |
| string | obtained from ServiceControl |

### LicensingComponent/RabbitMQ/UserName

Version: 5.4.0 to 6.4.0

The username to access the RabbitMQ management interface.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_RABBITMQ_USERNAME` |
| **App config key** | `LicensingComponent/RabbitMQ/UserName` |

| Type | Default value |
| --- | --- |
| string | obtained from ServiceControl |

### LicensingComponent/RabbitMQ/Password

Version: 5.4.0 to 6.4.0

The password to access the RabbitMQ management interface.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_RABBITMQ_PASSWORD` |
| **App config key** | `LicensingComponent/RabbitMQ/Password` |

| Type | Default value |
| --- | --- |
| string | obtained from ServiceControl |

## Usage Reporting when using the SqlServer transport

### LicensingComponent/SqlServer/ConnectionString

Version: 5.4.0+

The connection string that will provide at least read access to all queue tables.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_SQLSERVER_CONNECTIONSTRING` |
| **App config key** | `LicensingComponent/SqlServer/ConnectionString` |

| Type | Default value |
| --- | --- |
| string | obtained from ServiceControl |

### LicensingComponent/SqlServer/AdditionalCatalogs

Version: 5.4.0+

Specifies any additional databases on the same server that also contain NServiceBus message queues.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_SQLSERVER_ADDITIONALCATALOGS` |
| **App config key** | `LicensingComponent/SqlServer/AdditionalCatalogs` |

| Type | Default value |
| --- | --- |
| string | |

## Usage Reporting when using the PostgreSQL transport

### LicensingComponent/PostgreSQL/ConnectionString

Version: 5.10.0+

The connection string that will provide at least read access to all queue tables.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_POSTGRESQL_CONNECTIONSTRING` |
| **App config key** | `LicensingComponent/PostgreSQL/ConnectionString` |

| Type | Default value |
| --- | --- |
| string | obtained from ServiceControl |

## Plugin-specific

### ServiceControl/HeartbeatGracePeriod

The period that defines whether an endpoint is considered alive or not since the last received heartbeat.

Type: timespan

Default: `00:00:40` (40 secs)

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_HEARTBEATGRACEPERIOD` |
| **App config key** | `ServiceControl/HeartbeatGracePeriod` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| timespan | `00:00:40` (40 secs) |

When configuring the heartbeat grace period, make sure it is greater than the [heartbeat interval defined by the plugin](/monitoring/heartbeats/install-plugin.md).

> [!NOTE]
> When monitoring multiple endpoints, ensure that the heartbeat grace period is larger than any individual heartbeat interval set by the endpoints.

## Troubleshooting

### ServiceControl/DataSpaceRemainingThreshold

The percentage threshold for the [Message database storage space](/servicecontrol/servicecontrol-instances/#notifications-health-monitoring-message-database-storage-space) check. If the remaining hard drive space drops below this threshold (as a percentage of the total space on the drive), then the check will fail, alerting the user.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_DATASPACEREMAININGTHRESHOLD` |
| **App config key** | `ServiceControl/DataSpaceRemainingThreshold` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | 20 (percent) |

### ServiceControl/MinimumStorageLeftRequiredForIngestion

This setting was introduced in version 4.28. The percentage threshold for the [Critical message database storage space](/servicecontrol/servicecontrol-instances/#notifications-health-monitoring-critical-message-database-storage-space) check. If the remaining hard drive space drops below this threshold (as a percentage of the total space on the drive), then the check will fail, alerting the user. The message ingestion will also be stopped to prevent data loss. Message ingestion will resume once more disk space is made available.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MINIMUMSTORAGELEFTREQUIREDFORINGESTION` |
| **App config key** | `ServiceControl/MinimumStorageLeftRequiredForIngestion` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | 5 (percent) |
