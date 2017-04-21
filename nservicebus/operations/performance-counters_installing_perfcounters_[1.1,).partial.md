## Installing Counters

The NServiceBus Performance counters can be installed using the MSBuild task called `ScriptBuilderTask`. That task has few configuration options:
 - `AssemblyPath` - a path to the endpoint assembly that has PerformanceCounters enabled
 - `IntermediateDirectory` - a path to the directory to which the script files will be generated
 - `ProjectDirectory` - a directory of a project to which the files should be generated
 - `SolutionDirectory` - solution directory to which the files should be generated.

The above task takes preconfigured values and based on configured [Metrics](metrics.md) generated PowerShell and/or cs files containing code creating performance counters. After running that task to create performance counters the following calls have to be made:

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

include: performance-counters-troubleshooting