---
title: Upgrade AmazonSQS Transport Version 2 to 3
summary: Instructions on how to upgrade AmazonSQS Transport Version 2 to 3.
component: Sqs
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
---


## Destination Queue creation

Previous versions of the transport automatically created destination queues on send if not available. The automatic creation of destination queues has been removed. Setting up a topology with queues is an operations concern and should happen during the [installation phase](/nservicebus/operations/installers.md) of the endpoint or via scripting when provisioning the environment. 
