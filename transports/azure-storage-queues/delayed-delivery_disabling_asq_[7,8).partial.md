
### Disabling delayed delivery

In Versions 7.5 and above, delayed delivery can be turned off to disable unnecessary Azure Storage table polling. Delayed delivery should **not** be turned off if any of the following features are required:

 * Deferred messages
 * Saga timeouts
 * Delayed retries

snippet: delayed-delivery-disabled
