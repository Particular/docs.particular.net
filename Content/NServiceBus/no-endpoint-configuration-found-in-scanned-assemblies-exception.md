<!--
title: "No Endpoint Configuration Found in Scanned Assemblies Exception"
tags: ""
summary: "<p>As the exception states, NServiceBus was not able to find the endpoint configuration, i.e., an implementation of IConfigureThisEndoint.</p>
<p>The exception might be thrown by
<a href="the-nservicebus-host.md">NServiceBus.Host.Exe</a>, the NServiceBus generic host.</p>
"
-->

As the exception states, NServiceBus was not able to find the endpoint configuration, i.e., an implementation of IConfigureThisEndoint.

The exception might be thrown by
[NServiceBus.Host.Exe](the-nservicebus-host.md), the NServiceBus generic host.

Following are possible causes for this exception:

-   There is no implementation of IConfigureThisEndpoint.
-   The class implementing IConfigureThisEndpoint is not public or
    internal.
-   You have more than one implementation of IConfigureThisEndpoint.
-   NServiceBus.Host.Exe is scanning the folder (and subfolders) from
    where it is running, and finds more than one assembly implementing
    IConfigureThisEndpoint.
-   In the one Visual Studio solution you have different assemblies
    referencing different NServiceBus versions, and those assemblies
    reference each other.

     For example, the messages assembly is compiled against an
    NServiceBus version that is different from the endpoint assembly
    that references it.


