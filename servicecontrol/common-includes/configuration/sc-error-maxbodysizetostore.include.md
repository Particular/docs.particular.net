#if-version [7,)
### ServiceControl/MaxBodySizeToStore

The maximum size, in bytes, of a text message body stored inline in the SQL database. Larger text bodies, and binary bodies of any size, are stored using the configured external storage provider. A larger text body also keeps its first `MaxBodySizeToStore` bytes in the database for full-text search. See [what is stored where](/servicecontrol/storage/external-body/#inline-storage-and-compression).

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MAXBODYSIZETOSTORE` |
| **App config key** | `ServiceControl/MaxBodySizeToStore` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `102400` (100 KB) |
#end-if
