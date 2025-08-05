---
title: Management using PowerShell
summary: Install the infrastructure for NServiceBus on servers using PowerShell.
reviewed: 2025-08-05
related:
 - nservicebus/operations
redirects:
- nservicebus/managing-nservicebus-using-powershell
- nservicebus/operations/management-using-powershell
---

A [PowerShell module](https://learn.microsoft.com/en-us/powershell/scripting/lang-spec/chapter-11?view=powershell-7.5) that sets up a computer to run NServiceBus with the [MSMQ transport](/transports/msmq/).

The PowerShell module provides cmdlets to assist with:

- Installing [Microsoft Message Queuing Service](https://learn.microsoft.com/en-us/previous-versions/windows/desktop/msmq/ms711472(v=vs.85)) (MSMQ)
- Configuring [Microsoft Distributed Transaction Coordinator](https://en.wikipedia.org/wiki/Microsoft_Distributed_Transaction_Coordinator) (MSDTC)
- Setting the addresses of the default Error and Audit queues for use by deployed Endpoints
- Importing a [Particular Platform license](/nservicebus/licensing/) into the Registry

## Prerequisites

Prior to installation, ensure that PowerShell 5 is installed. Versions of PowerShell later than 5, including PowerShell Core, are not supported by the module and may not work as expected.

> [!WARNING]
> PowerShell 5 and under are [no longer supported](https://learn.microsoft.com/en-us/powershell/scripting/install/powershell-support-lifecycle?view=powershell-7.5#powershell-end-of-support-dates) by Microsoft.

You can confirm which PowerShell version you have installed by running the following PowerShell cmdlet.

  ```ps
  $PSVersionTable.PSVersion
  ```

> [!NOTE]
> In order to run PowerShell cmdlets, the PowerShell execution policy must be set to `Unrestricted` or a bypass must be granted to the module file. Refer to PowerShell documentation on how to change the execution policy.

## Installation

The installation file for the module can be **[downloaded here](https://github.com/particular/NServiceBus.PowerShell/releases/latest)**.

> [!WARNING]
> The [NServiceBus.PowerShell](https://github.com/Particular/NServiceBus.PowerShell) repository used for MSMQ is no longer maintained, and has been archived. As MSMQ is not available for .NET (Core), building new systems using MSMQ is not recommended.

## Usage

After installation, the module can be loaded into a PowerShell session by issuing the following PowerShell cmdlet:

```ps
Import-Module NServiceBus.PowerShell
```

The installation adds the NServiceBus.PowerShell module location to the `PSModulePath` environment variable. If the module isn't available, restarting the Windows session may be required for this change to take effect.

Most of the cmdlets require elevated privileges; the module should be used in a PowerShell session that is launched with `Run As Administrator`.

## Help

A list of available cmdlets can be found by issuing the following PowerShell cmdlet.

```ps
Get-Command -Module NServiceBus.PowerShell
```

Help for each cmdlet is incorporated within the module. Help can be accessed via the [PowerShell Get-Help cmdlet](https://technet.microsoft.com/en-us/library/ee176848.aspx), e.g. `Get-Help Set-NServiceBusLocalMachineSettings`.

## Troubleshooting

If you encounter issues when using the NServiceBus.PowerShell module, consider the following troubleshooting steps:

- **Check PowerShell Version:** Ensure you are running PowerShell 5. Use the following PowerShell cmdlet to verify your version:

  ```ps
  $PSVersionTable.PSVersion
  ```

- **Module Not Recognized:** If the module is not recognized, verify that the installation path is included in the `PSModulePath` environment variable. Restart your Windows session if necessary.

- **Execution Policy Errors:** If you receive errors related to script execution policies, ensure the policy is set to `Unrestricted` or that a bypass is granted to the module file. Refer to PowerShell documentation for details.

- **Administrator Privileges:** Some cmdlets require elevated privileges. Try running PowerShell as Administrator.

- **Module Maintenance:** The NServiceBus.PowerShell module is no longer maintained and may not be compatible with newer versions of Windows or PowerShell. Consider alternative approaches or transports if you encounter persistent issues.
