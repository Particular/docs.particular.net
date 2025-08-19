---
title: Installing ServiceInsight
summary: Learn about the prerequisites and the steps involved in installing ServiceInsight
component: ServiceInsight
reviewed: 2024-06-25
---

## Prerequisites

- Microsoft Windows
- .NET Framework 4.8 or later
- A running instance of [ServiceControl](/servicecontrol)

## Installation

1. Deploy [ServiceControl](/servicecontrol/)—at a minimum, an [error instance](/servicecontrol/servicecontrol-instances/deployment/) is required.
1. Download and run the [ServiceInsight installer](https://github.com/Particular/ServiceInsight/releases)
1. After accepting the license terms and conditions, click "Install" (the installer will require elevated privileges)
1. Validate the installation by launching the “ServiceInsight” desktop application and connecting to the ServiceControl error instance at the [host](/servicecontrol/servicecontrol-instances/configuration.md#host-settings-servicecontrolhostname) and [port](/servicecontrol/servicecontrol-instances/configuration.md#host-settings-servicecontrolport) configured.
