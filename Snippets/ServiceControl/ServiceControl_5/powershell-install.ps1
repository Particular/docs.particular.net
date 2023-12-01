# startcode new-servicecontrol-instance
$serviceControlInstance = New-ServiceControlInstance `
  -Name Test.ServiceControl `
  -InstallPath C:\ServiceControl\Bin `
  -DBPath C:\ServiceControl\DB `
  -LogPath C:\ServiceControl\Logs `
  -Port 33334 `
  -DatabaseMaintenancePort 33335 `
  -Transport MSMQ `
  -ErrorQueue error1 `
  -ErrorRetentionPeriod 10:00:00:00
# endcode

# startcode new-audit-instance
$auditInstance = New-ServiceControlAuditInstance `
  -Name Test.ServiceControl.Audit `
  -InstallPath C:\ServiceControl.Audit\Bin `
  -DBPath C:\ServiceControl.Audit\DB `
  -LogPath C:\ServiceControl.Audit\Logs `
  -Port 44444 `
  -DatabaseMaintenancePort 44445 `
  -Transport MSMQ `
  -AuditQueue audit1 `
  -AuditRetentionPeriod 10:00:00:00 `
  -ForwardAuditMessages:$false `
  -ServiceControlQueueAddress Test.ServiceControl

Add-ServiceControlRemote `
  -Name $serviceControlInstance.Name `
  -RemoteInstanceAddress $auditInstance.Url
# endcode

# startcode remove-servicecontrol-instance
Remove-ServiceControlInstance -Name Test.ServiceControl -RemoveDB -RemoveLogs
# endcode

# startcode remove-audit-instance
Remove-ServiceControlRemote `
 -Name Test.ServiceControl `
 -RemoteInstanceAddress http://localhost:44444/api

Remove-AuditInstance `
  -Name Test.ServiceControl.Audit
  -RemoveDB -RemoveLogs
# endcode

# startcode upgrade-servicecontrol-instance
Invoke-ServiceControlInstanceUpgrade -Name InstanceToUpgrade
# endcode

# startcode get-instances
Get-ServiceControlInstances | Select Name, Version
# endcode

# startcode upgrade-audit-instance
Invoke-ServiceControlAuditInstanceUpgrade -Name InstanceToUpgrade
# endcode

# startcode get-audit-instances
Get-ServiceControlAuditInstances | Select Name, Version
# endcode