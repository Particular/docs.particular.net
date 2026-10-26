### LicensingComponent/ServiceControlThroughputDataQueue

Version: 5.4.0+

The queue on which throughput data is received by the ServiceControl Error instance. This setting must match the equivalent [`Monitoring/ServiceControlThroughputDataQueue`](/servicecontrol/monitoring-instances/configuration.md#usage-reporting-monitoringservicecontrolthroughputdataqueue) setting for the Monitoring instance.

In most instances these settings do not need to be modified.

If running multiple setups of the Platform Tools (i.e. multiple versions of ServiceControl error and monitoring instances), then modify these settings so that the queue on each monitoring instance is matched to the queue of its error instance.

If using [MSMQ transport](/transports/msmq) and the monitoring instance is installed on a different machine than the ServiceControl error instance, only the monitoring instance setting needs to be modified to include the machine name of the error instance in the queue address.

| Context | Name |
| --- | --- |
| **Environment variable** | `LICENSINGCOMPONENT_SERVICECONTROLTHROUGHPUTDATAQUEUE` |
| **App config key** | `LicensingComponent/ServiceControlThroughputDataQueue` |

| Type | Default value |
| --- | --- |
| string | `ServiceControl.ThroughputData` |
