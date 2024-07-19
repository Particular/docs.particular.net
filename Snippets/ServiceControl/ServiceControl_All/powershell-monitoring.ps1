# startcode ps-new-monitoring-instance
$monitoringInstance = New-MonitoringInstance `
  -Name Particular.Monitoring `
  -InstallPath C:\ServiceControlMonitor\Bin `
  -LogPath C:\ServiceMonitor\Logs `
  -Port 33335 `
  -Transport MSMQ
# endcode

# startcode ps-remove-monitoring-instance
Remove-MonitoringInstance `
  -Name Test.Monitoring `
  -RemoveLogs
# endcode

# startcode ps-upgrade-monitoring-instance
Invoke-MonitoringInstanceUpgrade -Name Test.Monitoring
# endcode

# startcode ps-get-monitoring-instances
Get-MonitoringInstances | Select Name, Version
# endcode