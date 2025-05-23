## ServiceControl Transport Adapter Windows Service

> [!WARNING]
> [ServiceControl.TransportAdapter](/nservicebus/upgrades/supported-versions.md#other-packages-servicecontrol-transportadapter) is obsolete. Instead, use the [`NServiceBus.Transport.Bridge`](/nservicebus/bridge).

This template makes it easier to create a [Windows Service](https://docs.microsoft.com/en-us/dotnet/framework/windows-services/introduction-to-windows-service-applications) host for a [ServiceControl Transport Adapter](/servicecontrol/transport-adapter.md).

The template can be used via the following command:

snippet: scadapterservice-usage

This will create a new directory named `MyAdapter` containing a Windows service `.csproj` also named `MyAdapter`.

To add to an existing solution:

snippet: scadapterservice-addToSolution


### Options

snippet: scadapterservice-options

partial: target-framework

> [!NOTE]
> When installing an endpoint created from this template as a service, the `--run-as-service` parameter must be set on the command line. See [Windows Service Installation](/nservicebus/hosting/windows-service.md) for details.
