#if-version [7,)
### ServiceControl/MessageBody/MinCompressionSize

The minimum message body size, in bytes, at which external message bodies are compressed.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MESSAGEBODY_MINCOMPRESSIONSIZE` |
| **App config key** | `ServiceControl/MessageBody/MinCompressionSize` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `4096` (4 KB) |
#end-if
