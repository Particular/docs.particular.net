---
title: NServiceBus 32-bit (x86) Host Process
summary: If 32-bit code must be invoked and loaded, use NServiceBus.Host32.exe
component: Host
redirects:
 - nservicebus/nservicebus-32-bit-x86-host-process
reviewed: 2020-04-07
---

NServiceBus is an "Any CPU" framework. It doesn't have 32-bit or 64-bit specific code. This enables transitioning between 32- and 64-bit operating systems. Unfortunately, not all assemblies can be compiled using the default Any CPU architecture. In many, this is related to legacy systems that have 32-bit specific code for platform interoperability with native C libraries, etc.

With the default NServiceBus.Host, the application always loads in 64-bit (x64) mode if running on a 64-bit OS, or in 32-bit (x86) mode for a 32-bit OS. Again, this is typically not a problem.

If assemblies or other libraries containing 32-bit code must be invoked and loaded into a 64-bit process a `BadImageFormatException` will be thrown.

There are two specific versions of the NServiceBus Host: the default *Any CPU* version and `NServiceBus.Host32.exe`targeting 32-bit.

The second one allows users running a 64-bit OS to run a 32-bit NServiceBus host process, allowing execution of 32-bit binaries/code without resorting to workarounds such as `corflags.exe` to patch an assembly, which instruct the .NET Framework to run in 32-bit mode.

Links to the NuGet packages:

 * [NServiceBus.Host (32-bit and 64-bit)](https://www.nuget.org/packages/NServiceBus.Host)
 * [NServiceBus.Host32 (32-bit only)](https://www.nuget.org/packages/NServiceBus.Host32)
