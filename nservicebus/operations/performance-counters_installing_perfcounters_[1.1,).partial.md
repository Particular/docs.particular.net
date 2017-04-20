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

## Performance Monitor Users local security group

When [running installers](installers.md) the service account will be automatically added to the local Performance Monitor Users group if executed with elevated privileges.


## System.InvalidOperationException

If the endpoint instance throws one of the  following exceptions at startup, then [the performance counters need to be reinstalled](#installing-counters)

 * `InvalidOperationException: The requested Performance Counter is not a custom counter, it has to be initialized as ReadOnly.`
 * `InvalidOperationException: NServiceBus performance counter for '{counterName}' is not set up correctly.`


## Corrupted Counters

Corrupted performance counters can cause the endpoint to either hang completely during startup or fail with the following error:

`NServiceBus performance counter for '{counterName}' is not set up correctly`

Should this happen try rebuilding the performance counter library using the following steps:

 1. Open an elevated command prompt
 1. Execute the following command to rebuild the performance counter library: `lodctr /r`


### More information

 * [KB2554336: How to manually rebuild Performance Counters for Windows Server 2008 64bit or Windows Server 2008 R2 systems](https://support.microsoft.com/en-us/kb/2554336)
 * [KB300956: How to manually rebuild Performance Counter Library values](https://support.microsoft.com/kb/300956)
 * [LODCTR at TechNet](https://technet.microsoft.com/en-us/library/bb490926.aspx)
