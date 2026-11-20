# startcode ps-maintenance-mode-function
function Set-ServiceControlMaintenanceMode {
    param(
        [Parameter(Mandatory)] [string] $ServiceName,
        [Parameter(Mandatory)] [string] $SettingKey,
        [Parameter(Mandatory)] [bool] $Enabled
    )

    # The instance configuration file sits next to the service executable
    $exePath = (Get-CimInstance Win32_Service -Filter "Name='$ServiceName'").PathName.Trim('"')
    $configPath = "$exePath.config"

    Stop-Service -Name $ServiceName

    [xml]$config = Get-Content -Path $configPath
    $appSettings = $config.configuration.appSettings
    $setting = $appSettings.add | Where-Object key -eq $SettingKey

    if ($Enabled) {
        if (-not $setting) {
            $setting = $config.CreateElement('add')
            $setting.SetAttribute('key', $SettingKey)
            [void]$appSettings.AppendChild($setting)
        }
        $setting.SetAttribute('value', 'true')
    }
    elseif ($setting) {
        [void]$appSettings.RemoveChild($setting)
    }

    $config.Save($configPath)

    Start-Service -Name $ServiceName
}
# endcode

# startcode ps-maintenance-mode-error
# Enter maintenance mode
Set-ServiceControlMaintenanceMode -ServiceName 'Particular.ServiceControl' -SettingKey 'ServiceControl/MaintenanceMode' -Enabled $true

# Perform database maintenance, then resume normal operation
Set-ServiceControlMaintenanceMode -ServiceName 'Particular.ServiceControl' -SettingKey 'ServiceControl/MaintenanceMode' -Enabled $false
# endcode

# startcode ps-maintenance-mode-audit
# Enter maintenance mode
Set-ServiceControlMaintenanceMode -ServiceName 'Particular.ServiceControl.Audit' -SettingKey 'ServiceControl.Audit/MaintenanceMode' -Enabled $true

# Perform database maintenance, then resume normal operation
Set-ServiceControlMaintenanceMode -ServiceName 'Particular.ServiceControl.Audit' -SettingKey 'ServiceControl.Audit/MaintenanceMode' -Enabled $false
# endcode
