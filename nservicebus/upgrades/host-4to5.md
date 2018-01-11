---
title: NServiceBus Host Upgrade Version 4 to 5
summary: Instructions on how to upgrade NServiceBus Host Version 4 to 5.
component: Host
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 4
 - 5
---


http://apicomparer.particular.net/compare/nservicebus.host/4.7.12...5.0.0 

.NETFramework,Version=v4.5 (Compared To .NETFramework,Version=v4.0)

The following public types have been removed
- NServiceBus.Hosting.Roles.Handlers.TransportRoleHandler
- NServiceBus.Hosting.Windows.Roles.Handlers.ClientRoleHandler
- NServiceBus.Hosting.Windows.Roles.Handlers.PublisherRoleHandler
- NServiceBus.Hosting.Windows.Roles.Handlers.ServerRoleHandler

The following public types have been made internal.

- NServiceBus.Hosting.Windows.Arguments.HostArguments
- NServiceBus.Hosting.Windows.EndpointType
- NServiceBus.Hosting.Windows.EndpointTypeDeterminer
- NServiceBus.Hosting.Windows.HostServiceLocator
- NServiceBus.Hosting.Windows.Installers.WindowsInstaller
- NServiceBus.Hosting.Windows.LoggingHandlers.IntegrationLoggingHandler
- NServiceBus.Hosting.Windows.LoggingHandlers.LiteLoggingHandler
- NServiceBus.Hosting.Windows.LoggingHandlers.ProductionLoggingHandler
- NServiceBus.Hosting.Windows.Profiles.Handlers.PerformanceCountersProfileHandler

The following types have differences.
- NServiceBus.AsA_Publisher - Obsoleted Please use `AsA_Server` instead. Will be removed in version 6.0.0. Obsoleted with error.
- NServiceBus.Hosting.Windows.WindowsHost
  - Methods Removed
    - void .ctor(Type, String[], string, bool, IEnumerable<string>)
    - void Install(string)



## Roles are obsolete

In Versions 5 and above roles are obsoleted and should not be used. The functionality of AsA_Server, and AsA_Publisher have been made defaults in core and can be safely removed. 

https://docs.particular.net/nservicebus/hosting/nservicebus-host/?version=host_5#roles-built-in-configurations

