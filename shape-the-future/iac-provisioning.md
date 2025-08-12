---
title: Provisioning infrastructure as code (IaC) for NServiceBus systems
summary: Help shape the future by sharing how you use IaC to provision infrastructure when deploying NServiceBus systems
reviewed: 2025-07-21
related:
 - nservicebus/operations
---

NServiceBus can automatically provision the necessary infrastructure to run endpoints by using the [installers feature](/nservicebus/operations/installers.md). Enabling installers at the endpoint configuration level lets NServiceBus create queues, database tables and schema, and other infrastructure components required to consume and exchange messages successfully when the endpoint is started up. To do so, endpoints must be run with elevated permissions to grant them infrastructure management rights. Alternatively, infrastructure can be provisioned manually without using installers, for example, by using an Infrastructure as Code (IaC) approach. 

Currently, NServiceBus provides limited support for infrastructure-as-code tools. If you're using them today, e.g., [Azure Bicep, Azure ARM Templates](https://learn.microsoft.com/en-us/azure/templates/), [Terraform](https://developer.hashicorp.com/terraform), the [AWS CDK](https://docs.aws.amazon.com/cdk/v2/guide/home.html), or [Polumi](https://www.pulumi.com/), tell us what's missing by voicing your interest on the following public issues:

- [Ability to export resources manifest required for endpoints deployment](https://github.com/Particular/NServiceBus/issues/7370)
- [Endpoints do not support “desired state” deployment styles (e.g., AWS CDK, Bicep, or Terraform)](https://github.com/Particular/NServiceBus/issues/7189)
