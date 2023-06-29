---
title: Measuring system throughput using Amazon SQS
summary: Use the Particular throughput tool to measure the throughput of an NServiceBus system.
reviewed: 2022-11-09
related:
  - nservicebus/throughput-tool
---

The Particular throughput tool can be installed locally and run against a production system to discover the throughput of each endpoint in a system over a period of time.

This article details how to collect endpoint and throughput data when the system uses the [Amazon SQS transport](/transports/sqs/). Refer to the [throughput counter main page](./) for information how to install/uninstall the tool or for other data collection options.

## Running the tool

Collecting metrics for SQS relies upon [AWSSDK.SQS](https://www.nuget.org/packages/AWSSDK.SQS) to discover queue names and [AWSSDK.CloudWatch](https://www.nuget.org/packages/AWSSDK.CloudWatch) to gather per-queue metrics.

Authentication to AWS requires a [AWS credentials profile](https://docs.aws.amazon.com/toolkit-for-visual-studio/latest/user-guide/keys-profiles-credentials.html), or credentials can be created from the `AWS_ACCESS_KEY_ID` and `AWS_SECRET_ACCESS_KEY` environment variables, if both are not empty. The tool uses default constructors for the SQS and CloudWatch clients and follows the [credential and profile resolution](https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/creds-assign.html) rules determined by the AWS SDK.

The AWS region can be specified either by command-line parameter or by the `AWS_REGION` environment variable.

If the tool was [installed as a .NET tool](/nservicebus/throughput-tool/#installation-net-tool-recommended), execute the tool as shown:

```shell
throughput-counter sqs [options]
```

Or, if using the [self-contained executable](/nservicebus/throughput-tool/#installation-self-contained-executable):

```shell
Particular.EndpointThroughputCounter.exe sqs [options]
```

## Options

| Option | Description |
|-|-|
| <nobr>`--profile`</nobr> | The name of a local [AWS credentials profile](https://docs.aws.amazon.com/toolkit-for-visual-studio/latest/user-guide/keys-profiles-credentials.html). If not included, credentials can be read from the `AWS_ACCESS_KEY_ID` and `AWS_SECRET_ACCESS_KEY` environment variables. |
| <nobr>`--region`</nobr> | The AWS region to use when accessing AWS services. If not provided, the default profile value or `AWS_REGION` environment variable will be used. |
| <nobr>`--prefix`</nobr> | Report only on queues that begin with a specific prefix. This is commonly used when one AWS account must contain queues for multiple projects or multiple environments.<br/><br/>Example: `--prefix "prod-"` |
include: throughput-tool-global-options

## What the tool does

The tool first queries the SQS API to [fetch all queue names](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/APIReference/API_ListQueues.html). Then, for each queue that is discovered, the tool queries the [CloudWatch API](https://docs.aws.amazon.com/AmazonCloudWatch/latest/APIReference/API_GetMetricStatistics.html) for the `NumberOfMessagesDeleted` metrics for the past 30 days.

Unlike ServiceControl, using SQS and CloudWatch metrics allows the tool to capture the last 30 days worth of data at once, which means that the report will be generated without delay. Although the tool collects 30 days worth of data, only the highest daily throughput is included in the report.
