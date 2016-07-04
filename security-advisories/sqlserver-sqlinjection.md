---
title: Particular Software Security Advisory 2016-07-05
summary: NServiceBus SQL Server Transport injection vulnerability
reviewed: 2016-07-04
tags:
 - SQL Server
 - Transport
 - Security
---

# Particular Software Security Advisory 2016-07-05

This advisory discloses a security vulnerability that has been found in the NServiceBus SQL Server Transport and fixed in a recently released version of that transport.
* If you are using the NServiceBus SQL Server Transport you should upgrade to the latest version of the package to fix this vulnerability.
* No other NServiceBus Transports or Persisters are affected.

This advisory affects all versions of the NServiceBus SQL Server Transport up to and including 3.0.0-beta0002.

If you have any questions or concerns regarding this advisory, please send an email to security@particular.net

## NServiceBus SQL Server Transport injection vulnerability

We have fixed a vulnerability in the SQL Server Transport that allows attackers to manipulate databases using malicious SQL statements.

### Impact

Attackers can use this vulnerability to force an NServiceBus endpoint to execute arbitrary SQL statements against the SQL Server database that stores its input queue.

### Exploitability

The exploitation of this vulnerability requires that all of the conditions below are met at the same time:
 1. An attacker must have the ability to send a malicious message to an endpoint OR an attacker must be able to modify a message that is in transit to an endpoint such that the `ReplyToAddress` header can be manipulated,
 1. The receiving endpoint sends a message based on `ReplyToAddress` header.

### Affected versions

All versions of the NServiceBus SQL Server Transport, up to and including 3.0.0-beta0002, are affected by this vulnerability. The issue is tracked in https://github.com/Particular/NServiceBus.SqlServer/issues/272.

### Risk Mitigation

If you are unable to upgrade your endpoints that are using the SQL Server Transport, the following can be used as a **temporary workaround:**

* Stop all endpoints that send a message based on the `ReplyToAddress`

### Fix

This vulnerability can be fixed by upgrading the NServiceBus SQL Server Transport package that is being used. Upgrades should be performed as follows:

#### v1.x users should upgrade to v1.2.5 or higher.

Update the NuGet package using `Update-Package NServiceBus.SqlServer -Version 1.2.5`, re-compile the endpoint/application, and redeploy the endpoint/application

#### v2.x users should upgrade to v2.2.4 or higher.

**Option 1: Full update**
Update the NuGet package using `Update-Package NServiceBus.SqlServer -Version 2.2.4`, re-compile the endpoint/application, and redeploy the endpoint/application

**Option 2: In-place update**
Download the NuGet package using `Update-Package NServiceBus.SqlServer -Version 2.2.4`, stop affected endpoints, copy the new `NServiceBus.Transports.SqlServer.dll` assembly, overwriting the one(s) currently deployed for the endpoints, and restart the affected endpoints.

#### v3 beta users should upgrade to v3.0.0-beta0003 or higher

**Option 1 Full update**
Update the NuGet package using `Update-Package NServiceBus.SqlServer -Pre`, re-compile the endpoint/application, and redeploy the endpoint/application

**Option 2  In-place update**
Download the NuGet package using `Update-Package NServiceBus.SqlServer -Pre`, stop affected endpoints, copy the new `NServiceBus.Transports.SqlServer.dll` assembly, overwriting the one(s) currently deployed for the endpoints, and restart the affected endpoints.

### Contact Info
If you have any questions or concerns regarding this advisory, please send an email to security@particular.net