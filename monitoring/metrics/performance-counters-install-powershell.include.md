## Installing Counters

The NServiceBus performance counters can be installed using the [NServiceBus PowerShell tools](/nservicebus/operations/management-using-powershell.md).

```ps
Import-Module NServiceBus.PowerShell
Install-NServiceBusPerformanceCounters
```

To list the installed counters use

```ps
Get-Counter -ListSet NServiceBus | Select-Object -ExpandProperty Counter
```

NOTE: After installing the performance counters, all endpoints must be restarted in order to start collecting the new data.
