# startcode ps-new-audit-instance
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
# endcode

# startcode ps-add-audit-remote
Add-ServiceControlRemote `
  -Name $serviceControlInstance.Name `
  -RemoteInstanceAddress $auditInstance.Url
# endcode

# startcode ps-remove-audit-remote
Remove-ServiceControlRemote `
 -Name Test.ServiceControl `
 -RemoteInstanceAddress http://localhost:44444/api
# endcode

# startcode ps-remove-audit-instance
Remove-AuditInstance `
  -Name Test.ServiceControl.Audit
  -RemoveDB -RemoveLogs
# endcode

# startcode ps-upgrade-audit-instance
Invoke-ServiceControlAuditInstanceUpgrade -Name InstanceToUpgrade
# endcode

# startcode ps-get-audit-instances
Get-ServiceControlAuditInstances | Select Name, Version
# endcode