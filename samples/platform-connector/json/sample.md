---
title: Connect to ServicePlatform with JSON configuration
summary: A sample that shows how to connect an NServiceBus endpoint to the Particular Service Platform using the JSON schema
reviewed: 2024-01-25
component: PlatformConnector
related:
 - platform/connecting
---

## Introduction

This sample connects an NServiceBus endpoint to the Particular Service Platform and configures:

- An error queue
- Message auditing
- Saga auditing
- Endpoint heartbeats
- Custom checks
- Performance metrics

## Code walk-through

### Endpoint

A basic NServiceBus endpoint containing a saga, a handler, and a custom check.

The endpoint loads connection details from a file.

snippet: loadConnectionDetails

The connection details are used to configure all of the Particular Service Platform features.

snippet: configureConnection

Features can be turned on and off, and configured, by adjusting the configuration file at deployment.

#### Endpoint features

The endpoint contains:

- A saga that processes messages triggered by the user, sends a request to a message handler, and waits for a result before marking the saga instance as complete. Connect ServiceInsight to the ServiceControl instance created by PlatformLauncher to view saga audit data.
- A custom check that toggles the state between success and failure every 30 seconds. Check the Custom Checks tab in ServicePulse to see failures reported here.
- A message handler that waits half a second before returning a response. This simulates real-world message processing in the Monitoring tab of ServicePulse.

### PlatformLauncher

Sets up three instances of ServiceControl (Primary, Audit, and Monitoring) and runs ServicePulse connected to all three.

## Running the sample

Run the sample. Once running, press <kbd>Esc</kbd> to quit or any other key to send messages. Each message will trigger a saga, which will send a request message to a message handler and wait for a response.

Note the ServiceControl API address in the PlatformLauncher window to connect ServiceInsight to the sample and view message audit and saga audit details.
