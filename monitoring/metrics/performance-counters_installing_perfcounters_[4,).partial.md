## Installing counters

The `NServiceBus.Metrics.PerformanceCounter` package installs itself into the MSBuild pipeline and generates all performance counter installation scripts at compile time. It does this by interrogating types (in the target assembly) to infer what scripts to create. It is required for any project where performance counter installation scripts are needed.

NOTE: Performance Counters **Versions 1.1 and above:** is dependent on `NServiceBus.Metrics` in calculating counters values.

### Script creation

Scripts will be created in the directory format of `[CurrentProjectDebugDir]\[.NET Version]\NServiceBus.Metrics.PerformanceCounters`.

For example: A project named `ClassLibrary` built in Debug mode will have the following directories created:

 * `ClassLibrary\bin\Debug\[.NET Version]\NServiceBus.Metrics.PerformanceCounters`

Scripts will also be included in the list of project output files. This means those files produced will be copied to the output directory of any project that references it. If a script needs to be copied to a directory for inclusion in source control repository, a [post-build event](https://docs.microsoft.com/en-us/cpp/build/how-to-use-build-events-in-msbuild-projects) can be used to copy the output outside the build directory.


### Force category recreation

The descriptions of some of the performance counters have been changed in the PowerShell script from previous versions for more clarity when viewing counters in the Performance Monitor UI.

By default, the PowerShell script will not recreate counters just to change the descriptions. To force it to recreate the counters, the new `-ForceRecreate` switch can be used:

```ps
PS> .\CreateNSBPerfCounters.ps1 -ForceRecreate
```


### Script usage

To use the Powershell script to create performance counters, the following call have to be made with elevated permissions:

```ps
./CreateNSBPerfCounters.ps1
```

To list the installed counters use

```ps
Get-Counter -ListSet NServiceBus | Select-Object -ExpandProperty Counter
```

NOTE: Metrics are defined in the `NServiceBus.Metrics` NuGet package and will be dynamically turned into performance counters. When the `NServiceBus.Metrics` dependency is updated, new counters might be available in the installation script. Make sure the scripts are executed with elevated permissions on required machines when the scripts have been updated.

include: performance-counters-troubleshooting
