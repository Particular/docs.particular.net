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
Remove-ServiceControlAuditInstance `
  -Name Test.ServiceControl.Audit `
  -RemoveDB -RemoveLogs
# endcode

# startcode ps-upgrade-audit-instance
Invoke-ServiceControlAuditInstanceUpgrade -Name InstanceToUpgrade
# endcode

# startcode ps-get-audit-instances
Get-ServiceControlAuditInstances | Select Name, Version
# endcode

# startcode ps-start-audit-maintenance-mode
Start-ServiceControlAuditMaintenance -Name Test.ServiceControl.Audit
# endcode

# startcode ps-stop-audit-maintenance-mode
Stop-ServiceControlAuditMaintenance -Name Test.ServiceControl.Audit
# endcode