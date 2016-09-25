# startcode customSettingsToConfig
#Requires -Version 3
#Requires -RunAsAdministrator

Add-Type -AssemblyName System.Configuration
Import-Module 'C:\Program Files (x86)\Particular Software\ServiceControl Management\ServiceControlMgmt.psd1'

#Hashtable of settings to add
$customSettings = @{
    'ServiceControl/HeartbeatGracePeriod'='00:01:30'
}

$sc = Get-ServiceControlInstances | ? Name -eq "Particular.ServiceControl"
$sc | % {

    $configManager = [System.Configuration.ConfigurationManager]::OpenExeConfiguration((Join-Path $sc.InstallPath, "ServiceControl.exe"))
    $appSettings = $configManager.AppSettings.Settings
    foreach ($key in $customSettings.Keys)
    {
        $appSettings.Remove($key)
        $appSettings.Add((New-Object System.Configuration.KeyValueConfigurationElement($key, $customSettings[$key])))
    }
    $configManager.Save()
}
$sc | Restart-Service

# endcode