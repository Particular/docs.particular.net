---
title: Gateway Upgrade Version 1 to 2
summary: Instructions on how to upgrade the Gateway from Version 1 to 2.
tags:
 - upgrade
 - migration
related:
- nservicebus/gateway
- nservicebus/upgrades/5to6
---


## Extensibility

`IForwardMessagesToSites`, `IRouteMessagesToEndpoints`, and `IRouteMessagesToSites` have been deprecated and are no longer available as extension points in the gateway. These have been replaced by [custom channel types](/nservicebus/gateway/#custom-channel-types).


## Concurrency config

`NumberOfWorkerThreads` is now deprecated as a parameter for channels in the endpoint config file. Use `MaxConcurrency` to set the maximum number of messages that should be processed at any given time by the gateway instead.

snippet: 1to2GatewayConfig


## Automatic retries

In Versions 2 and above the gateway has its own retry mechanism. It will retry failed messages 4 times by default, increasing the delay by 60 seconds each time. The default retry policy can be [replaced](/nservicebus/gateway/#retries).


## Notifications

In Versions 2 and above the gateway does not provide any error notifications. When an error occurs during sending of a message to other sites, the message will be retried and possibly moved to the error queue. The user will not be notified about these events.

Note that in Version 1, when the user [subscribes to error notifications](/nservicebus/recoverability/subscribing-to-error-notifications.md) they receive notification in the situation described above.