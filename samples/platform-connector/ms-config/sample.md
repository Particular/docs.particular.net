---
title: Connect to ServicePlatform using the .NET Generic Host
summary: A sample that shows how to connect an NServiceBus endpoint hosted in the .NET Generic Host to the Particular Service Platform
reviewed: 2024-01-26
component: PlatformConnector
related:
 - platform/connecting
---

## Introduction

This sample connects an NServiceBus endpoint hosted in the .NET Generic Host to the Particular Service Platform and configures:

- An error queue
- Message auditing
- Saga auditing
- Endpoint heartbeats
- Custom checks
- Performance metrics

## Code walk-through

### Endpoint

A basic NServiceBus endpoint containing a saga, a handler, and a custom check. The endpoint is hosted in a [.NET Generic Host](https://docs.microsoft.com/en-us/dotnet/core/extensions/generic-host).

The host is configured to include a JSON configuration file. The configuration can come from [any provider](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration-providers) supported by the configuration framework.

snippet: addConfigFile

The endpoint loads connection details from a configuration section.

snippet: loadConnectionDetails

The connection details are used to configure all of the Particular Service Platform features.

snippet: configureConnection

Features can be turned on and off, and configured, by adjusting the configuration at deployment.

snippet: appsettings

Note that configuration details are stored in a section called `ServicePlatformConfiguration` which is the same key used above to retrieve them.

#### Endpoint features

The endpoint contains:

- A saga that processes messages triggered generated 5 times per second, sends a request to a message handler, and waits for a result before marking the saga instance as complete. Connect ServiceInsight or ServicePulse to the ServiceControl instance created by PlatformLauncher to view saga audit data.
- A custom check that toggles the state between success and failure every 30 seconds. Check the Custom Checks tab in ServicePulse to see failures reported here.
- A message handler that waits half a second before returning a response. This simulates real-world message processing in the Monitoring tab of ServicePulse.

### PlatformLauncher

Sets up three instances of ServiceControl (Primary, Audit, and Monitoring) and runs ServicePulse connected to all three.

## Running the sample

Run the sample. Once running, a message is generated once every 200ms. Each message will trigger a saga, which will send a request message to a message handler and wait for a response.

Note the ServiceControl API address in the PlatformLauncher window to connect ServiceInsight or ServicePulse to the sample and view message audit and saga audit details.
