---
title: Particular Platform with Amazon SQS in Aspire
summary: Orchestrating the Particular Platform with Amazon SQS transport via Aspire
component: Core
reviewed: 2026-05-07
---

[Aspire](https://aspire.dev/) is a stack for developing distributed applications provided by Microsoft.

This sample shows an Aspire AppHost project that orchestrates multiple NServiceBus endpoints,  wiring up the required infrastructure pieces when using the [Amazon SQS](/transports/sqs/) transport.

This sample does not include any AWS resource deployments as it is not required to demonstrate the Particular Platform running within aspire. When this sample is run the Platform and Endpoints will generate the required queues and topics in SQS. Refer to the [Integrating AWS with .NET Aspire in the AWS SDK for .NET documentation](https://docs.aws.amazon.com/sdk-for-net/v4/developer-guide/aspire-integrations.html) page for more information on AWS resources within Aspire.

## Running the sample

1. Run the AspireDemo.AppHost project
2. Open the Aspire dashboard
3. Review the metrics, traces, and structured log entries of each of the resources

> [!NOTE]
> This sample requires [Docker](https://www.docker.com/) to run. Ensure the predefined container ports are free and available.

## Code walkthrough

### AspireDemo.AppHost

The [Aspire orchestration project](https://aspire.dev/get-started/app-host/?lang=csharp) defines multiple resources and the relationships between them:

- Parameters to configure the sample
  - `accessKey` - configurable after startup
  - `accessSecret` - configurable after startup
- Two projects, each of which is an NServiceBus endpoint. All of these projects reference the `platform` resource.
  - `clientui`
  - `sales`
- ServiceControl error, audit and monitoring instances
- ServicePulse

#### Platform configuration

- `region`
- `resourceNamePrefix` - change this to avoid resource conflicts in your region
- `accessKey` - configurable after startup
- `accessSecret` - configurable after startup

`AddParticularPlatform` registers the Particular Platform as a resource named `particular`. The `WithTransportAmazonSqs` extension points the platform at the SQS configuration defined earlier, so that the ServiceControl instances connect to the same AWS region as the endpoints.

`AddDefaultComponents` registers the remaining platform components using their default configuration — the ServiceControl audit and monitoring instances and ServicePulse. The error instance is added explicitly above so that usage reporting can be configured on it.

snippet: platform-config

#### Endpoints

Each NServiceBus endpoint is added as an Aspire project and linked to the platform with `WithParticularPlatform`. The `ClientUI` endpoint additionally uses `WaitFor(sales)` so that the `Sales` endpoint exists before it starts sending messages to it.

The Aspire hosting component will automatically pass the transport authentication settings to the endpoints as [environment variables](/transports/sqs/#configuration), so that the NServiceBus transport can authenticate with AWS without additional configuration.

The `resourceNamePrefix` is also passed as `RESOURCE_NAME_PREFIX`, so that the endpoints can use it when configuring the transport and when sending messages to ensure they are using the correct queues. 

snippet: endpoints

### AspireDemo.ServiceDefaults

The [Aspire service defaults](https://aspire.dev/get-started/csharp-service-defaults/) project provides extension methods to configure application hosts and NServiceBus endpoints in a standardized way. This project is referenced by all of the NServiceBus endpoint projects.

The OpenTelemetry configuration has been updated to include NServiceBus metrics and traces.

snippet: add-nsb-otel

Each endpoint project creates an Amazon SQS transport and sets the queue prefix to match the AppHost configuation, and configures NServiceBus to use it as a transport relying on the `AWS_*` environment variables added by `WithParticularPlatform` to authenticate with AWS:

snippet: transport-config

Finally, each endpoint enables NServiceBus installers. Every time the application host is run, the transport and persistence database are recreated and will not contain the queues and tables needed for the endpoints to run. Enabling installers allows NServiceBus to set up the assets that it needs at runtime.

snippet: enable-installers

### Endpoint projects

Each of the endpoint projects contain the same code to create an application host, apply the configuration from the ServiceDefaults project on the NServiceBus endpoint.

snippet: endpoint-config

To demonstrate the platform's error handling, the `Sales` endpoint's handler throws an exception for a random subset of the messages it receives:

snippet: random-error

Failed messages are moved to the error queue, where they can be inspected and retried from ServicePulse.

If you're missing certain capabilities to use Aspire with NServiceBus, [share them and help shape the future of the platform](/shape-the-future/aspire.md).