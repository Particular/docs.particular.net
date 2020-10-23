---
title: Assembly scanning
summary: To enable automatic detection of various features NServiceBus scans assemblies for well known types
reviewed: 2020-10-24
component: core
redirects:
 - nservicebus/assembly-scanning
---

By default all assemblies in the endpoint's `bin` directory are scanned in search of related interfaces so that the endpoint can configure them automatically.

NOTE: During the scanning process, the core assembly for NServiceBus (`NServiceBus.Core.dll`) is automatically included since it is required for endpoints to properly function.


## Controlling the assemblies to scan

There are some cases where finer control over which assemblies are loaded is required:

 * To limit the number of assemblies being scanned and hence provide improvements to startup time.
 * If hosting multiple endpoints out of the same directory each endpoint may require a subset of assemblies to be loaded.

NOTE: NServiceBus extensions such as `NServiceBus.RavenDB.dll` are not considered a core assembly but will still need to be included when customizing the assembly scanning.


## Nested Directories

partial: nested


## Assemblies to scan

partial: assemblies-to-scan


## Exclude a list approach


### Exclude specific assemblies by name:

snippet: ScanningExcludeByName


partial: wildcard


### Exclude specific types:

snippet: ScanningExcludeTypes


partial: include

partial: mixing

partial: appdomain

partial: suppress

partial: additional-path