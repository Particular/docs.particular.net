# startcode customSettingsToConfig
#Requires -Version 3
#Requires -RunAsAdministrator

Add-Type -AssemblyName System.Configuration
Import-Module 'C:\Program Files (x86)\Particular Software\ServiceControl Management\ServiceControlMgmt.psd1'

$customSettings = @{
    'ServiceControl/HeartbeatGracePeriod'='00:01:30'
}

foreach ($sc in Get-ServiceControlInstances)
{
    $exe = Join-Path $sc.InstallPath -ChildPath 'servicecontrol.exe'
    $configManager = [System.Configuration.ConfigurationManager]::OpenExeConfiguration($exe)
    $appSettings = $configManager.AppSettings.Settings
    foreach ($key in $customSettings.Keys)
    {
        $appSettings.Remove($key)
        $appSettings.Add((New-Object System.Configuration.KeyValueConfigurationElement($key, $customSettings[$key])))
    }
    $configManager.Save()
    Restart-Service $sc.Name
}
# endcode