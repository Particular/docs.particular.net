Messages with Time-To-Be-Received (TTBR) specified are pushed through the message pump, just like regular messages, regardless of if their TTBR already elapsed or not. The receive query compares the current UTC time in the database with the `Expires` column value and sets a flag indicating if a given message has expired. When the message is received from the database that flag is checked and if it is set, the message is dropped and not passed into the processing pipeline. Setting the flag on the database level prevents the TTBR feature from being affected by potential clock drift of machines running sender and receiver endpoints.

In addition to that, expired messages can be removed from the queue before the endpoints start up. In this case the transport removes all expired messages, in batches, using `delete` statements. Each batch is - by default - limited to 10'000 messages (that setting is configurable). The receiver is started when there are no messages to remove. 

To enable start-up expired message purging use:

snippet: purge-expired-on-startup
