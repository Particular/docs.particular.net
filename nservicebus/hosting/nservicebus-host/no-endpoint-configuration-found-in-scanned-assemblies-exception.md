---
title: No Endpoint Configuration Found in Scanned Assemblies
summary: Troubleshooting when no IConfigureThisEndoint instances can be found
component: Host
reviewed: 2018-09-14
redirects:
 - nservicebus/no-endpoint-configuration-found-in-scanned-assemblies-exception
---

As the exception states, NServiceBus was not able to find the endpoint configuration, i.e., an implementation of `IConfigureThisEndoint`.

The exception might be thrown by [NServiceBus.Host.Exe](/nservicebus/hosting/nservicebus-host/), the NServiceBus generic host.

Here are possible causes for this exception:

 * There is no implementation of `IConfigureThisEndpoint`.
 * The class implementing `IConfigureThisEndpoint` is not public.
 * More than one implementation of `IConfigureThisEndpoint` exists.
 * NServiceBus.Host.Exe is scanning the directory (and sub-directories) from where it is running, and finds more than one assembly implementing `IConfigureThisEndpoint`.
 * In one Visual Studio solution, there are different assemblies referencing different NServiceBus versions, and those assemblies reference each other.
   * For example, the messages assembly is compiled against an NServiceBus version that is different from the endpoint assembly that references it.
