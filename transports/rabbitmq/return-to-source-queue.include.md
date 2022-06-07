## Return message to source queue

### The retry helper methods

This code shows an example of how to perform the following actions:

* Read a message from the error queue.
* Extract the failed queue from the headers.
* Forward that message to the failed queue name so it can be retried.

snippet: rabbit-return-to-source-queue

### Using the retry helper methods

snippet: rabbit-return-to-source-queue-usage
