# startcode ps-install
Install-Module -Name Particular.ServiceControl.Management
# endcode

# startcode ps-import
Import-Module Particular.ServiceControl.Management
# endcode

# startcode ps-getversion
Get-Module Particular.ServiceControl.Management | Select-Object -ExpandProperty Version
# endcode

# startcode ps-update
Update-Module -Name Particular.ServiceControl.Management
# endcode

# startcode ps-testport
Test-IfPortIsAvailable -Port 33333
# endcode

# startcode ps-testportrange
33330..33339 | Test-IfPortIsAvailable | ? Available
# endcode

# startcode ps-importlicense
Import-ServiceControlLicense License.xml
# endcode

# startcode ps-get-help
Get-Help Get-ServiceControlManagementCommands
# endcode