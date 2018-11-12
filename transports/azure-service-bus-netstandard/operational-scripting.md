---
title: Operational Scripting
summary: Explains how to create queues and topics with the Azure Service Bus transport using scripting
component: ASBS
tags:
 - Azure
reviewed: 2018-06-21
---

## Operational Scripting

In order to provision or de-provision the resources required by an endpoint, the `asb-transport` command line (CLI) tool can be used.

The tool can be obtained from NuGet and installed using the following command:

```
dotnet tool install -g NServiceBus.Transport.AzureServiceBus.CommandLine
```

Once installed, the `asb-transport` command line tool will be available for use.

`asb-transport <command> [options]`

### Available commands

- `endpoint create`
- `queue create`
- `queue delete`

### asb-transport endpoint create

Create a new endpoint using:

```
asb-transport endpoint create [--size]
                              [--partitioned]
                              [--topic]
                              [--subscription]
```

#### options
 
`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-s` | `--size` : Queue size in GB (defaults to 5)

`-p` | `--partitioned`: Enable partitioning

`-t` | `--topic`: Topic name (defaults to 'bundle-1')

`-b` | `--subscription`: Subscription name (defaults to endpoint name)


### asb-transport queue create
 
Create a queue using:

```
asb-transport queue create [--size]
                           [--partitioned]
```

#### options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-s` | `--size`: Queue size in GB (defaults to 5)

`-p` | `--partitioned`: Enable partitioning


### asb-transport queue delete
 
Delete a queue using:

```
asb-transport queue delete
```

#### options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'
 