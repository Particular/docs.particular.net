---
title: No Endpoint Configuration Found in Scanned Assemblies
summary: Troubleshooting when no IConfigureThisEndoint instances can be found
component: Host
reviewed: 2025-05-02
redirects:
 - nservicebus/no-endpoint-configuration-found-in-scanned-assemblies-exception
---

As the exception states, NServiceBus was not able to find the endpoint configuration, i.e., an implementation of `IConfigureThisEndoint`.

The exception might be thrown by [NServiceBus.Host.exe](/nservicebus/hosting/nservicebus-host/), the NServiceBus generic host.

Here are possible causes for this exception:

 * There is no implementation of `IConfigureThisEndpoint`.
 * The class implementing `IConfigureThisEndpoint` is not public.
 * More than one implementation of `IConfigureThisEndpoint` exists.
 * The `NServiceBus.Host.exe` finds more than one assembly containing an implementation of `IConfigureThisEndpoint`  in the directory (including subdirectories) where it is running.
 * In one Visual Studio solution, different assemblies are referencing different NServiceBus versions, and those assemblies reference each other.
   * For example, the messages-assembly is compiled against an NServiceBus version different from the endpoint assembly that references it.
