### ServiceControl.Audit/MaxBodySizeToStore

A size limit, in bytes, for message bodies. Its effect depends on the storage type:

| Storage | Effect |
| --- | --- |
| RavenDB | The largest message body that the Audit instance stores. Larger bodies cannot be viewed in ServicePulse. |
| SQL Server or PostgreSQL | The largest text message body that is stored inline in the database. Larger text bodies, and bodies that are not text, are stored in the [external message body store](/servicecontrol/storage/external-body/) and can be viewed in ServicePulse. A larger text body also keeps its first `MaxBodySizeToStore` bytes in the database for full-text search. See [what is stored where](/servicecontrol/storage/external-body/#inline-storage-and-compression). |

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MAXBODYSIZETOSTORE` |
| **App config key** | `ServiceControl.Audit/MaxBodySizeToStore` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `102400` (100 KB) |
