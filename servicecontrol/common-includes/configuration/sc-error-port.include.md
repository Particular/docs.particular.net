### ServiceControl/Port

The port to bind the embedded HTTP API server.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_PORT` |
| **App config key** | `ServiceControl/Port` |
| **SCMU field** | `PORT NUMBER` |

| Type | Default value |
| --- | --- |
| int | `33333` |

> [!WARNING]
> If the `ServiceControl/Port` setting is changed, and the `ServiceControl/DbPath` setting is not set, the path of the embedded RavenDB is changed. Refer to [Customize RavenDB Embedded Location](/servicecontrol/storage/ravendb/configure-ravendb-location.md).
