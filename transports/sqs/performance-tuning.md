---
title: Performance Tuning
summary: Tune receive concurrency and HTTP connection limits for the Amazon SQS transport.
component: SQS
reviewed: 2025-05-06
---

> [!NOTE]
> It is difficult to give performance tuning guidelines that will be generally applicable. Results may vary greatly depending on many factors such as bandwidth, latency, client version, and much more. As always with performance tuning: Measure, don't assume.

The Amazon SQS transport uses HTTP/S connections to send and receive messages from AWS services. Connection latency affects the performance of these operations.

## Parallel message retrieval

It is possible to increase the maximum concurrency to increase the throughput of a single endpoint. For more information about how to tune the endpoint message processing, consult the [tuning guide](/nservicebus/operations/tuning.md).

The transport fetches up to ten messages per receive request. When the maximum concurrency is ten or less, it uses one receive pump and fetches up to that many messages. For higher concurrency, it fetches up to ten messages per request and increases the number of parallel receive pumps.

The transport calculates receive parallelism using floating-point division:

```text
NumberOfMessagesToFetch = Math.Min(MaxConcurrency, 10)
DegreeOfReceiveParallelism = Math.Ceiling((double)MaxConcurrency / NumberOfMessagesToFetch)
```

The following examples illustrate how the formula is applied.

|`MaxConcurrency` | `DegreeOfReceiveParallelism` | `NumberOfMessagesToFetch` |
| :-: |:-:|:-:|
| 1 | 1 | 1 |
| 2 | 1 | 2 |
| 3 | 1 | 3 |
| 4 | 1 | 4 |
| 5 | 1 | 5 |
| 6 | 1 | 6 |
| 7 | 1 | 7 |
| 8 | 1 | 8 |
| 9 | 1 | 9 |
| 10 | 1 | 10 |
| 19 | 2 | 10 |
| 21 | 3 | 10 |
| 100 | 10 | 10 |

Each parallel message retrieval requires one long polling connection.

> [!NOTE]
> Changing the maximum concurrency will influence the total number of operations against SQS and can result in higher costs.

## Number of connections

A single endpoint requires multiple HTTP connections. When a connection limit is reached, additional requests wait for a connection, which can reduce throughput.

On .NET Core and later, set `MaxConnectionsPerServer` on `AmazonSQSConfig` or `AmazonSimpleNotificationServiceConfig` before constructing the corresponding client. By default, the AWS SDK leaves this property unset and uses the HTTP handler's default of `int.MaxValue` connections per server.

Provide the configured clients to the transport using the [SQS client](/transports/sqs/configuration-options.md#sqs-client) and [SNS client](/transports/sqs/configuration-options.md#sns-client) configuration options.

#if-version (,7)

### .NET Framework

On .NET Framework, set `ConnectionLimit` on the AWS client configuration instead. The default is 50 connections unless `ServicePointManager.DefaultConnectionLimit` has been explicitly set, in which case the SDK uses that value. Configure these settings before constructing the client.

#end-if

## Sending small messages

On .NET Core and later, the AWS SDK uses `HttpClient`. Configuring Nagle's algorithm through `ServicePointManager` does not affect these connections.

#if-version (,7)

On .NET Framework, the AWS SDK disables Nagle's algorithm by default (`UseNagleAlgorithm = false`). No additional configuration is needed to disable it.

#end-if

## Known limitations

The transport shares an SQS client between receive, send, and delete operations. Each parallel receive request can hold a connection open while long polling. When setting a connection limit, allow capacity for sends and message deletions as well as parallel receives.
