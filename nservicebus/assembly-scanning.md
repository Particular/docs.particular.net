---
title: Assembly scanning in NServiceBus
summary: To enable automatic detection of various features NServiceBus scans your assemblies for well known types
tags: []
---

By default, NServiceBus scans all assemblies in the endpoint bin folder to find types implementing its interfaces so that it can configure them automatically. 

NOTE: NServiceBus scanning always includes `NServiceBus.Core.dll` and if in use `NServiceBus.Host.exe` since NServiceBus needs them to function properly. 

## Controlling the assemblies to scan

Even though our scanning gets smarter and smarter sometimes you might need to fine grained control over which assemblies gets scanned.

#### Version 5

You can pass a list of assemblies:

<!-- import ScanningListOfAssembliesV5 -->

You can pass the assemblies one by one:

<!-- import ScanningParamArrayOfAssembliesV5 -->

You can exclude specific assemblies by name:

<!-- import ScanningExcludeByNameV5 -->

You can include assemblies using pattern matching:

<!-- import ScanningIncludeByPatternV5 -->

You can mix includes and excludes:

<!-- import ScanningMixingIncludeAndExcludeV5 -->

You can specificity the directory to scan:

<!-- import ScanningCustomDirectoryV5 -->

And if you need to control the exact types the NServiceBus uses you can pass them in:

<!-- import ScanningListOfTypesV5 -->

#### Version 4

By default all types in your bin directory is scanned if you call:

<!-- import ScanningDefaultV4 -->

You can pass a list of assemblies:

<!-- import ScanningListOfAssembliesV4 -->

You can pass the assemblies one by one:

<!-- import ScanningParamArrayOfAssembliesV4 -->

You can exclude specific assemblies by name:

<!-- import ScanningExcludeByNameV4 -->

You can include assemblies using pattern matching:

<!-- import ScanningIncludeByPatternV4 -->

You can mix includes and excludes:

<!-- import ScanningMixingIncludeAndExcludeV4 -->


You can specificity a the directory to scan:

<!-- import ScanningCustomDirectoryV4 -->

And if you need to control the exact types the NServiceBus uses you can pass them in:

<!-- import ScanningListOfTypesV4 -->
