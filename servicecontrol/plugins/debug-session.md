---
title: DebugSession Plugin
summary: 'Improves your debug sessions when using ServiceInsight.'
tags:
- ServiceControl
---

DANGER: **For Development only**. Since this is meant only for use with Visual Studio it adds no value to deploy this plugin to production.

The DebugSession plugin is used for for integrated debugging on a developers machine. The DebugSession is a dedicated plugin targeted for the developer that enables integration for ServiceInsight. It helps to filter out older messages in ServiceInsight that are not part of the current debug session in Visual Studio.

When deployed, the debug session plugin adds a specified debug session identifier to the header of each message sent by the endpoint. This allows messages sent by a debugging or test run within Visual Studio to be correlated, filtered, and highlighted within ServiceInsight.


## NuGets

 * NServiceBus Version 5.x: [ServiceControl.Plugin.Nsb5.DebugSession](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb5.DebugSession)
 * NServiceBus Version 4.x: [ServiceControl.Plugin.Nsb4.DebugSession](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb4.DebugSession)
 * NServiceBus Version 3.x: Not Available


### Deprecated NuGet

If you are using the older version of the plugin, namely **ServiceControl.Plugin.DebugSession** please remove the package and replace it with the appropriate plugin based on your NServiceBus version. This package has been deprecated and unlisted.
