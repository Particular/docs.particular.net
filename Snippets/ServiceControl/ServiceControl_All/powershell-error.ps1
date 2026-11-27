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

# startcode ps-start-error-maintenance-mode
Start-ServiceControlMaintenance -Name Test.ServiceControl
# endcode

# startcode ps-stop-error-maintenance-mode
Stop-ServiceControlMaintenance -Name Test.ServiceControl
# endcode

# startcode ps-list-audit-remotes
Get-ServiceControlRemotes -Name Particular.ServiceControl
# endcode