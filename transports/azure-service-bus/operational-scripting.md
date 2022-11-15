---
title: Operational Scripting
summary: Explains how to create queues and topics with the Azure Service Bus transport using scripting
component: ASBS
reviewed: 2022-11-15
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
- `endpoint subscribe`
- `endpoint unsubscribe`
- `queue create`
- `queue delete`

### asb-transport endpoint create

Create a new endpoint using:

```
asb-transport endpoint create name
                              [--size]
                              [--partitioned]
                              [--topic]
                              [--topic-to-publish-to] [--topic-to-subscribe-on]
                              [--subscription]
```

#### options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace for connecting with cached credentials, such as those from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-s` | `--size` : Queue size in GB (defaults to 5)

`-p` | `--partitioned`: Enable partitioning

`-t` | `--topic`: Topic name (defaults to 'bundle-1')

`-tp` | `--topic-to-publish-to`: The topic name to publish to.

`-ts` | `--topic-to-subscribe-on`: The topic name to subscribe on.

`-b` | `--subscription`: Subscription name (defaults to endpoint name)

### asb-transport endpoint subscribe

Create a new subscription for an endpoint using:

```
asb-transport endpoint subscribe name event-type
                              [--topic]
                              [--subscription]
                              [--rule-name]
```

#### Options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace for connecting with cached credentials, such as those from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-t` | `--topic`: Topic name to subscribe on (defaults to 'bundle-1')

`-b` | `--subscription`: Subscription name (defaults to endpoint name)

`-r` | `--rule-name`: Rule name (defaults to event type)

### asb-transport endpoint unsubscribe

Delete a subscription for an endpoint using:

```
asb-transport endpoint unsubscribe name event-type
                              [--topic]
                              [--subscription]
                              [--rule-name]
```

#### Options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace for connecting with cached credentials, such as those from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-t` | `--topic`: Topic name to unsubscribe from (defaults to 'bundle-1')

`-b` | `--subscription`: Subscription name (defaults to endpoint name)

`-r` | `--rule-name`: Rule name (defaults to event type)

### asb-transport queue create

Create a queue using:

```
asb-transport queue create name
                              [--size]
                              [--partitioned]
```

#### options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace for connecting with cached credentials, such as those from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-s` | `--size`: Queue size in GB (defaults to 5)

`-p` | `--partitioned`: Enable partitioning


### asb-transport queue delete

Delete a queue using:

```
asb-transport queue delete name
```

#### options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace for connecting with cached credentials, such as those from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

### Examples

#### Provisioning the audit and the error queues

```
asb-transport queue create audit -c "<connection-string>"
asb-transport queue create error -c "<connection-string>"
```

#### Using connection strings

```
asb-transport [command] [subcommand] -c "<connection-string>"
```

#### Using cached credentials

```
asb-transport [command] [subcommand] -n "somenamespace.servicebus.windows.net"
```

#### Provisioning endpoints

Create the topology for an endpoint named `MyEndpoint` using the default settings:

```
asb-transport endpoint create MyEndpoint -c "<connection-string>"
```

Create the topology for an endpoint named `MyEndpoint` and override the topic name to be `custom-topic` and the subscription name to be `my-endpoint`:

```
asb-transport endpoint create MyEndpoint -t custom-topic -s my-endpoint -c "<connection-string>"
```

Create the topology for an endpoint named `MyEndpoint` and override the publish topic name to be `custom-publish-topic` and the subscription topic name to be `custom-subscribe-topic`:

```
asb-transport endpoint create MyEndpoint -tp custom-publish-topic -ts custom-subscribe-topic -c "<connection-string>"
```

#### Subscribing to events

Subscribe `MyOtherEndpoint` to the event `Contracts.Events.SomeEvent` using the default settings:

```
asb-transport endpoint subscribe MyOtherEndpoint Contracts.Events.SomeEvent -c "<connection-string>"
```

Subscribe `MyOtherEndpoint` to the event `Contracts.Events.SomeEvent` and override the topic name to be `custom-topic`:

```
asb-transport endpoint subscribe MyOtherEndpoint Contracts.Events.SomeEvent -t custom-topic -c "<connection-string>"
```

Subscribe `MyOtherEndpoint` to the event `Contracts.Events.SomeEvent` and override the subscription name to be `my-other-endpoint`

```
asb-transport endpoint subscribe MyOtherEndpoint Contracts.Events.SomeEvent -s my-other-endpoint -c "<connection-string>"
```

Subscribe `MyOtherEndpoint` to the event `Contracts.Events.SomeEvent` and override the subscription rule name to be `SomeEvent`:

```
asb-transport endpoint subscribe MyOtherEndpoint Contracts.Events.SomeEvent -r SomeEvent -c "<connection-string>"
```