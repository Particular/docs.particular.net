## NServiceBus

Starting in version 10.2, NServiceBus can use a non-cryptographic hash algorithm (XxHash128) to generate deterministic unique identifiers for endpoints, also known as host identifiers (HostIds). Because XxHash128 is not a cryptographic algorithm, FIPS policy enforcement does not block host identifier generation for this code path; the host ID workaround described below is no longer needed.

To ensure the non-cryptographic hash is used in NServiceBus 10, set the following AppContext switch before endpoint startup:

```csharp
AppContext.SetSwitch("NServiceBus.Core.Hosting.UseV2DeterministicGuid", true);
```

Or via environment variable:

```text
DOTNET_NServiceBus_Core_Hosting_UseV2DeterministicGuid=true
```

Or via MSBuild in a project file:

```xml
<ItemGroup>
  <RuntimeHostConfigurationOption Include="NServiceBus.Core.Hosting.UseV2DeterministicGuid" Value="true" />
</ItemGroup>
```

> [!WARNING]
> Changing the host identifier algorithm changes the host ID that identifies an endpoint in ServicePulse and ServiceControl. Changes to the algorithm will cause existing known endpoints to appear inactive in the ServicePulse [heartbeats](/monitoring/heartbeats/in-servicepulse.md) and [monitoring](/monitoring/metrics/in-servicepulse.md) views while new instances (with the changed host identifiers) appear in their place. The changed host identifier also affects any custom logging, audit processing, dashboards, or queries that use the generated `$.diagnostics.hostid` value. Stale instances should be [removed from the monitoring view](/monitoring/metrics/in-servicepulse.md#disconnected-endpoints-removing-disconnected-endpoints).
>
> See the [NServiceBus version 10 to 11 upgrade guide](/nservicebus/upgrades/10to11/#host-identifier-algorithm-change) for more information on migrating from MD5 to the XxHash128 hash algorithm.
