---
title: SQL Server Transport Upgrade Version 2.x to 2.2.4
summary: Instructions on how to patch SQL injection vulnerability in SQL Server Transport version 2.x
reviewed: 2016-07-04
component: SqlTransport
related:
 - security-advisories/sqlserver-sqlinjection
redirects:
 - nservicebus/upgrades/sqlserver-2.x-2.2.4
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
---


This document explains how to patch a system for [SQL injection vulnerability in the SQL Server Transport](https://github.com/Particular/NServiceBus.SqlServer/issues/272) using hotfix release 2.2.4.

NOTE: Detailed information about the vulnerability, its impact, available mitigation steps and patching instructions can be found in the [security advisory](/security-advisories/sqlserver-sqlinjection.md).


## Updating the NuGet package

This vulnerability can be fixed by upgrading the SQL Server Transport package that is being used. The package can be updated by issuing the following command in the Package Manager Console within Visual Studio:

```ps
Update-Package NServiceBus.SqlServer -Version 2.2.4
```

After the package has been updated, all affected endpoints need to be rebuilt and redeployed.


## Patching a deployed system

This vulnerability can also be fixed by updating the SQL Server Transport .dll without the need to rebuild and redeploy an affected endpoint by following these steps:

 1. Update the NuGet package
 1. For each affected endpoint:
  1. Stop the endpoint.
  1. Copy the `NServiceBus.Transport.SqlServer.dll` file from the updated NuGet package to directory where binaries of the endpoint are stored. Make sure that updated version of the .dll overwrites the previous one.
  1. Restart the endpoint.