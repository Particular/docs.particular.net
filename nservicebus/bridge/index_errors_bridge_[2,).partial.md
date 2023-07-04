When the messaging bridge encounters an error while transferring a message between transports it will move the message to the `bridge.error` queue, which is specific to the messaging bridge. An additional header `NServiceBus.MessagingBridge.FailedQ` is added to allow moving a message back to its originating queue, so the messaging bridge can pick them up again.

