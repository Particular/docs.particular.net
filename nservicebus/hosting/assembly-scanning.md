---
title: Assembly scanning
summary: To enable the automatic detection of various features, NServiceBus scans assemblies for well-known types
reviewed: 2024-10-24
component: core
redirects:
 - nservicebus/assembly-scanning
---

NServiceBus scans assemblies at endpoint startup to automatically detect and load [message types](/nservicebus/messaging/messages-events-commands.md), [message handlers](/nservicebus/handlers/), [features](/nservicebus/pipeline/features.md), and [installers](/nservicebus/operations/installers.md).

There are some cases where finer control over which assemblies are loaded is required:

* To limit the number of assemblies being scanned and hence improve startup time.
* If hosting multiple endpoints out of the same directory, each endpoint may require loading a subset of assemblies.

> [!NOTE]
> NServiceBus extensions such as `NServiceBus.RavenDB.dll` are not considered a core assembly but still must be included when customizing the assembly scanning.

## AppDomain assemblies

By default, the assemblies already loaded into the AppDomain are scanned. The endpoint can also be configured to disable AppDomain assembly scanning:

snippet: ScanningApDomainAssemblies

## Assembly files

By default, all assemblies in the endpoint's `bin` directory are scanned for related interfaces so that the endpoint can configure them automatically.

### Additional assembly scanning path

> [!NOTE]
> This configuration option is available only in NServiceBus version 7.4 and above.

Assembly scanning can be configured to scan an additional path for assemblies outside the default scanning path.

snippet: AdditionalAssemblyScanningPath

### Nested directories

Nested directories are **not** scanned for assemblies by default. Nested directory assembly scanning can be enabled using:

snippet: ScanningNestedAssebliesEnabled

### Disable assembly files scanning

Scanning of assemblies deployed to the `bin` folder (and other configured scanning locations) can be disabled:

snippet: disable-file-scanning

> [!WARNING]
> When disabling scanning of assembly files, ensure that all required assemblies are correctly loaded into the AppDomain at endpoint startup and that AppDomain assembly scanning is enabled.

## Assemblies to scan

The assemblies being scanned can further be controlled via user-defined exclusions. This supports common scenarios removing specific assemblies from scanning without the risk of accidentally excluding required assemblies.

### Exclude specific assemblies by name

snippet: ScanningExcludeByName

### Exclude assemblies by wildcard

Multiple assemblies can be excluded by wildcards using the following approach:

snippet: ScanningAssembliesWildcard

### Exclude specific types

snippet: ScanningExcludeTypes

## Suppress scanning exceptions

> [!NOTE]
> This configuration option is only available in NServiceBus 6.2 and above.

By default, exceptions that occurred during assembly scanning will be re-thrown. Those exceptions can be ignored using the following:

snippet: SwallowScanningExceptions

> [!WARNING]
> Ignoring assembly scanning exceptions can cause the endpoint not to load some features, behaviors, messages or message handlers and cause incorrect behavior.

partial: disable-assembly-scanning
