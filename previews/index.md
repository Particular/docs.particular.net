---
title: Particular Preview Program
summary: License details and other information about the Particular Preview Program
reviewed: 2020-11-08
---

Particular Previews are [production-supported](support-policy.md) components that are a introduced through the Particular Preview Program. This program was created to make it easier for Particular Software to advance new components into the platform while improving user involvement, validating user interest in those components, and increasing the rate at which new components can be adopted.

Preview components are offered under a more [permissive license](https://particular.net/eula/previews) than the rest of the platform. The license permits use of the preview component in production until the 1.0 version is released after which it falls under the standard platform license.

NOTE: Some components cannot be used independently of NServiceBus and the platform, which requires a commercial license to use in production.

Once a preview is launched, the preview period continues until sufficient customer adoption in production has been verified, after which the 1.0 version of the component will be released, ending the preview period.

Leading up to the launch of a new preview component, in order to help ensure adoption of the preview, a research process is undertaken to identify and validate customer needs and determine the most important features that are required to meet those needs.

User feedback is central to the Preview Program, so the more feedback is received, the better the end result will be.

## Completed Previews

| Name | Notes |
|------|-------|
| [NServiceBus.AzureFunctions.ServiceBus](/nservicebus/hosting/azure-functions-service-bus) | General Availability |
| [NServiceBus.Persistence.CosmosDB](/persistence/cosmosdb) | General Availability |
| [NServiceBus.AwsLambda.Sqs](/nservicebus/hosting/aws-lambda-simple-queue-service.md)| General Availability |

## Retired Previews

| Name | Notes |
|------|-------|
| [NServiceBus.FileBasedRouting](https://github.com/ParticularLabs/NServiceBus.FileBasedRouting) | Replaced by an [OSS equivalent](https://github.com/timbussmann/NServiceBus.FileBasedRouting) |
| [NServiceBus.AzureFunctions.StorageQueues](https://github.com/Particular/NServiceBus.AzureFunctions.StorageQueues)| Retired due to lack of adoption |
