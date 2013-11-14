<!--
title: "NServiceBus Installers"
tags: ""
summary: "<p>NServiceBus has the concept of installers to make sure that endpoint specific specific artifacts e.g., queues, folders, or databases are installed and configured automatically for you if needed at install time.</p>
<p>To create your own installer is as easy as implementing the
<a href="https://github.com/NServiceBus/NServiceBus/blob/master/src/NServiceBus.Core/Installation/INeedToInstallSomething.cs">INeedToInstallSomething<t></a> interface. The generic parameter gives you a way to restrict your installer to a specific platform. Currently this is either Windows or Azure.</p>
"
-->

NServiceBus has the concept of installers to make sure that endpoint specific specific artifacts e.g., queues, folders, or databases are installed and configured automatically for you if needed at install time.

To create your own installer is as easy as implementing the
[INeedToInstallSomething<t>](https://github.com/NServiceBus/NServiceBus/blob/master/src/NServiceBus.Core/Installation/INeedToInstallSomething.cs) interface. The generic parameter gives you a way to restrict your installer to a specific platform. Currently this is either Windows or Azure.

If you don’t care about the runtime environment, just use the
[INeedToInstallSomething](https://github.com/NServiceBus/NServiceBus/blob/master/src/NServiceBus.Core/Installation/INeedToInstallSomething.cs) interface instead.

NServiceBus scans the assemblies in the runtime directory for installers so you don’t need any code to register them.

**Version 3.0 Only:** Version 3.0 included an interface called INeedToInstallInfrastructure<t> interface. It was primarily used for things that are not specific to a given endpoint e.g., RavenDB or MSMQ. This interface has been obsoleted in version 4.0 and will be removed in
5.0, since the introduction of [PowerShell commandlets](managing-nservicebus-using-powershell.md) to aid the installation of infrastructure.

When are they invoked?

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
**Infrastructure (v3.0 Only)**

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


```C#
Bus = NServiceBus.Configure.With()
  .DefaultBuilder()
  .UseTransport<Msmq>()
      .PurgeOnStartup(false)
  .UnicastBus()
  .CreateBus()
  .Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());
```

 NOTE: The use of /installInfrastructure flag with the NServiceBus.Host has been deprecated in version 4.0. To install needed infrastructure, use the [PowerShell commandlets](managing-nservicebus-using-powershell.md) instead.

