---
title: AWS Lambda Upgrade from 2 to 3
summary: Instructions on how to upgrade NServiceBus.AwsLambda.SQS 2 to 3
component: SQSLambda
reviewed: 2025-06-04
isUpgradeGuide: true
---

## Upgraded NServiceBus.AmazonSQS version 8 which upgrades AWS SDK to version 4

The AWS SDK introduces [breaking changes](https://docs.aws.amazon.com/sdk-for-net/v4/developer-guide/net-dg-v4.html), such as returning null instead of empty collections. The lambda package has been updated internally to accommodate these changes.

If your application code directly uses the AWS SDK and is affected by these changes, you’ll need to adjust your code accordingly by following the [AWS SDK v4 upgrade guidelines](https://docs.aws.amazon.com/sdk-for-net/v4/developer-guide/net-dg-v4.html).
