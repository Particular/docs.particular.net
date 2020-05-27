---
title: Critical Error Behavior
summary: Handling Critical errors in Azure Cloud Services
component: CloudServicesHost
reviewed: 2019-11-04
---

include: cloudserviceshost-deprecated-warning


## Handling critical errors


### Versions 6.2.2 and above

The Azure host is terminated on critical errors by default. When the host is terminated, the Azure Fabric controller will restart the host automatically.


### Versions 6.2.1 and below

The Azure host is not terminated on critical errors by default and only shuts down the bus. This would cause the role not to process messages until the role host is restarted. To address this (probably undesired) behavior, implement a critical errors action that shuts down the host process instead.

snippet: DefineCriticalErrorActionForAzureHost