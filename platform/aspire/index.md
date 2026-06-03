---
title: Aspire
summary: Describes how to orchestrate the Particular Platform via the Aspire AppHost
reviewed: 2026-05-21
component: Aspire.Hosting
related:
- samples/hosting/aspire
- samples/aspire/platform
- samples/aspire/platform-asb
---

The `Particular.Aspire.Hosting.ServicePlatform` package is an [Aspire](https://aspire.dev/) hosting integration that runs the Particular Platform — the ServiceControl instances, ServicePulse, persistence, and message transport — as part of an Aspire AppHost. It is intended for developers and technical leads who run the platform locally during development and want the same AppHost to carry through to publish-mode deployments, without maintaining a separate set of infrastructure scripts.

## Overview

The `Particular.Aspire.Hosting.ServicePlatform` package extends an [Aspire](https://aspire.dev/) AppHost so it can run a complete Particular Platform alongside the rest of a distributed application.

A single `AddParticularPlatform(...)` call registers a platform resource that owns:

- [ServiceControl](/servicecontrol) error, audit, and monitoring instances
- [ServicePulse](/servicepulse)
- A managed RavenDB persistence instance, or one supplied by the AppHost
- The configured message transport (Learning, Azure Service Bus, or RabbitMQ)
- The platform license

Transport, persistence, and licensing are configured once on the platform resource and propagated to every component, so the containers start in the correct order. [NServiceBus endpoints](/nservicebus/endpoints/) attach with `WithParticularPlatform(...)` and pick up the same transport connection string and license without additional wiring.

snippet: aspire-apphost

## Supported components

The integration is being built out across the Particular Platform in stages. The tables below show what is currently wired through the AppHost.

If you are using the Particular Service Platform with Aspire today and would like to see more Aspire support, [reach out](https://github.com/Particular/NServiceBus/issues/6941).

### Transport

| Transport                                                 | Status                       |
| --------------------------------------------------------- | ---------------------------- |
| [Learning](/transports/learning/)                         | Supported (Development only) |
| [Azure Service Bus](/transports/azure-service-bus/)       | Supported                    |
| [RabbitMQ](/transports/rabbitmq/)                         | Supported                    |
| [Amazon SQS](/transports/sqs/)                            | Supported                    |
| [Azure Storage Queues](/transports/azure-storage-queues/) | Not yet supported            |
| [Microsoft SQL Server](/transports/sql/)                  | Not yet supported            |
| [PostgreSQL](/transports/postgresql/)                     | Not yet supported            |
| [IBM MQ](/transports/ibmmq/)                              | Not yet supported            |

### Persistence

| Persistence                          | Status    |
| ------------------------------------ | --------- |
| [RavenDB](/servicecontrol/ravendb/)  | Supported |

## Prerequisites

- The [Aspire CLI](https://aspire.dev/get-started/install-cli/) and the .NET 10 SDK, used to build and run the AppHost project.
- A container runtime such as [Docker Desktop](https://www.docker.com/products/docker-desktop/) or [Podman](https://podman.io/). The platform components are pulled from Docker Hub as `particular/servicecontrol`, `particular/servicecontrol-audit`, `particular/servicecontrol-monitoring`, `particular/servicecontrol-ravendb`, and `particular/servicepulse`.
- A Particular Platform license. See [Configuring the license](#configuring-the-license) for the license sources the integration accepts.

## Installation

If the solution does not yet have an Aspire AppHost project, follow the [Build your first Aspire app](https://aspire.dev/get-started/first-app/) quickstart to scaffold one.

Then add the integration package to the AppHost project:

```sh
dotnet add package Particular.Aspire.Hosting.ServicePlatform
```

Only the AppHost project needs this reference. NServiceBus endpoint projects pick up the platform license and transport connection string from environment variables injected by `WithParticularPlatform(...)`, so they do not need their own reference to this package.

## Quick start

`AddDefaultComponents()` wires up the Learning transport, a managed RavenDB persistence instance, the ServiceControl error, audit, and monitoring instances, and ServicePulse with sensible defaults:

snippet: aspire-quick-start-1

To attach an [NServiceBus endpoint](/nservicebus/endpoints/) so it picks up the platform's license and transport connection string, chain `WithParticularPlatform(platform)` on the endpoint's project resource:

snippet: aspire-quick-start-2

These defaults are intended for local development. For deployment scenarios, configure an explicit transport, persistence, and license as described in [Configuring the transport](#configuring-the-transport), [Configuring persistence](#configuring-persistence), and [Configuring the license](#configuring-the-license).

## Viewing the platform in the Aspire dashboard

The platform appears in the [Aspire dashboard](https://aspire.dev/dashboard/) as a single `ParticularPlatform` parent resource. The components the integration creates are nested as children, each surfacing the URL exposed by its primary endpoint:

- **ServicePulse**: the web UI
- **ServiceControl Error**: the error instance API
- **ServiceControl Audit**: the audit instance API
- **ServiceControl Monitoring**: the monitoring instance API
- **Management Studio**: the RavenDB management UI (when using the managed RavenDB persistence instance)

Externally supplied resources are not nested under the platform. An Azure Service Bus or RabbitMQ broker passed in via `AddConnectionString("...")` (or any other `Add*` call), and an existing RavenDB instance attached via `WithPersistenceRavenDb(existingRaven)`, appear as separate top-level resources in the dashboard. The platform does not own their lifecycle, so it does not group them under itself.

The platform node tracks readiness as its children come up. It starts in `Starting`, transitions to `Running` once every child reports healthy, and moves to `RuntimeUnhealthy` if any child stops.

## Configuring the transport

The platform uses whichever transport is configured via a `WithTransport*` extension on the platform resource. The same transport connection string is then propagated to every ServiceControl instance and to any NServiceBus endpoint attached via `WithParticularPlatform(...)`. See [Supported components](#supported-components) for the transports currently wired through the integration.

### Learning transport

The [Learning transport](/transports/learning/) stores messages on the local file system. By default they are persisted to `.learningtransport` under the AppHost project directory. Pass a custom path to override.

snippet: aspire-transport-learning

The Learning transport appears in the Aspire dashboard as a `learning-transport` connection-string resource nested under the platform, holding the resolved storage path.

> [!WARNING]
> The Learning transport is intended for development only and the AppHost will error during publish flows when it is included.

#### Options

| Option | Default |
| --- | --- |
| `storagePath` (`string?`) parameter on `WithTransportLearning` | `.learningtransport` (relative to the AppHost project directory) |

### Azure Service Bus

The platform treats Azure Service Bus as an external resource, so the AppHost only needs a connection string to reach the namespace. Other ways to model the resource in the AppHost (such as `AddAzureServiceBus` with Aspire provisioning, or `AsExisting`) are described in [Set up Azure Service Bus in the AppHost](https://aspire.dev/integrations/cloud/azure/azure-service-bus/azure-service-bus-host/) in the Aspire documentation.

snippet: aspire-transport-asb

#### Options

| Option | Default |
| --- | --- |
| `azureServiceBus` (`IResourceBuilder<IResourceWithConnectionString>`) parameter on `WithTransportAzureServiceBus` | Required |

Azure Service Bus tuning such as topic name, partitioning, hierarchy namespace, and websocket mode is configured on the connection string itself. See [Azure Service Bus transport configuration](/servicecontrol/transports.md#azure-service-bus) for the full list of connection-string options.

### RabbitMQ

The platform treats RabbitMQ as an external resource, so the AppHost only needs a connection string to reach the broker. Other ways to model the resource in the AppHost (such as `AddRabbitMQ` with Aspire provisioning) are described in [Set up RabbitMQ in the AppHost](https://aspire.dev/integrations/messaging/rabbitmq/rabbitmq-host/) in the Aspire documentation.

Pass the resource to `WithTransportRabbitMQ` along with a [routing topology](/transports/rabbitmq/routing-topology.md).

snippet: aspire-transport-rabbitmq

### Amazon SQS

The platform requires a pre-configured access token to connect to AmazonSQS, and these values will be passed to endpoints [via Environment Variables](/transports/sqs/#configuration). It is recommended that parameter resources are used for this.

snippet: aspire-transport-sqs

#### Options

| Option                                                                                                 | Default  |
| ------------------------------------------------------------------------------------------------------ | -------- |
| `region`                   (`string`) parameter on `WithTransportAmazonSQS`                            | Required |
| `accessKey`                (`IExpressionValue`) parameter on `WithTransportAmazonSQS`                  | Required |
| `secretAccessKey`          (`IExpressionValue`) parameter on `WithTransportAmazonSQS`                  | Required |
| `configure`                (`Action<AmazonSQSTransportSettings`) parameter on `WithTransportAmazonSQS` | Optional |
| `QueueNamePrefix`          (`string?`) property on `AmazonSQSTransportSettings`                        | Optional |
| `TopicNamePrefix`          (`string?`) property on `AmazonSQSTransportSettings`                        | Optional |
| `S3BucketForLargeMessages` (`IExpressionValue?`) property on `AmazonSQSTransportSettings`              | Optional |
| `QueueNamePrefix`          (`string?`) property on `AmazonSQSTransportSettings`                        | Optional |

## Configuring persistence

ServiceControl uses a persistence backend to store error and audit data, retry state, and saga history. See [Supported components](#supported-components) for the persisters currently wired through the integration.

Configure persistence on the platform resource, then pass the resulting persistence builder into the ServiceControl Error and Audit instances that need it. The Monitoring instance does not require persistence.

### RavenDB

RavenDB can be modelled as either a managed child of the platform (the integration provisions and starts the container) or as an external resource declared elsewhere in the AppHost.

#### Managed RavenDB instance

`AddPersistenceRavenDb(name)` adds the [`particular/servicecontrol-ravendb` container](/servicecontrol/ravendb/containers.md) as a child of the platform. This is the path used by `AddDefaultComponents()`.

snippet: aspire-persistence-ravendb-managed

#### External RavenDB instance

Use `WithPersistenceRavenDb(existing)` to attach a RavenDB resource declared elsewhere in the AppHost, for example via the [Aspire RavenDB integration](https://aspire.dev/integrations/databases/ravendb/ravendb-host/) or `AddConnectionString`.

snippet: aspire-persistence-ravendb-external

External RavenDB instances are not nested under the platform in the dashboard, since the platform does not own their lifecycle.

#### Options

| Option | Default |
| --- | --- |
| `name` (`string`) parameter on `AddPersistenceRavenDb` | Required |
| `persistence` (`IResourceBuilder<IResourceWithConnectionString>`) parameter on `WithPersistenceRavenDb` | Required |

## Configuring the license

`AddParticularPlatform` registers an Aspire parameter named `<platform-name>-license` (marked as a secret) to carry the Particular Platform license. The value is resolved as follows:

1. Any value supplied through the standard [Aspire parameter input mechanisms](https://aspire.dev/get-started/resources/) (user secrets, environment variables, command-line) takes precedence.
2. Otherwise the default set in code is used, which is either the built-in auto-discovery or whatever `WithLicenseFromFile` or `WithLicenseFromText` overrides it to.

### Default auto-discovery

If neither `WithLicenseFromFile` nor `WithLicenseFromText` is called, the integration searches the standard Particular Platform [license file locations](/nservicebus/licensing/#license-management) at AppHost startup in this order:

1. The machine-wide `%PROGRAMDATA%\ParticularSoftware\license.xml` file
2. The user-specific `%LOCALAPPDATA%\ParticularSoftware\license.xml` file
3. The `PARTICULARSOFTWARE_LICENSE` environment variable

This makes local development work without explicit configuration on machines where the license is already installed.

### From a file

Use `WithLicenseFromFile(file)` to read the license from a specific file path.

snippet: aspire-license-file

### From inline text

Use `WithLicenseFromText(licenseText)` to inline the license XML directly. This is useful in tests or when reading the license through `IConfiguration`.

snippet: aspire-license-text

### Options

| Option | Default |
| --- | --- |
| `file` (`string`) parameter on `WithLicenseFromFile` | Required |
| `licenseText` (`string`) parameter on `WithLicenseFromText` | Required |

## Adding platform components individually

`AddDefaultComponents()` is the easiest way to wire up the Error, Audit, Monitoring, and ServicePulse instances. When custom queue names, partial topologies (for example, an Error instance without an Audit), or multiple remote audit instances are required, the components can be added one at a time.

Each `Add*` method below is an extension on the platform resource builder. Every component automatically receives the platform license and transport configuration. Error and Audit additionally receive the persistence resource.

snippet: aspire-components-explicit

### ServiceControl Error instance

`AddServiceControlErrorInstance(name, persistence)` adds a [ServiceControl Error instance](/servicecontrol/servicecontrol-instances/), running the [`particular/servicecontrol`](https://hub.docker.com/r/particular/servicecontrol) image, as a child of the platform. The supplied persistence must have been registered on the platform via a `WithPersistence*` or `AddPersistence*` extension.

The error queue name defaults to `error`. Override it with `WithErrorQueueName(queueName)`. Use `WithThroughputQueue(queueName)` to override the queue on which throughput data is received from the monitoring instance. This is independent of throughput reporting, which separately configures broker-statistics querying.

snippet: aspire-components-error

#### Options

| Option | Default |
| --- | --- |
| `name` (`string`) parameter on `AddServiceControlErrorInstance` | Required |
| `persistence` (`IResourceBuilder<IResource>`) parameter on `AddServiceControlErrorInstance` | Required |
| `queueName` (`string`) parameter on `WithErrorQueueName` | `error` |
| `queueName` (`string`) parameter on `WithThroughputQueue` | `ServiceControl.ThroughputData` |
| `runMode` (`PlatformRunMode`) parameter on `WithRunMode` | `SetupAndRun` |

### ServiceControl Audit instance

`AddServiceControlAuditInstance(name, error, persistence)` adds a [ServiceControl Audit instance](/servicecontrol/audit-instances/), running the [`particular/servicecontrol-audit`](https://hub.docker.com/r/particular/servicecontrol-audit) image, as a child of the platform. The audit instance is automatically registered as a remote instance on the supplied error instance, so the error instance can query audit data through it.

The audit queue name defaults to `audit`. Override it with `WithAuditQueueName(queueName)`.

snippet: aspire-components-audit

#### Options

| Option | Default |
| --- | --- |
| `name` (`string`) parameter on `AddServiceControlAuditInstance` | Required |
| `serviceControl` (`IResourceBuilder<ServiceControlErrorInstanceResource>`) parameter on `AddServiceControlAuditInstance` | Required |
| `persistence` (`IResourceBuilder<IResource>`) parameter on `AddServiceControlAuditInstance` | Required |
| `queueName` (`string`) parameter on `WithAuditQueueName` | `audit` |
| `runMode` (`PlatformRunMode`) parameter on `WithRunMode` | `SetupAndRun` |

### ServiceControl Monitoring instance

`AddServiceControlMonitoringInstance(name)` adds a [ServiceControl Monitoring instance](/servicecontrol/monitoring-instances/), running the [`particular/servicecontrol-monitoring`](https://hub.docker.com/r/particular/servicecontrol-monitoring) image, as a child of the platform. The monitoring instance does not require a persistence resource.

The monitoring queue name defaults to `Particular.Monitoring`. Override it with `WithMonitoringQueueName(queueName)`. Use `WithThroughputQueueFrom(error)` to copy the throughput queue name from the error instance and keep the two aligned without repeating the name; `WithThroughputQueue(queueName)` sets it explicitly when the queue address needs to differ from the error instance.

snippet: aspire-components-monitoring

#### Options

| Option | Default |
| --- | --- |
| `name` (`string`) parameter on `AddServiceControlMonitoringInstance` | Required |
| `queueName` (`string`) parameter on `WithMonitoringQueueName` | `Particular.Monitoring` |
| `errorInstance` (`IResourceBuilder<ServiceControlErrorInstanceResource>`) parameter on `WithThroughputQueueFrom` | Required (when called) |
| `queueName` (`string`) parameter on `WithThroughputQueue` | `ServiceControl.ThroughputData` |
| `runMode` (`PlatformRunMode`) parameter on `WithRunMode` | `SetupAndRun` |

### ServicePulse

`AddServicePulse(name, servicecontrol, monitoring?)` adds [ServicePulse](/servicepulse/), running the [`particular/servicepulse`](https://hub.docker.com/r/particular/servicepulse) image, as a child of the platform. The error instance is wired in as the ServiceControl API endpoint. The optional monitoring argument exposes real-time monitoring data in the UI; if omitted, the monitoring panel is unavailable.

snippet: aspire-components-servicepulse

#### Options

| Option | Default |
| --- | --- |
| `name` (`string`) parameter on `AddServicePulse` | Required |
| `serviceControl` (`IResourceBuilder<ServiceControlErrorInstanceResource>`) parameter on `AddServicePulse` | Required |
| `monitoring` (`IResourceBuilder<ServiceControlMonitoringInstanceResource>?`) parameter on `AddServicePulse` | `null` |

## Using standard Aspire extensions

Each container the integration adds (the ServiceControl Error, Audit, and Monitoring instances, ServicePulse, and the managed RavenDB persistence instance) is a regular Aspire [container resource](https://aspire.dev/get-started/resources/), and `AddParticularPlatform` returns a standard resource builder. Any extension method that applies to those base types can be chained with the integration's own `Add*` and `With*` methods. Common examples include:

- `WithImageTag(tag)` to pin a specific image version
- `WithImageRegistry(registry)` to pull from a private registry mirror
- `WithEnvironment(name, value)` to pass through extra environment variables
- `WithVolume(...)` or `WithBindMount(...)` to map storage from the host
- `ExcludeFromManifest()` to keep a resource out of publish output
- `WaitFor(other)` and `WaitForCompletion(other)` to gate startup ordering

See the Aspire documentation on [resources](https://aspire.dev/get-started/resources/) for the full set of container-resource extensions.

## Connecting NServiceBus endpoints to the platform

`WithParticularPlatform(platform)` is an extension on any Aspire resource that exposes environment variables (typically a project resource added with `AddProject<T>(...)`). It injects the platform license and the transport connection string into the resource's environment, so an [NServiceBus endpoint](/nservicebus/endpoints/) reading those values picks up the same license and transport that the platform components use, without further wiring in the AppHost.

snippet: aspire-endpoint-connect

The endpoint project still configures NServiceBus itself: choosing the transport package (`NServiceBus.Transport.AzureServiceBus`, `NServiceBus.Transport.RabbitMQ`, and so on), reading the connection string from `IConfiguration` or environment, and setting up routing, persistence, and serialization as usual. `WithParticularPlatform` only delivers the platform-side configuration; it does not replace NServiceBus configuration code in the endpoint.

### Environment variables injected

`WithParticularPlatform(platform)` adds two pieces of configuration to the endpoint resource:

| Variable                                | Source                                                                  |
| --------------------------------------- | ----------------------------------------------------------------------- |
| `PARTICULARSOFTWARE_LICENSE`            | The platform license parameter                                          |
| `ConnectionStrings__<transport-name>`   | The transport connection-string resource passed to a `WithTransport*` call |

NServiceBus picks up `PARTICULARSOFTWARE_LICENSE` automatically at endpoint startup, as described in [License management](/nservicebus/licensing/#license-management). For the transport, read the connection string from `IConfiguration.GetConnectionString("<transport-name>")` and pass it to the transport configuration in the usual way.

The `<transport-name>` portion of the connection-string variable matches the name passed to `AddConnectionString(...)` (or whichever resource was supplied to `WithTransport*`). For the Learning transport the integration creates an internal connection-string resource named `learning-transport`, so endpoints receive `ConnectionStrings__learning-transport`.

### Waiting for the platform

`WithParticularPlatform` does not add a startup-order dependency on the platform. To delay the endpoint's startup until the platform reports `Running`, chain `WaitFor(platform)`:

snippet: aspire-endpoint-waitfor

### Options

| Option | Default |
| --- | --- |
| `platform` (`IResourceBuilder<ParticularPlatformResource>` or `ParticularPlatformResource`) parameter on `WithParticularPlatform` | Required |

## Throughput reporting

Throughput reporting lets the ServiceControl Error instance query the broker's management API to collect queue throughput statistics for licensing. The credentials required to make those queries depend on the transport. See [Supported components](#supported-components) for the transports the integration currently supports for throughput reporting, and the upstream [Usage reporting](/servicecontrol/servicecontrol-instances/configuration.md#usage-reporting-when-using-servicecontrol) docs for what ServiceControl does with the values.

To enable it on the ServiceControl Error instance, chain `WithThroughputReporting(provider)` and pass a provider matching the configured transport. The provider throws at configuration time if it is paired with a different transport.

### Azure Service Bus

`ThroughputReportingAzureServiceBus` supplies Azure AD credentials so the Error instance can query Service Bus management APIs. Pass each value as an Aspire parameter; mark the client secret as a secret parameter.

snippet: aspire-throughput-asb

See [Usage reporting when using the Azure Service Bus transport](/servicecontrol/servicecontrol-instances/configuration.md#usage-reporting-when-using-the-azure-service-bus-transport) for what each value is used for in ServiceControl.

### RabbitMQ

`ThroughputReportingRabbitMq` supplies RabbitMQ management API credentials so the Error instance can query broker statistics. All parameters are optional; ServiceControl falls back to the broker connection string for any value not supplied.

snippet: aspire-throughput-rabbitmq

See [Usage reporting when using the RabbitMQ transport](/servicecontrol/servicecontrol-instances/configuration.md#usage-reporting-when-using-the-rabbitmq-transport) for what each value is used for in ServiceControl.

### Amazon SQS

`ThroughputReportingAmazonSQS` supplies AmazonSQS overrides so the Error instance can query broker statistics. All parameters are optional; ServiceControl falls back to the values from the transport connection if they are not provided.

snippet: aspire-throughput-sqs

See [Usage reporting when using the Amazon SQS transport](/servicecontrol/servicecontrol-instances/configuration.md#usage-reporting-when-using-the-amazon-sqs-transport) for what each value is used for in ServiceControl.

## Production considerations

The same AppHost code is intended to run locally and deploy to production. The notes below highlight what to review before promoting the AppHost to publish or deploy mode.

### Learning transport in publish mode

The Learning transport is blocked in publish mode by design. Calling `WithTransportLearning` from an AppHost run with `aspire publish` or `aspire deploy` throws at startup. Configure a production-grade transport for non-development deployments. See [Configuring the transport](#configuring-the-transport) for the supported options.

### Supplying the license in production

`AddParticularPlatform` registers the license as a secret Aspire parameter. The in-code defaults set by `WithLicenseFromFile`, `WithLicenseFromText`, and the built-in auto-discovery are evaluated when the manifest is generated and the resulting value is embedded into the published manifest. Embedding a production license in the manifest is rarely desirable.

For production deployments, supply the `<platform-name>-license` parameter through the standard Aspire parameter input mechanisms (user secrets, environment variables, command-line arguments, or a secret-store integration) so the value never lives in the published manifest. The in-code defaults remain useful for local development.

### Configuring host ports

Each managed container exposes an endpoint allowing Aspire to manage the exposed endpoint to avoid conflicts with ports already in use by other processes. As these ports are selected randomly by Aspire at startup you can refer to the dashboard for the url to access each component.

In some cases it may be desirable to have a fixed port exposed by these components to provide a stable url (for example `http://localhost:9090` for ServicePulse) without having to inspect the Aspire dashboard.

Override a port with the standard Aspire `WithEndpoint(endpointName, callback)` API:

snippet: aspire-host-ports

### Pinning container image versions

The integration uses the `latest` tag for every managed container image (`particular/servicecontrol`, `particular/servicecontrol-audit`, `particular/servicecontrol-monitoring`, `particular/servicecontrol-ravendb`, `particular/servicepulse`). Pin each component to a specific version for production so deployments are reproducible and ServiceControl picks up new major versions intentionally.

The ServiceControl Error, Audit, Monitoring, and RavenDB versions must align. See [Managing ServiceControl RavenDB instances via Containers](/servicecontrol/ravendb/containers.md) for the version-pairing rules.

snippet: aspire-image-pinning

### Container run mode

By default, each ServiceControl instance container runs in `SetupAndRun` mode: on startup it performs setup, creating the queues and database structures it needs, and then runs the instance. This keeps the local development experience working without any manual preparation.

In production, setup is often performed as a separate, controlled step rather than on every container start, for example because the runtime account is not permitted to create queues, or to avoid setup running on each scaled-out replica. Use `WithRunMode(PlatformRunMode.Run)` to start an instance without performing setup, assuming the queues and database structures already exist:

snippet: aspire-run-mode

The available modes are:

- `PlatformRunMode.SetupAndRun` (default): perform setup, then run the instance.
- `PlatformRunMode.Run`: run the instance without performing setup.
- `PlatformRunMode.Setup`: perform setup only, then exit. This is useful for running setup as a dedicated step before starting the instances in `Run` mode.

`WithRunMode` is configured per ServiceControl instance, so the Error, Audit, and Monitoring instances can each use a different mode when required.

## Publishing and deploying

The integration's components are standard Aspire resources, so they participate in `aspire publish` and `aspire deploy` like any other resource. The synthetic platform parent resource is excluded from the publish manifest, but every component it owns (the ServiceControl instances, ServicePulse, and the managed RavenDB persistence instance) is included as a normal container resource in the published output. Externally supplied resources, such as an Azure Service Bus broker passed in via `AddConnectionString`, appear as they would in any other Aspire AppHost.

Aspire supports multiple deployment targets, including [Docker Compose](https://aspire.dev/deployment/docker-compose/), Kubernetes, and Azure Container Apps. See [Aspire pipelines](https://aspire.dev/deployment/pipelines/) for the deployment model and the supported targets.

Two integration-specific points to keep in mind for any publish target:

- The license parameter default is materialized into the manifest at publish time. Supply the license through Aspire parameter inputs in production so the value does not live in the published output. See [Supplying the license in production](#production-considerations-supplying-the-license-in-production).
- The Learning transport is blocked in publish mode.

## Troubleshooting

### "No transport configured" exception at startup

`AddParticularPlatform` requires a transport to be configured via one of the `WithTransport*` extensions before any platform component can be wired. Call `WithTransportLearning()`, `WithTransportAzureServiceBus(asb)`, or `WithTransportRabbitMQ(routing, rabbit)` on the platform builder. Alternatively, call `AddDefaultComponents()`, which falls back to the Learning transport when no transport has been set.

### Platform stays in `RuntimeUnhealthy` with no children

If `AddParticularPlatform` is called but no platform components are added, the integration marks the platform as `RuntimeUnhealthy` at startup and logs a warning that no child resources were added. Add components individually (see [Adding platform components individually](#adding-platform-components-individually)) or call `AddDefaultComponents()` to wire the standard topology.

### ServicePulse shows the monitoring panel as unavailable

`AddServicePulse` accepts an optional monitoring instance. When the monitoring argument is omitted, or `WithMonitoringInstance(null)` is called on the ServicePulse builder, ServicePulse runs without the monitoring panel. Pass an `IResourceBuilder<ServiceControlMonitoringInstanceResource>` to `AddServicePulse`, or call `WithMonitoringInstance(monitoring)` on the ServicePulse builder, to enable it. (`AddDefaultComponents()` always wires monitoring in automatically.)

### "Platform [name] has mismatched ServiceControl container image versions" in Aspire host console

This warning appears when the ServiceControl container images (error, audit, and monitoring instances) are configured with different version tags. For example:

snippet: aspire-components-version

 All ServiceControl components (error, audit, and monitoring) must run the same version to ensure compatibility. The warning lists each component with its configured image and tag so the mismatch is easy to identify.

To resolve the warning, ensure all ServiceControl instances use the same image tag, or remove explicit `.WithImage()` calls to use the default `latest` tag, which always refers to a compatible set of images.

 > [!NOTE]
 > This check only applies to ServiceControl instances. The ServicePulse image follows its own versioning and is not included in this validation.
