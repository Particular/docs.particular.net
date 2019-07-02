```ps
New-ServiceControlInstance -Name Test.ServiceControl -InstallPath C:\ServiceControl\Bin -DBPath C:\ServiceControl\DB -LogPath C:\ServiceControl\Logs -Port 33334 -DatabaseMaintenancePort 33335 -Transport MSMQ -ErrorQueue error1 -ErrorRetentionPeriod 10:00:00:00
```

There are additional parameters available to set additional configuration options such as forwarding queues, the transport connection string, or host name.

If audit ingestion is required, create a separate ServiceControl Audit instance:

```ps
New-AuditInstance -Name Test.ServiceControl.Audit -InstallPath C:\ServiceControl.Audit\Bin -DBPath C:\ServiceControl.Audit\DB -LogPath C:\ServiceControl.Audit\Logs -Port 44444 -DatabaseMaintenancePort 44445 -Transport MSMQ -AuditQueue audit1 -AuditRetentionPeriod 10:00:00:00 -ForwardAuditMessages:$false -ServiceControlAddress Test.ServiceControl
```

Add the ServiceControl Audit instance as a remote to the main instance:

```ps
Add-ServiceControlRemote -Name Test.ServiceControl -RemoteInstanceAddress http://localhost:44444/api
```