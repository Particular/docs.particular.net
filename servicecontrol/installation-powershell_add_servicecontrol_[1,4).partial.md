```ps
New-ServiceControlInstance -Name Test.ServiceControl -InstallPath C:\ServiceControl\Bin -DBPath C:\ServiceControl\DB -LogPath C:\ServiceControl\Logs -Port 33334 -DatabaseMaintenancePort 33335 -Transport MSMQ -ErrorQueue error1 -AuditQueue audit1 -ForwardAuditMessages:$false -AuditRetentionPeriod 01:00:00 -ErrorRetentionPeriod 10:00:00:00
```

There are additional parameters available to set additional configuration options such as forwarding queues, the transport connection string, or host name.
