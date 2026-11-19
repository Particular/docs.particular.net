### ServiceControl.Audit/Port

The port to bind the embedded HTTP API server.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_PORT` |
| **App config key** | `ServiceControl.Audit/Port` |
| **SCMU field** | `PORT NUMBER` |

| Type | Default value |
| --- | --- |
| int | `44444` |

> [!WARNING]
> If the `ServiceControl.Audit/Port` setting is changed, and the `ServiceControl.Audit/DbPath` setting is not set, the path of the embedded RavenDB is changed. Refer to [Customize RavenDB Embedded Location](/servicecontrol/storage/ravendb/configure-ravendb-location.md).
