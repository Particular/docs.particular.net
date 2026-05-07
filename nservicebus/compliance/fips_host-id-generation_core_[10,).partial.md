## NServiceBus

Starting in version 10.2, NServiceBus uses a non-cryptographic hash algorithm (XxHash128) to generate deterministic unique identifiers for endpoints, also known as HostIds. This algorithm is not subject to FIPS policy enforcement and eliminates the need for workarounds on FIPS-enabled systems.

### Migrating from MD5 to XxHash128 host identifiers

The new XxHash128-based algorithm produces different host identifiers than the legacy MD5-based algorithm. To avoid duplicate endpoint entries in [ServicePulse](/servicepulse/) after upgrading, the legacy MD5 algorithm remains the default in version 10.2.

To opt into the new XxHash128-based host identifier, set the following AppContext switch before endpoint startup:

```csharp
AppContext.SetSwitch("NServiceBus.Core.Hosting.UseV2DeterministicGuid", true);
```

Or via environment variable (.NET 9+):

```text
DOTNET_NServiceBus_Core_Hosting_UseV2DeterministicGuid=true
```

Or via MSBuild in the project file:

```xml
<ItemGroup>
  <RuntimeHostConfigurationOption Include="NServiceBus.Core.Hosting.UseV2DeterministicGuid" Value="true" />
</ItemGroup>
```

> [!WARNING]
> Changing the host identifier algorithm changes the host ID that identifies an endpoint in ServicePulse and ServiceControl. Only enable this switch after coordinating with operations teams to ensure monitoring continuity. Endpoints that change their host identifier will appear as new entries in ServicePulse until stale instances are [removed from the monitoring view](/monitoring/metrics/in-servicepulse.md#disconnected-endpoints-removing-disconnected-endpoints).

In version 11, the XxHash128 algorithm will become the default, and the legacy MD5 algorithm will require explicit opt-out. In version 12, the legacy MD5 algorithm will be removed entirely.

### Manual host identifier override (legacy approach)

Before version 10.2, the only way to run on FIPS-enabled systems was to provide a FIPS-compliant hash and [override the HostId](/nservicebus/hosting/override-hostid.md):

snippet: SHA1DeterministicGuid

snippet: HostIdFixer