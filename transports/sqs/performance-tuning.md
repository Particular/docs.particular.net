---
title: Performance Tuning
summary: Guidance for tuning the performance of the Amazon SQS transport
component: SQS
reviewed: 2026-11-16
---

> [!NOTE]
> It is difficult to give performance tuning guidelines that will be generally applicable. Results may vary greatly depending on many factors such as bandwidth, latency, client version, and much more. As always with performance tuning: Measure, don't assume.

The Amazon SQS transport uses HTTPS connections to send and receive messages from AWS services. Network latency between the endpoint and SQS affects transport performance.

## Parallel message retrieval

It is possible to increase the maximum concurrency to increase the throughput of a single endpoint. For more information about how to tune the endpoint message processing, consult the [tuning guide](/nservicebus/operations/tuning.md).

The transport automatically adjusts receive parallelism based on the maximum concurrency. Each receive request fetches up to ten messages, limited by the maximum concurrency:

```text
NumberOfMessagesToFetch = min(MaxConcurrency, 10)
DegreeOfReceiveParallelism = ceiling(MaxConcurrency / NumberOfMessagesToFetch)
```

In this formula, `ceiling` rounds the result of the division up to the nearest integer. The following examples illustrate how the formula applies to different concurrency limits:

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

A single endpoint requires multiple connections. The HTTP client infrastructure pools and reuses these connections. If requests exceed the connection limit, they wait for an available connection, which can reduce throughput.

Connection settings depend on the .NET runtime and AWS SDK HTTP implementation. Configure the [SQS client](/transports/sqs/configuration-options.md#sqs-client) or the [SNS client](/transports/sqs/configuration-options.md#sns-client) as needed.

### .NET Framework

On .NET Framework, the AWS SDK client configuration exposes the `ConnectionLimit` property, which defaults to 50. Alternatively, set `ServicePointManager.DefaultConnectionLimit` during process startup.

include: servicepoint-manager-connection-limit

> [!NOTE]
> The preceding guidance applies to .NET Framework. On modern .NET, configure the HTTP handler used by the AWS SDK instead of relying on `ServicePointManager`.

## Sending small messages

On .NET Framework, if the endpoint sends many small messages, it might be beneficial to disable [Nagle's algorithm](https://en.wikipedia.org/wiki/Nagle's_algorithm). Measure the effect before applying this change.

To disable Nagle's algorithm for a specific endpoint URI, use:

```csharp
using System;
using System.Net;

var servicePoint = ServicePointManager.FindServicePoint(new Uri("sqs-endpoint-uri"));
servicePoint.UseNagleAlgorithm = false;
```

Replace `sqs-endpoint-uri` with the endpoint's absolute HTTPS URI. To find the endpoint URIs, consult the [AWS Regions and Endpoints](https://docs.aws.amazon.com/general/latest/gr/rande.html) documentation.

To disable Nagle's algorithm for the application domain, use:

```csharp
using System.Net;

ServicePointManager.UseNagleAlgorithm = false;
```

## Known Limitations

The transport uses a single client for all operations on SQS. Its connection limit can therefore constrain endpoint throughput.
