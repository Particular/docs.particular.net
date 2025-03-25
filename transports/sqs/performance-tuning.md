---
title: Performance Tuning
summary: Guidance to tweak the performance of the SQS transport
component: SQS
reviewed: 2025-03-06
---

> [!IMPORTANT]
> It is difficult to give performance tuning guidelines that will be generally applicable. Results may vary greatly depending on many factors such as bandwidth, latency, client version, and much more. As always with performance tuning: Measure, don't assume.

The Amazon SQS transport uses HTTP/S connections to send and receive messages from the AWS web services. The performance of the operations performed by the transport are subjected to the latency of the connection between the endpoint and SQS.

## Parallel message retrieval

It is possible to increase the maximum concurrency to increase the throughput of a single endpoint. For more information about how to tune the endpoint message processing, consult the [tuning guide](/nservicebus/operations/tuning.md).

The transport will automatically increase the degree of parallelism by applying the following formula.

```
Degree of parallelism = Math.Ceiling(MaxConcurrency / NumberOfMessagesToFetch)
```

The following examples illustrate how the formula is applied when the concurrency is greater or equal to 10.

|DegreeOfReceiveParallelism | MaxConcurrency | NumberOfMessagesToFetch |
| :-: |:-:|:-:|
| 1 | 1 | 1 |
| 1 | 2 | 2 |
| 1 | 3 | 3 |
| 1 | 4 | 4 |
| 1 | 5 | 5 |
| 1 | 6 | 6 |
| 1 | 7 | 7 |
| 1 | 8 | 8 |
| 1 | 9 | 9 |
| 1 | 10 | 10 |
| 2 | 19 | 10 |
| 3 | 21 | 10 |
| 10 | 100 | 10 |

Each parallel message retrieval requires one long polling connection.

> [!WARNING]
> Changing the maximum concurrency will influence the total number of operations against SQS and can result in higher costs.

## Number of connections

A single endpoint requires multiple connections. Connections might be established or reused due to the connection pooling of the HTTP client infrastructure. By default, a single SQS client has a connection limit of 50 connections. When more than 50 connections are used, the endpoint connections will get queued up, and performance might decrease.

It is possible to set the `ConnectionLimit` property on the client programmatically by overriding the [SQS client](/transports/sqs/configuration-options.md#sqs-client) or the [SNS client](/transports/sqs/configuration-options.md#sns-client) as needed.

## Known Limitations

- The transport uses a single client for all operations on SQS. The throughput of a single endpoint is thus limited to the number of connections a single client can handle
