A retry involves the following actions:

 * Read a message from the error queue.
 * Extract the failed queue from the headers.
 * Forward that message to the failed queue name so it can be retried.