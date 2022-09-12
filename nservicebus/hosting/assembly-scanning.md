---
title: Assembly scanning
summary: To enable automatic detection of various features NServiceBus scans assemblies for well known types
reviewed: 2022-02-15
component: core
redirects:
 - nservicebus/assembly-scanning
---

NServiceBus scans assemblies at endpoint startup to automatically detect and load [message types](/nservicebus/messaging/messages-events-commands.md), [message handlers](/nservicebus/handlers/), [features](/nservicebus/pipeline/features.md), and [installers](/nservicebus/operations/installers.md).

There are some cases where finer control over which assemblies are loaded is required:

* To limit the number of assemblies being scanned and hence provide improvements to startup time.
* If hosting multiple endpoints out of the same directory each endpoint may require a subset of assemblies to be loaded.

NOTE: NServiceBus extensions such as `NServiceBus.RavenDB.dll` are not considered a core assembly but still must be included when customizing the assembly scanning.

partial: appdomain

## Assembly files

By default all assemblies in the endpoint's `bin` directory are scanned in search of related interfaces so that the endpoint can configure them automatically.

partial: additional-path

### Nested directories

Nested directories are **not** scanned for assemblies by default. Nested directory assembly scanning can be enabled using:

snippet: ScanningNestedAssebliesEnabled

partial: disable-file-scanning

## Assemblies to scan

The assemblies being scanned can be further controlled via user-defined exclusions. This supports common scenarios removing specific assemblies from scanning without the risk of accidentally excluding required assemblies.

### Exclude specific assemblies by name

snippet: ScanningExcludeByName


### Exclude assemblies by wildcard:

Multiple assemblies can be excluded by wildcards using the following approach:

snippet: ScanningAssembliesWildcard

### Exclude specific types

snippet: ScanningExcludeTypes



## Suppress scanning exceptions

NOTE: This configuration option is only available in NServiceBus 6.2 and above.

By default, exceptions occurred during assembly scanning will be re-thrown. Those exceptions can be ignored using the following:

snippet: SwallowScanningExceptions

WARNING: Ignoring assembly scanning exceptions can cause the endpoint to not load some features, behaviors, messages or message handlers and cause incorrect behavior.
