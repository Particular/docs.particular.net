---
title: SQL Server Transport Upgrade Version 1.x to 1.2.5
summary: Instructions on how to patch SQL injection vulnerability in SQL Server Transport version 1.x
reviewed: 2018-05-18
component: SqlTransport
related:
 - security-advisories/sqlserver-sqlinjection
redirects:
 - nservicebus/upgrades/sqlserver-1.x-1.2.5
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 4
---


This document explains how to patch a system for [SQL injection vulnerability in the SQL Server transport](https://github.com/Particular/NServiceBus.SqlServer/issues/272) using hotfix release 1.2.5.

NOTE: Detailed information about the vulnerability, its impact, available mitigation steps, and patching instructions can be found in the [security advisory](/security-advisories/sqlserver-sqlinjection.md).


## Updating the NuGet package

The vulnerability can be fixed by upgrading the SQL Server transport package that is being used. The package can be updated with the following command in the Package Manager Console within Visual Studio:

```ps
Update-Package NServiceBus.SqlServer -Version 1.2.5
```

After the package has been updated, all affected endpoints must be rebuilt and redeployed.
