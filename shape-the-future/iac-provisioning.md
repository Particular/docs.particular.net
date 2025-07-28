---
title: Provisioning infrastructure with code (IaC) for NServiceBus systems
summary: Help us shape the future by describing how IaC is used to provision infrastructure to deploy NServiceBus systems
reviewed: 2025-07-21
related:
 - nservicebus/operations
---

System resources (infrastructure components) required by endpoints to run can be provisioned automatically using [installers](/nservicebus/operations/installers.md). When provisioning infrastructure manually without using installers (e.g., with Infrastructure as Code), several information must be gathered to provision the required infrastructure successfully.

If you're using infrastructure as code tools today, e.g., Azure Bicep, Azure ARM Templates, Terraform, or the AWS CDK, tell us what's missing by voicing your interest on the following public issues:

- [Ability to export resources manifest required for endpoints deployment](https://github.com/Particular/NServiceBus/issues/7370)
- [Endpoints do not support “desired state” deployment styles (e.g., AWS CDK, Bicep, or Terraform)](https://github.com/Particular/NServiceBus/issues/7189)
