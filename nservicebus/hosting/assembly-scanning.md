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

partial: nested

partial: disable-file-scanning

## Assemblies to scan

partial: assemblies-to-scan

partial: include

### Exclude specific assemblies by name

snippet: ScanningExcludeByName

partial: wildcard

### Exclude specific types

snippet: ScanningExcludeTypes

partial: suppress
