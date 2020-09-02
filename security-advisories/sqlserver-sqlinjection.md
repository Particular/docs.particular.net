---
title: Security Advisory 2016-07-05
summary: NServiceBus SQL Server Transport injection vulnerability
reviewed: 2020-09-02
---

This advisory discloses a security vulnerability that has been found in the [SQL Server Transport](/transports/sql/) and fixed in a recently released version.

 * If using the NServiceBus SQL Server Transport then all endpoints should upgraded to the latest version of the package to fix this vulnerability.
 * No other transports or persisters are affected.

This advisory affects all versions of the SQL Server Transport up to and including 3.0.0-beta0002.

If there are any questions or concerns regarding this advisory, send an email to [security@particular.net](mailto://security@particular.net).


## SQL Server Transport injection vulnerability

A vulnerability has been fixed in the SQL Server Transport that allows attackers to manipulate databases using malicious SQL statements.


### Impact

Attackers can use this vulnerability to force an endpoint to execute arbitrary SQL statements against the SQL Server database that stores its input queue.


### Exploitability

The exploitation of this vulnerability requires that all of the conditions below are met at the same time:

 1. An attacker must have the ability to send a malicious message to an endpoint OR an attacker must be able to modify a message that is in transit to an endpoint such that the `ReplyToAddress` header can be manipulated,
 1. The receiving endpoint sends a message based on `ReplyToAddress`s header - for example, by using the `Reply` method of `IBus` or using the `IMessageHandlerContext` parameter available in the message handlers when using NServiceBus version 6 and above.


### Affected versions

All versions of the SQL Server Transport, up to and including 3.0.0-beta0002, are affected by this vulnerability. The issue is tracked in https://github.com/Particular/NServiceBus.SqlServer/issues/272.


### Risk mitigation

If it is not possible to immediately upgrade endpoints that are using the SQL Server Transport, the following approach can be used as a **temporary workaround:**

 * Stop all endpoints that send a message based on the `ReplyToAddress`.


### Fix

This vulnerability can be fixed by upgrading the SQL Server Transport package that is being used. Upgrades should be performed as follows:


#### Version 1.x users should upgrade to version 1.2.5 or higher.

Update the NuGet package using `Update-Package NServiceBus.SqlServer -Version 1.2.5`, re-compile the endpoint/application, and redeploy the endpoint/application


#### Version 2.x users should upgrade to version 2.2.4 or higher

**Option 1: Full update**

Update the NuGet package using `Update-Package NServiceBus.SqlServer -Version 2.2.4`, re-compile the endpoint/application, and redeploy the endpoint/application

**Option 2: In-place update**

Download the NuGet package using `Update-Package NServiceBus.SqlServer -Version 2.2.4`, stop affected endpoints, copy the new `NServiceBus.Transports.SqlServer.dll` assembly, overwriting the one(s) currently deployed for the endpoints, and restart the affected endpoints.


#### Version 3 beta users should upgrade to version 3.1 or higher

**Option 1: Full update**

Update the NuGet package using `Update-Package NServiceBus.SqlServer -Pre`, re-compile the endpoint/application, and redeploy the endpoint/application.

**Option 2:  In-place update**

Download the NuGet package using `Update-Package NServiceBus.SqlServer -Pre`, stop affected endpoints, copy the new `NServiceBus.Transports.SqlServer.dll` assembly, overwriting the one(s) currently deployed for the endpoints, and restart the affected endpoints.


### Contact info

If there are any questions or concerns regarding this advisory, contact [security@particular.net](mailto://security@particular.net).
