---
title: Operational Scripting
summary: Explains how to create queues and topics with the Azure Service Bus transport using scripting
component: ASBS
reviewed: 2026-09-22
related:
- transports/azure-service-bus/configuration
---

## Operational Scripting

In order to provision or de-provision the resources required by an endpoint, a command line (CLI) tool called `asb-transport` is provided. 

The tool can be obtained from NuGet and installed using the following command:

```
dotnet tool install -g NServiceBus.Transport.AzureServiceBus.CommandLine
```

Once installed, the `asb-transport` command line tool will be available for use.
This tool accepts commands with the shape:

```
asb-transport <command> [options]`
```

## Connection string
All of the supported commands will obtain the connection string from the `AzureServiceBus_ConnectionString` environment variable if no other value is provided.

## Available commands

partial: endpoint-command

partial: queue-command

partial: migration-endpoint-command

## Examples

### Provisioning the audit and the error queues

```
asb-transport queue create audit -c "<connection-string>"
asb-transport queue create error -c "<connection-string>"
```

### Using connection strings

```
asb-transport [command] [subcommand] -c "<connection-string>"
```

### Using cached credentials

```
asb-transport [command] [subcommand] -n "somenamespace.servicebus.windows.net"
```

partial: examples
