
WARNING: Make sure *all* queues of all endpoints are drained and empty, including any failed messages in the error queue or messages that need to be retried in the ServiceControl queue. NServiceBus Versions 5 and earlier only support a single serializer/deserializer. Live migration is only supported in NServiceBus Versions 6 and above.
