---
title: Installing ServiceInsight
summary: Describes how ServiceInsight is installed
component: ServiceInsight
reviewed: 2024-06-05
---

## Prerequisites

- Microsoft Windows
- .NET Framework 4.8 or later
- A running instance of [ServiceControl](/servicecontrol)

## Installation

1. Install [ServiceControl](/servicecontrol/installation.md)—at a minimum, an [error instance](/servicecontrol/#servicecontrol-instance-types) is required.
1. Download and run the [ServiceInsight installer](https://github.com/Particular/ServiceInsight/releases)
1. After accepting the license terms and conditions, click "Install" (the installer will require elevated privileges)
1. Validation the installation by launching the “ServiceInsight” desktop application and connecting to the ServiceControl error instance at the URL shown in the [ServiceControl Management application](/servicecontrol/installation.md#installing-servicecontrol-instances) (or the host and port specified when [installing ServiceControl using PowerShell](/servicecontrol/powershell.md)). 
