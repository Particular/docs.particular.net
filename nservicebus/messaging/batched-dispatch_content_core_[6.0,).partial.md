In Versions 6 and above, NServiceBus will collect all outgoing operations (e.g. commands, responses, events) that happen as part of processing a message and pass them on to the transport after message handling pipeline has completed. This has two main benefits:

 * Business data will always be committed to storage before any outgoing operations are dispatched. This ensures that there are no outgoing messages in case an exception occurs in in the message handler.
 * Allows transports to improve performance by batching outgoing operations. Since transports get access to all outgoing messages in one go they can optimize communication with the underlying queuing infrastructure to minimize round trips.
