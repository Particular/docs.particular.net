#if-version [7,)
### ServiceControl/MessageBody/FileSystem/DataSpaceRemainingThreshold

The percentage of disk space that must remain available on the drive containing the message body storage directory. The value must be between `0` and `100`.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_MESSAGEBODY_FILESYSTEM_DATASPACEREMAININGTHRESHOLD` |
| **App config key** | `ServiceControl/MessageBody/FileSystem/DataSpaceRemainingThreshold` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `15` (percent) |
#end-if
