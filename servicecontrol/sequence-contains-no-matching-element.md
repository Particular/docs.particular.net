---
title: ServiceControl - InvalidOperationException: Sequence contains no matching element
summary: ServiceControl Management Utility shows System.InvalidOperationException: Sequence contains no matching element
reviewed: 2024-06-06
---

## Symptoms

The following error is shown when either:
 - launching the ServiceControl Management Utility (SCMU)
 - upgrading an instance on version >= 5.x via the SCMU


```txt
System.InvalidOperationException: Sequence contains no matching element
   at System.Linq.Enumerable.Single[TSource](IEnumerable`1 source, Func`2 predicate)
   at ServiceControlInstaller.Engine.Instances.ServiceControlAuditInstance.Reload() in 
   ...
```


## Problem

The SCMU version that is launched that is incompatible with newer added ServiceControl instances.

The SCMU version is likely to be prior to 4.33.x and a version 5.x or newer instance exists on the system.

## Solution


If you no longer have any 4.x instances:

- Uninstall the version 4.x via Windows Add/Remove software.


If you still have version 4.x instances:

- Install the latest version 4 update which does not have this bug


The lastest version 4.x release at this writing is 4.33.4 which is available for download at the following location:


- <https://github.com/Particular/ServiceControl/releases/download/4.33.4/Particular.ServiceControl-4.33.4.exe>

> [!NOTE]
> A newer version 4.x might be available. Review all versions at <https://github.com/Particular/ServiceControl/releases>

## More information

- Version 5.x instances need to be managed using the downloaded executable

- Version 4.x instance need to be managed using the installed version.

