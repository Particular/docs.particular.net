# startcode ps-new-error-instance
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

# startcode ps-remove-error-instance
Remove-ServiceControlInstance `
  -Name Test.ServiceControl `
  -RemoveDB -RemoveLogs
# endcode

# startcode ps-upgrade-error-instance
Invoke-ServiceControlInstanceUpgrade -Name InstanceToUpgrade
# endcode

# startcode ps-get-error-instances
Get-ServiceControlInstances | Select Name, Version
# endcode

# startcode ps-list-audit-remotes
Get-ServiceControlRemotes -Name Particular.ServiceControl
# endcode