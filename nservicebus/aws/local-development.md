---
title: AWS local development using LocalStack
summary: How to use LocalStack for your development needs when using Amazon SQS Transport and DynamoDB Persistence
reviewed: 2024-10-29
---

[LocalStack](https://www.localstack.cloud/) is a tool to develop and test your AWS applications locally, reducing development time and increasing productivity.

> [!NOTE]
> Refer to the [official LocalStack documentation](https://docs.localstack.cloud/getting-started/installation/) to learn how to install and run it locally.

To configure an NServiceBus endpoint to connect to LocalStack instead of AWS, the AWS endpoint URL must be set to the address of the LocalStack instance. The simplest way is by defining the `AWS_ENDPOINT_URL` environment variable and setting its value to the LocalStack URL:

```
AWS_ENDPOINT_URL=http://localhost.localstack.cloud:4566
```

Alternatively, configure the AWS SDK `ServiceURL` configuration property, like in the following example programmatically:


```csharp
var amazonSqsConfig = new AmazonSQSConfig();
amazonSqsConfig.ServiceURL = "http://localhost.localstack.cloud:4566";
var amazonSqsClient = new AmazonSQSClient(amazonSqsConfig);
```

The above example refers to Amazon SQS. The same needs to be done for Amazon SNS and Amazon DynamoDB when using these services.


> [!NOTE]
> Remember that, even if you are not connecting to AWS cloud services, it is still required to specify access credentials and region.
> From the LocalStack perspective they could be fake values, like the following:
>
> ```bash
> AWS_ACCESS_KEY_ID=demo
> AWS_SECRET_ACCESS_KEY=demo
> AWS_REGION=us-east-1
> ```
