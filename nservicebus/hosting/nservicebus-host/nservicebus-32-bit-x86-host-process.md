---
title: NServiceBus 32-bit (x86) Host Process
summary: 'If 32-bit code must be invoked and loaded, use NServiceBus.Host32.exe instead. '
tags:
- NServiceBus.Host
redirects:
 - nservicebus/nservicebus-32-bit-x86-host-process
---

NServiceBus is an "Any CPU" framework. It doesn't have 32-bit or 64-bit specific code. This makes it very easy to transition between 32- and 64-bit operating systems. Unfortunately, not all assemblies can be compiled using the default Any CPU architecture. In many, if not most cases, this is related to legacy systems that have 32-bit specific code for platform interoperability with native C libraries, etc.

With the default NServiceBus.Host, the application always loads in 64-bit (x64) mode if you are running it on a 64-bit OS, or in 32-bit (x86) mode for a 32-bit OS. Again, this is typically not a problem.

But if assemblies or other libraries containing 32-bit code must be invoked and loaded into the process, you have a problem, called `BadImageFormatException`.

Beginning with NServiceBus Version 3, there are two specific versions of the NServiceBus Host: the default Any CPU version and `NServiceBus.Host32.exe`.

The second one allows users running a 64-bit OS to run a 32-bit NServiceBus process, allowing execution of 32-bit binaries/code without resorting to workarounds such as `corflags.exe`, which instruct the .NET Framework to run in 32-bit mode.

Links to the NuGet packages:

 * [NServiceBus.Host (32-bit and 64-bit)](https://www.nuget.org/packages/NServiceBus.Host)
 * [NServiceBus.Host32 (32-bit only)](https://www.nuget.org/packages/NServiceBus.Host32)