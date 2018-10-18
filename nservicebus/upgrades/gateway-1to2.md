---
title: Gateway Upgrade Version 1 to 2
summary: Instructions on how to upgrade the Gateway from version 1 to 2.
reviewed: 2018-10-18
component: Gateway
related:
 - nservicebus/upgrades/5to6
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


## Extensibility

`IForwardMessagesToSites`, `IRouteMessagesToEndpoints`, and `IRouteMessagesToSites` have been deprecated and are no longer available as extension points in the gateway. These have been replaced by [custom channel types](/nservicebus/gateway/multi-site-deployments.md#incoming-channels).


## Concurrency configuration

`NumberOfWorkerThreads` is deprecated as a parameter for channels in the endpoint config file. Use `MaxConcurrency` to set the maximum number of messages that should be processed at any given time by the gateway instead.

snippet: 1to2GatewayConfig


## Automatic retries

In versions 2 and above, the gateway has its own retry mechanism. It will retry failed messages four times by default, increasing the delay by 60 seconds each time. The default retry policy can be [replaced](/nservicebus/gateway/#using-the-gateway-recoverability).


## Notifications

In versions 2 and above, the gateway does not provide error notifications. When an error occurs during sending of a message to other sites, the message will be retried and possibly moved to the error queue.

Note that in version 1, when [subscribing to error notifications](/nservicebus/recoverability/subscribing-to-error-notifications.md), the notification is received in the situation described above.