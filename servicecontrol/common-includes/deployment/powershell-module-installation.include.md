The `Particular.ServiceControl.Management` module can be installed from the [PowerShell Gallery](https://www.powershellgallery.com/packages/Particular.ServiceControl.Management), and is used to add, remove, update and delete instances of ServiceControl.

## Prerequisites

#if-version [5,)

The ServiceControl PowerShell module requires a version of PowerShell (Core) greater or equal to the [oldest supported LTS version](https://learn.microsoft.com/en-us/powershell/scripting/install/powershell-support-lifecycle#powershell-end-of-support-dates). The [PowerShell Gallery page](https://www.powershellgallery.com/packages/Particular.ServiceControl.Management) will identify the minimum PowerShell version for each release.

Windows PowerShell is not supported.

> [!NOTE]
> The ServiceControl and PowerShell modules versions must match. When installing ServiceControl, instance versions must match the version of the PowerShell module used to install them.

#end-if
#if-version [,5)

The ServiceControl PowerShell module requires PowerShell Core 7.2 or later, or Windows PowerShell 5.1.

#end-if

## Installing and using the PowerShell module

In order to use the PowerShell module, the PowerShell execution policy needs to be set to `RemoteSigned`. Refer to the [PowerShell documentation](https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.security/set-executionpolicy) on how to change the execution policy.

The module can be installed from the PowerShell Gallery with the following command:

snippet: ps-install

Once the module is installed, it can be used by importing the module into the PowerShell session with the following command:

snippet: ps-import

To obtain the version of the installed management module the following command can be used:

snippet: ps-getversion

> [!NOTE]
> The majority of the cmdlets will only work if the PowerShell session is running with administrator privileges.
#if-version [,5)
> [!NOTE]
> The ServiceControl installer currently includes a legacy version of the PowerShell module called `ServiceControlMgmt` that is only supported on Windows PowerShell 5.1. It does not work with newer versions of PowerShell. The ServiceControl installer creates a shortcut in the Windows start menu to launch an administrative PowerShell Session with this legacy module automatically loaded. The legacy module is not signed, so the PowerShell execution policy needs to be set to `Unrestricted` to use it.

> [!IMPORTANT]
> `Method not found: 'System.Security.AccessControl.DirectorySecurity System.IO.DirectoryInfo.GetAccessControl(System.Security.AccessControl.AccessControlSections)'`
>
> This indicates that the PowerShell module is being executed using a newer version of PowerShell than it supports. To resolve this issue, make sure to use the PowerShell module hosted on the [PowerShell Gallery](https://www.powershellgallery.com/packages/Particular.ServiceControl.Management/).
#end-if
