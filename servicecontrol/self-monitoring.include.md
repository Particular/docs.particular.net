ServiceControl includes some basic self-monitoring implemented as [custom checks](/monitoring/custom-checks/). These checks will be reported in ServicePulse alongside any custom checks being reported from endpoints.

#### MSMQ transactional dead letter queue

MSMQ servers have a single transactional dead letter queue. Messages that cannot be delivered to queues located on remote servers will eventually be moved to the transactional dead letter queue when the MSMQ service is unable to deliver the message. ServiceControl will monitor the transactional dead letter queue on the server it is installed on as the presence of messages in this queue may indicate a problem with delivering message retries.

#### Azure Service Bus staging dead letter queue

Azure Service Bus queues each come with an associated dead letter queue. When ServiceControl sends a message for retry it utilizes a staging queue to do so. ServiceControl will monitor the dead letter queue of the ServiceControl staging queue as the presence of messages in this queue indicates a problem with delivering message retries.

#### Failed imports

When ServiceControl is unable to properly import an audit or error message, the error is logged and the message is stored separately in ServiceControl. ServiceControl will monitor these failed import stores and notify when any are found. For more information, see [re-importing failed messages](/servicecontrol/import-failed-messages.md).

#### Error Message Ingestion Process

When ServiceControl has difficulty connecting to the configured transport, the error message ingestion process is shut down for sixty seconds. These shutdowns are monitored with a custom check. The time to wait before restarting the error ingestion process is configurable with the [ServiceControl/TimeToRestartErrorIngestionAfterFailure](/servicecontrol/creating-config-file.md#host-settings-servicecontroltimetorestarterroringestionafterfailure) setting.

#### Message database storage space

ServiceControl stores messages in an embedded database on the local file system. If the disk runs out of space, message ingestion will fail and the ServiceControl instance will stop. This type of failure can cause instability in the database, even after storage space has been increased. ServiceControl will monitor the remaining storage space on the drive containing the embedded message store. The check will report a failure if the hard drive has less than 20% remaining of its total capacity. This threshold can be changed with the [ServiceControl/DataSpaceRemainingThreshold](/servicecontrol/creating-config-file.md#troubleshooting-servicecontroldataspaceremainingthreshold) setting.
