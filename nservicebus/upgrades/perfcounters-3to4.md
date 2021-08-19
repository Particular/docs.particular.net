---
title: Upgrade PerformanceCounters Version 3 to 4
summary: Instructions on how to upgrade PerformanceCounters Version 3 to 4.
component: PerfCounters
reviewed: 2021-08-19
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

NServiceBus.Metrics.PerformanceCounters 4.0 supports .NET Core, and deprecates the NServiceBus.Metrics.PerformanceCounters.MsBuild package.

## .NET Core support

NServiceBus.Metrics.PerformanceCounters now supports .NET Framework and .NET Core, however because performance counters are Windows only, the package only works in .NET Core on Windows. Attempting to run it on other architectures will result in a `PlatformNotSupportedException`.

## MsBuild package deprecated

In NServiceBus.Metrics.PerformanceCounters 4.0, the [NServiceBus.Metrics.PerformanceCounters.MsBuild](https://www.nuget.org/packages/NServiceBus.Metrics.PerformanceCounters.MsBuild) NuGet package is deprecated, and the PowerShell script used to create performance counters has been merged into the [NServiceBus.Metrics.PerformanceCounters](https://www.nuget.org/packages/NServiceBus.Metrics.PerformanceCounters) package.

Version 4.0.0 of NServiceBus.Metrics.PerformanceCounters.MsBuild will be the last release of this package. When included in a project, it will produce this error at compile time:

> The NServiceBus.Metrics.PerformanceCounters.MsBuild package is deprecated and should be removed from the project. The PowerShell script to create performance counter objects is now included in the NServiceBus.Metrics.PerformanceCounters package.

In all projects where the NServiceBus.Metrics.PerformanceCounters.MsBuild package is in use, it can be removed when upgrading to NServiceBus.Metrics.PerformanceCounters 4.0, and the PowerShell script will continue to be included in the build output.

## PerformanceCounterSettingsAttribute deprecated

Along with the MsBuild package deprecation, the `PerformanceCounterSettingsAttribute` which controlled the MsBuild package's generation capabilities is also deprecated.

The `ScriptPromotionPath` property is no longer supported. The new package will no longer promote the script(s) outside of the build directory. If this is required, a [post-build event](https://docs.microsoft.com/en-us/cpp/build/how-to-use-build-events-in-msbuild-projects) can be used to copy the output outside the build directory.

## Counter descriptions changed

The descriptions of some of the performance counters have been changed in the PowerShell script for more clarity when viewing counters in the Performance Monitor UI.

By default, the PowerShell script will not recreate counters just to change the descriptions. To force it to recreate the counters, the new `-ForceRecreate` switch can be used:

```ps
PS> .\CreateNSBPerfCounters.ps1 -ForceRecreate
```

## C# script deprecated

The C# script file `Counters.g.cs` is deprecated and will no longer be included in the build output.

If this is required, the PowerShell script can be executed from within C# code using `Process.Start()`.

Alternatively, the following is the source of the removed script:

```cs
using System;
using System.Diagnostics;
using System.Security;
using System.Runtime.CompilerServices;

[CompilerGenerated]
public static class CounterCreator
{
    public static void Create()
    {
        var counterCreationCollection = new CounterCreationDataCollection(Counters);
        try
        {
            var categoryName = "NServiceBus";

            if (PerformanceCounterCategory.Exists(categoryName))
            {
                foreach (CounterCreationData counter in counterCreationCollection)
                {
                    if (!PerformanceCounterCategory.CounterExists(counter.CounterName, categoryName))
                    {
                        PerformanceCounterCategory.Delete(categoryName);
                        break;
                    }
                }
            }

            if (PerformanceCounterCategory.Exists(categoryName) == false)
            {
                PerformanceCounterCategory.Create(
                    categoryName: categoryName,
                    categoryHelp: "NServiceBus statistics",
                    categoryType: PerformanceCounterCategoryType.MultiInstance,
                    counterData: counterCreationCollection);
            }

            PerformanceCounter.CloseSharedResources();
        }
        catch (Exception ex) when(ex is SecurityException || ex is UnauthorizedAccessException)
        {
            throw new Exception("Execution requires elevated permissions", ex);
        }
    }

    static CounterCreationData[] Counters = new CounterCreationData[]
    {
        new CounterCreationData("SLA violation countdown", "Seconds until the SLA for this endpoint is breached.", PerformanceCounterType.NumberOfItems32),
        new CounterCreationData("Critical Time Average", "The time it took from sending to processing the message.", PerformanceCounterType.AverageTimer32),
        new CounterCreationData("Critical Time AverageBase", "The time it took from sending to processing the message.", PerformanceCounterType.AverageBase),
        new CounterCreationData("Critical Time", "The time it took from sending to processing the message.", PerformanceCounterType.NumberOfItems32),
        new CounterCreationData("Processing Time Average", "The time it took to successfully process a message.", PerformanceCounterType.AverageTimer32),
        new CounterCreationData("Processing Time AverageBase", "The time it took to successfully process a message.", PerformanceCounterType.AverageBase),
        new CounterCreationData("Processing Time", "The time it took to successfully process a message.", PerformanceCounterType.NumberOfItems32),
        new CounterCreationData("# of msgs failures / sec", "The current number of failed processed messages by the transport per second.", PerformanceCounterType.RateOfCountsPerSecond32),
        new CounterCreationData("# of msgs successfully processed / sec", "The current number of messages processed successfully by the transport per second.", PerformanceCounterType.RateOfCountsPerSecond32),
        new CounterCreationData("# of msgs pulled from the input queue /sec", "The current number of messages pulled from the input queue by the transport per second.", PerformanceCounterType.RateOfCountsPerSecond32),
        new CounterCreationData("Retries", "A message has been scheduled for retry (FLR or SLR)", PerformanceCounterType.RateOfCountsPerSecond32),

    };
}
```

