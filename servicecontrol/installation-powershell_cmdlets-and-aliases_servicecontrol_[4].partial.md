| Alias                  | Cmdlet                                                     |
| ---------------------- | ---------------------------------------------------------- |
| sc-add                 | New-ServiceControlInstance                                 |
| sc-addfromunattendfile | New-ServiceControlInstanceFromUnattendedFile (deprecated)  |
| sc-delete              | Remove-ServiceControlInstance                              |
| sc-instances           | Get-ServiceControlInstances                                |
| sc-makeunattendfile    | New-ServiceControlUnattendedFile                           |
| sc-transportsinfo      | Get-ServiceControlTransportTypes                           |
| sc-upgrade             | Invoke-ServiceControlInstanceUpgrade                       |

The following cmdlets are available for the management of ServiceControl Audit instances.

| Alias                  | Cmdlet                                        |
| ---------------------- | --------------------------------------------- |
| audit-add              | New-ServiceControlAuditInstance               |
| audit-delete           | Remove-ServiceControlAuditInstance            |
| audit-instances        | Get-ServiceControlAuditInstances              |
| audit-upgrade          | Invoke-ServiceControlAuditInstanceUpgrade     |

The following cmdlets are available for the management of ServiceControl remotes.

| Alias                  | Cmdlet                                        |
| ---------------------- | --------------------------------------------- |
| sc-addremote           | Add-ServiceControlRemote                      |
| sc-deleteremote        | Remove-ServiceControlRemote                   |
| sc-remotes             | Get-ServiceControlRemotes                     |

The following cmdlets are available for the management of ServiceControl Monitoring instances.

| Alias                  | Cmdlet                                        |
| ---------------------- | --------------------------------------------- |
| mon-add                | New-MonitoringInstance                        |
| mon-delete             | Remove-MonitoringInstance                     |
| mon-instances          | Get-MonitoringInstances                       |
| mon-upgrade            | Invoke-MonitoringInstanceUpgrade              |

The following general cmdlets and aliases are provided by the ServiceControl Management PowerShell module.

| Alias                  | Cmdlet                                        |
| ---------------------- | --------------------------------------------- |
| sc-addlicense          | Import-ServiceControlLicense                  |
| sc-findlicense         | Get-ServiceControlLicense                     |
| sc-help                | Get-ServiceControlMgmtCommands                |
| urlacl-add             | Add-UrlAcl                                    |
| urlacl-delete          | Remove-UrlAcl                                 |
| urlacl-list            | Get-UrlAcls                                   |
| port-check             | Test-IfPortIsAvailable                        |
| user-sid               | Get-SecurityIdentifier                        |