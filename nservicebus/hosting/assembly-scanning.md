---
title: Assembly scanning
summary: To enable automatic detection of various features NServiceBus scans assemblies for well known types
reviewed: 2016-03-16
tags:
- Assembly scanning
redirects:
 - nservicebus/assembly-scanning
---

By default, NServiceBus scans all assemblies in the endpoint bin directory to find types implementing its interfaces so that it can configure them automatically.

Scanning is invoked by default for self-hosting

snippet:ScanningDefault

NOTE: During the scanning process, the core dlls for NServiceBus namely `NServiceBus.Core.dll`, `NServiceBus.dll` (in versions prior to Version 5) are automatically included since the endpoint needs them to function properly.


## Controlling the assemblies to scan

There are some cases where finer control over which assemblies are loaded is required:

 * To limit the number of assemblies being scanned and hence provide improvements to startup time.
 * If hosting multiple endpoints out of the same directory (made possible in Version 5) i.e. each endpoint would want to load a subset of assemblies.
 * In Version 4.0 and below, non .NET assemblies, e.g. COM dlls might need to be excluded. In Version Version 4.1 and above non .NET assemblies are automatically excluded.

NOTE: Extensions to NServiceBus (for example `NServiceBus.Distributor.MSMQ.dll` or `NServiceBus.RavenDB.dll`) are not considered core dlls and will need to be explicitly added if when customizing assembly scanning.


## Nested Directories

In Versions 6 and above the default behavior is **not** to scan nested directories for assemblies. Nested directories assembly scanning can be enabled using:

snippet:ScanningNestedAssebliesEnabled

In Versions 5 and below assemblies in nested directories are scanned by default.


## Assemblies to scan

In Versions 6 and above the API uses an "Exclude a list" approach. This supports that the common scenario removing specific assemblies from scanning without the common side effect of accidentally excluding required assemblies.

In Versions 5 and below the API for assembly scanning took an "Include a list" approach. This proved to be problematic. Many extensions to NServiceBus rely on assembly scanning, for example transports and persistences in external NuGets. If, at endpoint configuration time, a list of assemblies was generated, and that list did not include extension assemblies, the endpoint would fail at runtime with some unexpected and hard to diagnose behaviors.


## Exclude a list approach


### Exclude specific assemblies by name:

snippet:ScanningExcludeByName


### Exclude specific types:

snippet:ScanningExcludeTypes


## Include a list approach

Note: These options are deprecated from Version 6 and above.


### Including assemblies:

snippet:ScanningListOfAssemblies


### Controlling the exact types that NServiceBus uses:

snippet:ScanningListOfTypes


### Including assemblies using pattern matching:

snippet:ScanningIncludeByPattern

`AllAssemblies` helper class can be used to create a list of assemblies either by creating a blacklist using the method `Except` or a whitelist by using Matching or a combination of both.

NOTE: The `Except`, `Matching` and `And` methods behave like `string.StartsWith(string)`.


### Mixing includes and excludes:

snippet:ScanningMixingIncludeAndExclude


### Specifying the directory to scan:

snippet:ScanningCustomDirectory