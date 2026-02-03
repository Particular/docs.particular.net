---
title: Operational Scripting
summary: Explains how to create queues and topics with the Azure Service Bus transport using scripting
component: ASBS
reviewed: 2025-02-17
related:
- transports/azure-service-bus/configuration
---

## Operational Scripting

In order to provision or de-provision the resources required by an endpoint, the `asb-transport` command line (CLI) tool can be used.

The tool can be obtained from NuGet and installed using the following command:

```
dotnet tool install -g NServiceBus.Transport.AzureServiceBus.CommandLine
```

Once installed, the `asb-transport` command line tool will be available for use.

`asb-transport <command> [options]`

## Available commands

- `endpoint create`
- `endpoint subscribe`
- `endpoint unsubscribe`
- `queue create`
- `queue delete`

partial: endpoint-command

### asb-transport queue create

Create a queue using:

```
asb-transport queue create name
                              [--size]
                              [--partitioned]
```

#### options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-s` | `--size`: Queue size in GB (defaults to 5)

`-p` | `--partitioned`: Enable partitioning


### asb-transport queue delete

Delete a queue using:

```
asb-transport queue delete name
```

#### options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

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
