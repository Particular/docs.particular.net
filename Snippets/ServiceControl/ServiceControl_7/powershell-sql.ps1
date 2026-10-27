# startcode ps-new-error-instance-sqlserver
$serviceControlInstance = New-ServiceControlInstance `
  -Name Test.ServiceControl `
  -InstallPath C:\ServiceControl\Bin `
  -LogPath C:\ServiceControl\Logs `
  -Port 33334 `
  -Transport MSMQ `
  -ErrorQueue error1 `
  -ErrorRetentionPeriod 10:00:00:00 `
  -PersistenceType SQLServer `
  -DatabaseConnectionString "Server=sqlserver;Database=ServiceControl;Integrated Security=True" `
  -MessageBodyStorageType FileSystem `
  -MessageBodyPath C:\ServiceControl\Bodies
# endcode

# startcode ps-new-audit-instance-sqlserver
$auditInstance = New-ServiceControlAuditInstance `
  -Name Test.ServiceControl.Audit `
  -InstallPath C:\ServiceControl.Audit\Bin `
  -LogPath C:\ServiceControl.Audit\Logs `
  -Port 44444 `
  -Transport MSMQ `
  -AuditQueue audit1 `
  -AuditRetentionPeriod 10:00:00:00 `
  -ForwardAuditMessages:$false `
  -ServiceControlQueueAddress Test.ServiceControl `
  -PersistenceType SQLServer `
  -DatabaseConnectionString "Server=sqlserver;Database=ServiceControl;Integrated Security=True"
# endcode
