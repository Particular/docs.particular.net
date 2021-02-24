---
title: Performance Tuning
summary: Guidance to tweak the performance of the SQS transport
component: SQS
reviewed: 2020-05-25
---

NOTE: It is difficult to give performance tuning guidelines that will be generally applicable. Results may vary greatly depending on many factors such as bandwidth, latency, client version, and much more. As always with performance tuning: Measure, don't assume.

The Amazon SQS transport uses HTTP/S connections to send and receive messages from the AWS web services. The performance of the operations performed by the transport are subjected to the latency of the connection between the endpoint and SQS.

## Parallel message retrieval

It is possible to increase the maximum concurrency to increase the throughput of a single endpoint. For more information about how to tune the endpoint message processing, consult the [tuning guide](/nservicebus/operations/tuning.md).

In Version 4 and higher, the transport will automatically increase the degree of parallelism by applying the following formula.

```
Degree of parallelism = Math.Ceiling(MaxConcurrency / NumberOfMessagesToFetch)
```

The following examples illustrate how the formula is applied when the concurrency is greater or equal to 10.

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

NOTE: Changing the maximum concurrency will influence the total number of operations against SQS and can result in higher costs.

## Number of connections

A single endpoint requires multiple connections. Connections might be established or reused due to the connection pooling of the HTTP client infrastructure. By default, a single SQS client has a connection limit of 50 connections. When more than 50 connections are used, the endpoint connections will get queued up, and performance might decrease. 

It is possible to set the `ConnectionLimit` property on the client programatically by overriding the [client](/transports/sqs/configuration-options.md#client) or set the `ServicePointManager.DefaultConnectionLimit` (recommended).

include: servicepoint-manager-connection-limit

## Sending small messages

If the endpoint is sending a lot of small messages (http message size < 1460 bytes) it might be beneficial to turn off the [NagleAlgorithm](https://en.wikipedia.org/wiki/Nagle's_algorithm). 

To disable Nagle for a specific endpoint URI use

```
var servicePoint = ServicePointManager.FindServicePoint(new Uri("sqs-endpoint-uri"));
servicePoint.UseNagleAlgorithm = false;
```

to find the endpoint URIs used, consult the [AWS Regions and Endpoints](https://docs.aws.amazon.com/general/latest/gr/rande.html) documentation

it is also possible to disable Nagle globally for the Application Domain by applying

```
ServicePointManager.UseNagleAlgorithm = false;
```

## Known Limitations

- The transport uses a single client for all operations on SQS. The throughput of a single endpoint is thus limited to the number of connections a single client can handle