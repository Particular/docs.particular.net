---
title: Security and permissions
summary: IBM MQ authorities required by the transport for queue, topic, and subscription operations
reviewed: 2026-03-26
component: IBMMQ
related:
- transports/ibmmq/topology
- transports/ibmmq/operations-scripting
---

The IBM MQ transport requires specific queue manager authorities depending on the endpoint's role and whether infrastructure setup (installers) is enabled. Use `setmqaut`, the IBM MQ Explorer, or equivalent tooling to grant the required authorities.

## Infrastructure setup permissions

> [!NOTE]
> These permissions are only required when `EnableInstallers()` is called. In production, prefer pre-creating resources using the [command-line tool](operations-scripting.md) or native `runmqsc` scripts and running endpoints without installers.

When `EnableInstallers()` is called, the endpoint creates IBM MQ resources at startup using PCF commands. This requires elevated administrative authorities **in addition to** the runtime permissions below.

|Operation|Object type|Object|Authority|
|:---|---|---|---|
|Create queues|Queue manager|`QMGR`|`crt`|
|Create topics|Queue manager|`QMGR`|`crt`|

The following resources are created automatically:

|Resource|Created by|
|:---|---|
|Endpoint input queue|The endpoint that owns it|
|Error queue|Any endpoint configured to send to it|
|Send destination queues|The sending endpoint|
|Topic objects|Any endpoint with explicit `PublishTo` or `SubscribeTo` routes configured|
|Durable subscriptions|The subscribing endpoint, at subscribe time|

> [!WARNING]
> Topic objects are only created automatically when explicit routes are configured via `PublishTo` or `SubscribeTo`. Topics derived from the naming convention are created on first subscription or publish if the queue manager's topic tree allows implicit topic strings.

## Runtime permissions

> [!NOTE]
> These are the minimum permissions required when all infrastructure is pre-created using the [command-line tool](operations-scripting.md), `runmqsc` scripts, or by an administrator.

### All endpoints

|Operation|Object type|Object|Authority|
|:---|---|---|---|
|Connect to queue manager|Queue manager|`QMGR`|`connect`, `inq`|
|Receive from input queue|Queue|Endpoint input queue|`get`|
|Send to error queue|Queue|Error queue|`put`|

### Sending endpoints

|Operation|Object type|Object|Authority|
|:---|---|---|---|
|Send a command|Queue|Destination queue|`put`|
|Reply to a message|Queue|Reply-to queue|`put`|

### Publishing endpoints

|Operation|Object type|Object|Authority|
|:---|---|---|---|
|Publish an event|Topic|Event topic|`pub`|

### Subscribing endpoints

|Operation|Object type|Object|Authority|
|:---|---|---|---|
|Subscribe to an event|Topic|Event topic|`sub`|
|Receive subscription messages|Queue|Endpoint input queue|`get`|
|Unsubscribe from an event|Subscription|Subscription object|`ctrl`|

> [!WARNING]
> When an endpoint subscribes, IBM MQ delivers messages from the topic to the endpoint's input queue via the durable subscription. The subscription's destination queue must allow `put` from the queue manager's internal delivery mechanism, which is typically permitted by default.

## Least privilege

In production environments, avoid granting `crt` authority to application accounts. Instead:

1. Pre-create all queues, topics, and subscriptions using the [command-line tool](operations-scripting.md) or native `runmqsc` scripts during deployment.
2. Run endpoints without `EnableInstallers()`.
3. Grant only the minimum runtime authorities listed above.

## SSL/TLS authentication

For encrypted and certificate-authenticated connections, see [SSL/TLS configuration](connection-settings.md#ssltls).
