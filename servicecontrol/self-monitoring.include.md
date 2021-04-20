ServiceControl includes some basic self-monitoring implemented as [custom checks](/monitoring/custom-checks/). These checks are reported in ServicePulse along with other custom checks.

#### MSMQ transactional dead letter queue

A machine running MSMQ has a single transactional dead letter queue. Messages that cannot be delivered to queues located on remote machines are eventually moved to the transactional dead letter queue. ServiceControl monitors the transactional dead letter queue on the machine it is installed on. The presence of messages in this queue may indicate problems delivering messages for retries.

#### Azure Service Bus staging dead letter queue

Every Azure Service Bus queue has an associated dead letter queue. When ServiceControl sends a message for retry it uses a staging queue. ServiceControl monitors the dead letter queue associated with the staging queue. The presence of messages in the dead letter queue indicates problems delivering messages for retries.

#### Failed imports

When ServiceControl is unable to ingest an audit or error message, an error is logged and the message is stored separately. ServiceControl monitors these messages. For more information, see [re-importing failed messages](/servicecontrol/import-failed-messages.md).

#### Error message ingestion process

When ServiceControl has difficulty connecting to the configured transport, the error message ingestion process is shut down for sixty seconds. These shutdowns are monitored. The time to wait before restarting the error ingestion process is controlled by the [ServiceControl/TimeToRestartErrorIngestionAfterFailure](/servicecontrol/creating-config-file.md#host-settings-servicecontroltimetorestarterroringestionafterfailure) setting.

#### Message database storage space

ServiceControl stores messages in an embedded database. If the drive containing the database runs out of storage space, message ingestion fails and the ServiceControl instance stops. This may cause instability in the database, even after storage space is increased. The remaining storage space on the drive is monitored. The check reports a failure if the drive has less than 20% remaining of its total capacity. This threshold is controlled by the [ServiceControl/DataSpaceRemainingThreshold](/servicecontrol/creating-config-file.md#troubleshooting-servicecontroldataspaceremainingthreshold) setting.
