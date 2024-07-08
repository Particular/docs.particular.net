---
title: "InvalidOperationException: Sequence contains no matching element"
summary: "ServiceControl Management Utility shows System.InvalidOperationException: Sequence contains no matching element"
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

The SCMU version that is launched is incompatible with instances created with a later version of ServiceControl.

More specifically, it's likely that the version of SCMU is earlier than 4.33.x and there are ServiceControl instances created with version 5.x or later.

## Solution

If you no longer have any instances of ServiceControl running in version 4.x or earlier:

- Uninstall SCMU version 4.x via Windows Add/Remove software.

If you still have instances of ServiceControl running in version 4.x or earlier:

- Install the latest version 4.x version of SCMU which does not have this bug.

The ServiceControl Management Utility is available for download at the following location:

- <https://github.com/Particular/ServiceControl/releases/download/4.33.4/Particular.ServiceControl-4.33.4.exe>

> [!NOTE]
> A newer version 4.x might be available. Review all versions at <https://github.com/Particular/ServiceControl/releases>

## More information

- Version 5.x instances must be managed using the downloaded executable.

- Version 4.x instances must be managed using the installed version.
