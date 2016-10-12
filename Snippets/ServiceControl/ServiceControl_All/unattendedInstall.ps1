# startcode unattendedInstall
#Requires -Version 3
#Requires -RunAsAdministrator

Import-Module "C:\Program Files (x86)\Particular Software\ServiceControl Management\ServiceControlMgmt.psd1"

$installparams = @{
    Name = 'Particular.ServiceControl'  
    DisplayName = 'Particular.ServiceControl'
    Description = 'ServiceControl Test Instance'
    ServiceAccount = 'LocalService'
    #ServiceAccountPassword = 
    HostName = 'localhost'
    Port = 33333
    InstallPath ='C:\ServiceControl\Bin'
    DBPath = 'C:\ServiceControl\DB'
    LogPath = 'C:\ServiceControl\Logs'
    IngestionCachePath = 'C:\ServiceControl\IngestionCache'
    BodyStoragePath = 'C:\ServiceControl\BodyStorage'
    Transport = 'MSMQ'
    #ConnectionString = 
    ErrorQueue = 'Error'
    AuditQueue = 'Audit'
    ErrorLogQueue = 'Error.Log'
    AuditLogQueue = 'Audit.Log'
    ForwardAuditMessages = $false
    ForwardErrorMessages = $false
    AuditRetentionPeriod = (New-TimeSpan -Days 30)
    ErrorRetentionPeriod = (New-TimeSpan -Days 10)
}

New-ServiceControlInstance @installparams
# endcode