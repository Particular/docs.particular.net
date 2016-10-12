# startcode upgradeWithDefaults
#Requires -Version 3
#Requires -RunAsAdministrator

Import-Module "C:\Program Files (x86)\Particular Software\ServiceControl Management\ServiceControlMgmt.psd1"

$upgradeparams = @{
	Name = 'Particular.ServiceControl'
	Auto = $true
}

Invoke-ServiceControlUpgrade @upgradeparams 
# endcode