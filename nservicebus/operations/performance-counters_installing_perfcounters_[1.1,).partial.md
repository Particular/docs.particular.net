## Installing Counters


### [NServiceBus.Metrics.PerformanceCounters.MsBuild](https://www.nuget.org/packages/NServiceBus.Metrics.PerformanceCounters.MsBuild/)

This packages installs into the MSBuild pipeline and generates all performance counter installation scripts at compile time. It does this by interrogating types (in the target assembly) and attributes (from the `NServiceBus.Metrics.PerformanceCounter` as well as `NServiceBus.Metrics` NuGet package) to infer what scripts to create. It is required for any project where those performance counter installation scripts are required. This package has a dependency on the `NServiceBus.Metrics.PerformanceCounter` NuGet package

NOTE: Performance Counters **Versions 1.1 and above:** is dependent on `NServiceBus.Metrics` in calculating counters values.  

### Script Creation

Performance counter installation scripts are created at compile time by the `NServiceBus.Metrics.PerformanceCounters.MsBuild` NuGet package.

Scripts will be created in the directory format of `[CurrentProjectDebugDir]\NServiceBus.Metrics.PerformanceCounters\[ScriptVariant]`.

For example for a project named `ClassLibrary` build in Debug mode the following directories will be created.

 * `ClassLibrary\bin\Debug\NServiceBus.Metrics.PerformanceCounters\CSharp`
 * `ClassLibrary\bin\Debug\NServiceBus.Metrics.PerformanceCounters\Powershell`

Scripts will also be included in the list of project output files. So this means those files produced will be copied to the output directory of any project that references it.

Scripts creation can configured via the use of `[PerformanceCounterSettings]` applied to the target assembly.


### To Produce All scripts

snippet: perfcounter-script-all


### To Produce only CSharp scripts

snippet: perfcounter-script-csharp


### To Produce only Powershell scripts

snippet: perfcounter-script-powershell


### Promotion

As stated above, scripts are created in the target project output directory. Generally this directory will be excluded from source control. To add created scripts to source control they can be "promoted".

WARNING: The target directory will be deleted and recreated as part of each build. So ensure to choose a path that is for script promotion only.

Some token replacement using [MSBuild variables](https://msdn.microsoft.com/en-us/library/c02as0cs.aspx) is supported.

 * `$(SolutionDir)`: The directory of the solution.
 * `$(ProjectDir)`: The directory of the project

All tokens are drive + path and include the trailing backslash `\`.

snippet: perfcounter-script-promotion

The path calculation is performed relative to the current project directory. So, for example, a value of `PromotedScripts` (with no tokens) would evaluate as `$(ProjectDir)PromotedScripts`.


### Script Usage

The above task takes preconfigured values and based on configured [Metrics](metrics.md) generated PowerShell and/or cs files containing code creating performance counters. After running that task to create performance counters the following calls have to be made with elevated permissions:

```cs
CounterCreator.Create();
```

```ps
./CreateNSBPerfCounters.ps1
```

To list the installed counters use

```ps
Get-Counter -ListSet NServiceBus | Select-Object -ExpandProperty Counter
```

NOTE: Metrics are defined in `NServiceBus.Metrics` NuGet package and will be dynamically turned into performance counters. When the `NServiceBus.Metrics` dependency is updated new counters might be available in the installation script. Make sure the scripts are executed with elevated permissions on required machines when the scripts have been updated.

include: performance-counters-troubleshooting