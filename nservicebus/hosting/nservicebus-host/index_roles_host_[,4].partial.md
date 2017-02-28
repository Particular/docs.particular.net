## Roles - Built-in configurations

While NServiceBus allows picking and choosing which technologies to use and how to configure each of them, the host packages these choices into three built-in options: `AsA_Client`, `AsA_Server`, and `AsA_Publisher`. All these options make use of `XmlSerializer`, `MsmqTransport`, and `UnicastBus`. The difference is in the configuration:

 * `AsA_Client` sets `MsmqTransport` as non-transactional and purges its queue of messages on startup. This means that it starts afresh every time, not remembering anything before a crash. Also, it processes messages using its own permissions, not those of the message sender.
 * `AsA_Server` sets `MsmqTransport` as transactional and does not purge messages from its queue on startup. This makes it fault-tolerant.
 * `AsA_Publisher` extends `AsA_Server` and indicates to the infrastructure to set up storage for subscription requests, described in the [profiles page](/nservicebus/hosting/nservicebus-host/profiles.md).