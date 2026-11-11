### ServiceControl.Audit/MaxBodySizeToStore

The largest message body, in bytes, that the Audit instance stores. Larger bodies cannot be viewed in ServicePulse.

This applies to RavenDB storage. With SQL Server and PostgreSQL storage, this setting is the maximum size, in bytes, of a text message body stored inline in the database. Larger text bodies, and bodies that are not text, are stored in the [external message body store](/servicecontrol/storage/external-body/) and can be viewed in ServicePulse. A larger text body also keeps its first `MaxBodySizeToStore` bytes in the database for full-text search. See [what is stored where](/servicecontrol/storage/external-body/#inline-storage-and-compression).

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MAXBODYSIZETOSTORE` |
| **App config key** | `ServiceControl.Audit/MaxBodySizeToStore` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `102400` (100 KB) |
