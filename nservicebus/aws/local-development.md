---
title: AWS local development using LocalStack
summary: How to use LocalStack for your development needs when using Amazon SQS Transport and DynamoDB Persistence
reviewed: 2024-10-29
---

[LocalStack](https://www.localstack.cloud/) is a tool to develop and test your AWS applications locally reducing development time and increasing product velocity.

> [!NOTE]
> Refer to the [official LocalStack documentation](https://docs.localstack.cloud/getting-started/installation/) to learn how to install and run it locally.

To configure an NServiceBus endpoint to connect to LocalStack instead of connecting to AWS, the AWS endpoint URL must to be set to the address of the LocalStack instance.

There are several ways you can achieve that.

The simplest way is by defining the following environment variable:

```
AWS_ENDPOINT_URL=http://localhost.localstack.cloud:4566
```

The alternative is to programmatically configure the AWS SDK like in the following example:


```csharp
var amazonSqsConfig = new AmazonSQSConfig();
amazonSqsConfig.ServiceURL = "http://localhost.localstack.cloud:4566";
var amazonSqsClient = new AmazonSQSClient(amazonSqsConfig);
```


> [!NOTE]
> Remember that, even if you are not connecting to AWS cloud services, it is still required to specify access credentials and region.
> From the LocalStack perspective they could be fake values, like the following:
>
> ```
> AWS_ACCESS_KEY_ID=demo
> AWS_SECRET_ACCESS_KEY=demo
> AWS_REGION=us-east-1
> ```
