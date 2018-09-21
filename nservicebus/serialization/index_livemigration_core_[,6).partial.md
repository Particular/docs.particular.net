
WARNING: Make sure *all* queues of all endpoints are drained and empty, including any failed messages in the error queue and messages that need to be retried in the ServiceControl queue. NServiceBus versions 5 and earlier only support a single serializer/deserializer. Live migration is only supported in NServiceBus versions 6 and above.
