### ServiceControl.Audit/HostName

The hostname to bind the embedded HTTP API server to; modify this setting to bind to a specific hostname, eg. `sc.mydomain.com` and make the machine remotely accessible.

This field can also contain a `*` as a wildcard to allow remote connections that use any hostname.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_HOSTNAME` |
| **App config key** | `ServiceControl.Audit/HostName` |
| **SCMU field** | `HOST NAME` |

| Type | Default value |
| --- | --- |
| string | `localhost` |

> [!WARNING]
> If the `ServiceControl.Audit/HostName` setting is changed, and the `ServiceControl.Audit/DbPath` setting is not set, the path of the embedded RavenDB is changed. Refer to [Customize RavenDB Embedded Location](/servicecontrol/storage/ravendb/configure-ravendb-location.md).

#if-version [5,)
> [!NOTE]
> Changing the `ServiceControl.Audit/HostName` setting does not affect how the embedded RavenDB database is exposed. The embedded RavenDB database remains accessible only via `localhost`.
#end-if
