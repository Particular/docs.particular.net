---
title: AmazonSQS Transport Upgrade Version 7 to 8
summary: Instructions on how to upgrade the AmazonSQS transport from version 7 to 8
component: SQS
reviewed: 2025-06-04
isUpgradeGuide: true
---

## Upgraded the AWS SDK to version 4

The AWS SDK introduces [breaking changes](https://docs.aws.amazon.com/sdk-for-net/v4/developer-guide/net-dg-v4.html), such as returning null instead of empty collections. The persistence has been updated internally to accommodate these changes.

If your application code directly uses the AWS SDK and is affected by these changes, you’ll need to adjust your code accordingly by following the [AWS SDK v4 upgrade guidelines](https://docs.aws.amazon.com/sdk-for-net/v4/developer-guide/net-dg-v4.html).