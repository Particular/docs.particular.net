---
title: "GHSA-9cvc-h2w8-phrp"
summary: "AWS SDK for .NET V4 adopted defense in depth enhancement for region parameter value"
reviewed: "2026-01-14"
---

## Security Advisory Id GHSA-9cvc-h2w8-phrp

This advisory discloses a security vulnerability 
Patches for components to update their dependencies to avoid references that have the [GHSA-9cvc-h2w8-phrp](https://github.com/advisories/ghsa-9cvc-h2w8-phrp) security advisory: AWS SDK for .NET V4 adopted defense in depth enhancement for region parameter value.

### Patch releases

| Component | Version | Where to get it |
| --------- | ------- | --------------- |
|NServiceBus.AmazonSQS|8.0.1|[NuGet](https://www.nuget.org/packages/NServiceBus.AmazonSQS/8.0.1)|
|NServiceBus.AmazonSQS.CommandLine|8.0.1|[NuGet](https://www.nuget.org/packages/NServiceBus.AmazonSQS.CommandLine/8.0.1) or `dotnet tool update --g NServiceBus.AmazonSQS.CommandLine --v 8.0.1`|
|NServiceBus.Persistence.DynamoDB|3.0.1|[NuGet](https://www.nuget.org/packages/NServiceBus.Persistence.DynamoDB/3.0.1)|
|NServiceBus.Persistence.DynamoDB.TransactionalSession|3.0.1|[NuGet](https://www.nuget.org/packages/NServiceBus.Persistence.DynamoDB.TransactionalSession/3.0.1)|
|NServiceBus.AwsLambda.Sqs|3.0.1|[NuGet](https://www.nuget.org/packages/NServiceBus.AwsLambda.SQS/3.0.1)|

### How to know if you are affected

You are affected if you are using previous versions of any of these components, but this doesn't necessarily mean you are vulnerable.

### Symptoms

For NuGet packages your projects have the setting `NuGetAuditMode` set to `all` and see transitive dependency warnings at build time that mention Particular packages.

Other components of the platform will not have any symptoms.

### When to upgrade

You should upgrade immediately if you are affected. Otherwise, you should upgrade during your next maintenance window.
