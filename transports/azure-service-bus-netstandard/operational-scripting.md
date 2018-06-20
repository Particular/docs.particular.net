---
title: Operational scripting
summary: Explains how to create queues and topics using scripting
component: ASBS
tags:
 - Azure
reviewed: 2018-06-20
---

## Operational Scripting

In order to provision or deprovision the resources required by an endpoint, the `asb-transport` command line (cli) tool can be used.

`asb-transport <command> [options]`

### Available commands

- `endpoint`
- `queue`

#### options

`-c` | `--connection-string`:  Connection string to the Azure Service Bus namespace (defaults to value from environment variable 'x')

### asb-transport endpoint create

Create a new endpoint using

```
asb-transport endpoint create [--size]
                              [--partitioned]
                              [--topic]
                              [--subscription]
```

#### options
 
`-s` | `--size` : Queue size in GB (defaults to 5)

`-p` | `--partitioned`: Enable partitioning

`-t` | `--topic`: Topic name (defaults to 'bundle-1')

`-b` | `--subscription`: Subscription name (defaults to endpoint name)

### asb-transport queue create
 
Create a queue using

```
asb-transport queue create [--size]
                           [--partitioned]
```

#### options

`-s` | `--size`: Queue size in GB (defaults to 5)

`-p` | `--partitioned`: Enable partitioning

### asb-transport queue delete
 
Delete a queue using

```
asb-transport queue delete
```
 