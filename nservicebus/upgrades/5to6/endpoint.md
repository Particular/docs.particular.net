---
title: Endpoint API changes in Version 6
reviewed: 2016-10-26
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


## [Endpoint](/nservicebus/endpoints/) Name is mandatory

In Versions 6 and above endpoint name is mandatory.

snippet: 5to6-endpointNameRequired

The endpoint name is used as a logical identifier when sending or receiving messages. It is also used for determining the name of the input queue the endpoint will be bound to. See [Derived endpoint name](#endpoint-name-helper) for the algorithm used in Versions 5 and below to select endpoint name if backwards compatibility is a concern.


## Interface Changes

The `IWantToRunWhenBusStartsAndStops` interface is now obsolete.


### [Self Hosting](/nservicebus/hosting/#self-hosting)

When self-hosting, call any startup code after `Endpoint.Start` or any cleanup code after `Endpoint.Stop`.

snippet: 5to6-endpoint-start-stop

While the [Dispose Pattern](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/dispose-pattern) can no longer be used (since `IEndpointInstance` does not implement [IDisposable](https://msdn.microsoft.com/en-us/library/system.idisposable.aspx)) this is actually not a common use case since in most [hosting scenarios](/nservicebus/hosting/) the startup code is not in the same method as the shutdown code. For example

 * [Windows Service Hosting](/nservicebus/hosting/windows-service.md) where startup is usually done in [ServiceBase.OnStart](https://msdn.microsoft.com/en-us/library/system.serviceprocess.servicebase.onstart.aspx) and shutdown is usually done in [ServiceBase.OnStop](https://msdn.microsoft.com/en-us/library/system.serviceprocess.servicebase.onstop.aspx).
 * [Web Site or Web Service Hosting](/nservicebus/hosting/web-application.md) where startup is usually done in [HttpApplication.Application_Start](https://msdn.microsoft.com/en-us/library/ms178473.aspx) and shutdown is usually done in [HttpApplication.Dispose](https://msdn.microsoft.com/en-us/library/system.web.httpapplication.dispose.aspx).

If the extensibility provided by `IWantToRunWhenBusStartsAndStops` is still required it can be achieved via other means, for example, [using MEF or Reflection to customize NServiceBus](/samples/plugin-based-config/).


### Using [NServiceBus.Host](/nservicebus/hosting/nservicebus-host/)

See the upgrade guide for more details on [using the new interface](/nservicebus/upgrades/host-6to7.md) provided by the host.


### Using [AzureCloudService Host](/nservicebus/hosting/cloud-services-host/)

See the upgrade guide for more details on [using the new interface](/nservicebus/upgrades/acs-host-6to7.md) provided by the host.


## Endpoint Name helper

This algorithm is used in Version 5 of NServiceBus to determine Endpoint Name if none is provided. So if the same behavior is is desired in Versions 6 and above this helper class can be used.

snippet: 5to6EndpointNameHelper