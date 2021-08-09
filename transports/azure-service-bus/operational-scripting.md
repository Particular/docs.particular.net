---
title: Operational Scripting
summary: Explains how to create queues and topics with the Azure Service Bus transport using scripting
component: ASBS
reviewed: 2021-07-29
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
                              [--subscription]
```

#### options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-s` | `--size` : Queue size in GB (defaults to 5)

`-p` | `--partitioned`: Enable partitioning

`-t` | `--topic`: Topic name (defaults to 'bundle-1')

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

`-t` | `--topic`: Topic name (defaults to 'bundle-1')

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

`-t` | `--topic`: Topic name (defaults to 'bundle-1')

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

`-s` | `--size`: Queue size in GB (defaults to 5)

`-p` | `--partitioned`: Enable partitioning


### asb-transport queue delete

Delete a queue using:

```
asb-transport queue delete name
```

#### options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

### Examples

#### Provisioning the audit and the error queues

```
asb-transport queue create audit -c "<connection-string>"
asb-transport queue create error -c "<connection-string>"
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