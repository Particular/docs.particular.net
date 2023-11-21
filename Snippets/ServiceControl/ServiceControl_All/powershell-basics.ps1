# startcode ps-install
Install-Module -Name Particular.ServiceControl.Management
# endcode

# startcode ps-import
Import-Module Particular.ServiceControl.Management
# endcode

# startcode ps-getversion
Get-Module Particular.ServiceControl.Management | Select-Object -ExpandProperty Version
# endcode

# startcode ps-testport
Test-IfPortIsAvailable -Port 33333
# endcode

# startcode ps-testportrange
33330..33339 | Test-IfPortIsAvailable | ? Available
# endcode

# startcode ps-urlacls
Get-UrlAcls | ? Port -eq 33333
# endcode

# startcode ps-removeurlacl
Get-UrlAcls | ? Port -eq 33335 | Remove-UrlAcl
# endcode

# startcode ps-addurlacl
Add-UrlAcl -Url http://servicecontrol.mycompany.com:33333/api/ -Users Users
# endcode

# startcode ps-importlicense
Import-ServiceControlLicense License.xml
# endcode

# startcode ps-get-help
Get-Help Get-ServiceControlInstances
# endcode