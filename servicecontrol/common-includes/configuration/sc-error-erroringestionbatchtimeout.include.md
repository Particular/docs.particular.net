#if-version [7,)
### ServiceControl/ErrorIngestionBatchTimeout

The maximum time to wait for a batch to fill before it is written. Full batches are written immediately; a partial batch can wait up to this time. The default can add up to 100 milliseconds of latency at low message volumes.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_ERRORINGESTIONBATCHTIMEOUT` |
| **App config key** | `ServiceControl/ErrorIngestionBatchTimeout` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| timespan | `00:00:00.100` |

The allowed range is `00:00:00` to `00:00:05`. An explicit value of `00:00:00` is preserved, so partial batches are not delayed.
#end-if
