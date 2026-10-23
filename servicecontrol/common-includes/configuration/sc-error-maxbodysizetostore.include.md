#if-version [7,)
### ServiceControl/MaxBodySizeToStore

The maximum message body size, in bytes, stored inline in the SQL database. Larger message bodies are stored using the configured external storage provider.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MAXBODYSIZETOSTORE` |
| **App config key** | `ServiceControl/MaxBodySizeToStore` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `102400` (100 KB) |
#end-if
