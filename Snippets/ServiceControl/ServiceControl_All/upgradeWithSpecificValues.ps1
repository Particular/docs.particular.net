# startcode upgradeWithSpecificValues
#Requires -Version 3
#Requires -RunAsAdministrator

Import-Module "C:\Program Files (x86)\Particular Software\ServiceControl Management\ServiceControlMgmt.psd1"

$upgradeparams = @{
   Name = 'Particular.ServiceControl'
   ForwardErrorMessages = $false                  
   AuditRetentionPeriod = (New-TimeSpan -Days 30) 
   ErrorRetentionPeriod = (New-TimeSpan -Days 10)
   BodyStoragePath = 'C:\ProgramData\Particular\ServiceControl\BodyStorage'
   IngestionCachePath = 'C:\ProgramData\Particular\ServiceControl\IngestionCache'
} 

Invoke-ServiceControlUpgrade @upgradeparams
# endcode