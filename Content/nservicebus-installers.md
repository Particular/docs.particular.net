---
layout:
title: "NServiceBus Installers"
tags: 
origin: http://www.particular.net/Articles/nservicebus-installers
---
NServiceBus V3.0 introduces the concept of installers to make sure that both infrastructure and endpoint specific artifacts are installed and configured automatically for you if needed.

The installers come in two flavors, infrastructure installers and regular installers. 

Infrastructure installers are used for things that are not specific to a given endpoint e.g., RavenDB or MSMQ. 

Regular installers focus on setting up things that the current endpoint depends upon, e.g., queues, folders, or databases. 

Infrastructure installers are always invoked before regular installers. Although the installers are mainly used internally, you could use them for your own purposes as well, for example to ensure folders are created, database scripts invoked, etc.

To create your own installer is as easy as implementing the
[INeedToInstallInfrastructure<t>](https://github.com/NServiceBus/NServiceBus/blob/master/src/installation/NServiceBus.Installation/INeedToInstallInfrastructure.cs) interface. The generic parameter gives you a way to restrict your installer to a specific platform. Currently this is either Windows or Azure. 

If you don’t care about the runtime environment, use the
[INeedToInstallInfrastructure](https://github.com/NServiceBus/NServiceBus/blob/master/src/installation/NServiceBus.Installation/INeedToInstallInfrastructure.cs) interface instead. To create a regular installer, implement the
[INeedToInstallSomething<t>](https://github.com/NServiceBus/NServiceBus/blob/master/src/installation/NServiceBus.Installation/INeedToInstallSomething.cs) interface again using the T to restrict it to a specific environment.

NServiceBus scans the assemblies in the runtime directory for installers so you don’t need any code to register them.

When are they invoked?
----------------------

When using the NServiceBus host, installers are invoked as shown:

<table style="margin: 0px; padding: 0px; border: 1px solid black; font-size: 13px; font-family: Calibri; vertical-align: baseline; outline: none; color: rgb(0, 0, 0); line-height: normal; background-color: rgb(240, 240, 240); width: 564px;">
<tbody style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none;">
<tr style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none;">
<th style="margin: 0px; padding: 3px; border: 0px; vertical-align: baseline; outline: none;">
</th>
<td colspan="2" style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none; text-align: center;">
<span style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none;">**<span style="margin: 0px; padding: 0px; border: 0px; font-size: medium; vertical-align: baseline; outline: none;">Installers Invoked</span>**</span>

</td>
</tr>
<tr style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none;">
<th style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none;">
**<span style="margin: 0px; padding: 0px; border: 0px; font-size: medium; vertical-align: baseline; outline: none;">Command Line Parameters</span>**

</th>
<th style="margin: 0px; padding: 0px; border: 0px; font-size: medium; vertical-align: baseline; outline: none; text-align: center;">
**Infrastructure**

</th>
<th style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none; text-align: center;">
**<span style="margin: 0px; padding: 0px; border: 0px; font-size: medium; vertical-align: baseline; outline: none;">Regular</span>**

</th>
</tr>
<tr style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none;">
<td style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none;">
/install NServiceBus.Production

</td>
<td style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none; text-align: center;">
√

</td>
<td style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none; text-align: center;">
√

</td>
</tr>
<tr style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none;">
<td style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none;"> NServiceBus.Production

</td>
<td style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none; text-align: center;">
×

</td>
<td style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none; text-align: center;">
×

</td>
</tr>
<tr style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none;">
<td style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none;">
/install NServiceBus.Integration

</td>
<td style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none; text-align: center;">
√

</td>
<td style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none; text-align: center;">
√

</td>
</tr>
<tr style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none;">
<td style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none;"> NServiceBus.Integration

</td>
<td style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none; text-align: center;">
×

</td>
<td style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none; text-align: center;">
√

</td>
</tr>
<tr style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none;">
<td style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none;">
/install NServiceBus.Lite

</td>
<td style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none; text-align: center;">
√

</td>
<td style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none; text-align: center;">
√

</td>
</tr>
<tr style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none;">
<td style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none;"> NServiceBus.Lite

</td>
<td style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none; text-align: center;">
×

</td>
<td style="margin: 0px; padding: 0px; border: 0px; vertical-align: baseline; outline: none; text-align: center;">
√

</td>
</tr>
</tbody>
</table> The installers are controlled by both the /install command line option to the host and the current profile in use. You can, of course, implement your own profile if you have other requirements.

When self hosting NServiceBus, invoke the installers manually, using this:


    Configure.Instance.ForInstallationOn().Install()


Read a [full example](http://github.com/NServiceBus/NServiceBus/blob/master/Samples/AsyncPages/WebApplication1/Global.asax.cs)
.

There is another way to run the infrastructure installers. If you invoke the host with the /installInfrastructure flag, the host runs the infrastructure installers for you without requiring you to configure anything. This can be useful when you set up a new server or a developer machine, since it verifies and installs the required infrastructure for NServiceBus to run. The RunMeFirst.bat file included in the download does just that.

