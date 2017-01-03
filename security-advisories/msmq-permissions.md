---
title: Security Advisory 2017-01-03
summary: MSMQ permissions vulnerability
reviewed: 2017-01-03
tags:
 - MSMQ
 - Transport
 - Security
---

This advisory discloses a security vulnerability that has been found in NServiceBus and fixed in a recently released hotfix.

 * All endpoints should be upgraded to the latest version of the NServiceBus package to fix this vulnerability if:
   * using the NServiceBus MSMQ transport,
   * running the NServiceBus [installers](/nservicebus/operations/installers.md) to provision endpoints,
   * queue permissions for Anonymous and Everyone accounts have been revoked.

This advisory affects all versions of the NServiceBus up to and including 5.2.19 and 6.0.2.

If there are any questions or concerns regarding this advisory, send an email to security@particular.net


## Vulnerability: installers change permissions on existing MSMQ queues

A vulnerability has been fixed that allows attackers to grant Anonymous and Everyone accounts access to existing MSMQ queues by enforcing an endpoint restart.


### Impact

Attackers can use this vulnerability to:

 * send unauthorized messages to MSMQ queues managed by NServiceBus,
 * peek and receive messages from MSMQ queues managed by NServiceBus.


### Exploitability

The exploitation of this vulnerability requires that all of the conditions below are met at the same time the attacker must be able to:

 1. Enforce an endpoint restart,
 1. Access MSMQ queues managed by the restarted endpoint using an Anonymous or Everyone account.


### Affected versions

All versions of NServiceBus, up to and including 5.2.19 and 6.0.2, are affected by this vulnerability. The issue is tracked in https://github.com/Particular/NServiceBus/issues/4266.


### Risk Mitigation

If it is not possible to upgrade all endpoints that are using the affected version of NServiceBus, the following can be used as a **temporary workaround**:

 * Schedule a task that revokes Anonymous and Everyone accounts access to MSMQ queues managed by NServiceBus after each endpoint restart.


### Fix

This vulnerability can be fixed by upgrading the NServiceBus package that is being used. Upgrades should be performed as follows:


#### Version .x users should upgrade to Version 5.2.20 or higher.

**Option 1: Full update**

Update the NuGet package using `Update-Package NServiceBus -Version 5.2.20`, re-compile the endpoint/application, and redeploy the endpoint/application.

**Option 2: In-place update**

Download the NuGet package using `Update-Package NServiceBus -Version 5.2.20`, stop affected endpoints, copy the new `NServiceBus.Core.dll` assembly, overwriting the one(s) currently deployed for the endpoints, and restart the affected endpoints.


#### Version 6.x users should upgrade to Version 6.0.3 or higher

**Option 1 Full update**

Update the NuGet package using `Update-Package NServiceBus -Version 6.0.3`, re-compile the endpoint/application, and redeploy the endpoint/application.

**Option 2  In-place update**

Download the NuGet package using `Update-Package NServiceBus -Version 6.0.3`, stop affected endpoints, copy the new `NServiceBus.Core.dll` assembly, overwriting the one(s) currently deployed for the endpoints, and restart the affected endpoints.


### Contact Info

If there are any questions or concerns regarding this advisory, send an email to security@particular.net