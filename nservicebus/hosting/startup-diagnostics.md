---
title: Startup diagnostics
summary: Describes the mechanism for gathering diagnostic information when endpoints start
component: Core
versions: '[7,)'
reviewed: 2023-01-03
---

INFO: This document does not apply to Azure Function hosts. For Azure Function hosts, see [Azure Function In-process diagnostics](/nservicebus/hosting/azure-functions-service-bus/#configuration-custom-diagnostics) or [Azure Function Isolated Worker diagnostics](/nservicebus/hosting/azure-functions-service-bus/isolated-worker.md#custom-triggers-custom-diagnostics)

To make troubleshooting easier, diagnostic information is collected during endpoint startup and written to a `.diagnostics` sub-folder in the host output directory.

NOTE: By default, the output directory is called `AppDomain.CurrentDomain.BaseDirectory`, except for web applications where `App_Data` is used instead.

To change the output path:

snippet: SetDiagnosticsPath

At every endpoint startup the current diagnostics will be written to `{endpointName}-configuration.txt`. If possible, attach this file to support requests.

WARNING: The structure and format of the data produced should not be considered stable or parsable. Nodes may be added, removed, or moved in minor and patch versions.


### Sample content

Sample partial content of the startup diagnostics (formatted for readability):

snippet: StartUpDiagnosticsWriter


## Writing to other targets

To take full control of how diagnostics are written:

snippet: CustomDiagnosticsWriter


## Adding startup diagnostics sections

To extend the startup diagnostics with custom sections:

snippet: CustomDiagnosticsSection

Settings can be accessed from a [feature](/nservicebus/pipeline/features.md#feature-setup) or via the [endpoint configuration](/nservicebus/pipeline/features.md#feature-settings-endpointconfiguration).
