---
title: Critical Error Behavior
summary: Handling Critical errors in Azure Cloud Services
component: CloudServicesHost
reviewed: 2022-03-11
---

include: cloudserviceshost-deprecated-warning

## Handling critical errors

### Versions 6.2.2 and above

By default, the Azure host is terminated when critical errors occur. The Azure Fabric controller restarts the host automatically.

### Versions 6.2.1 and below

By default, the Azure host is _not_ terminated when critical errors occur. Only the bus is shut down. The role does not process messages until the role host is restarted. This (probably undesired) behavior may be corrected by implementing a critical errors action that shuts down the host.

snippet: DefineCriticalErrorActionForAzureHost