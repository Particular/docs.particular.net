It is possible to renew the lock automatically for longer than the default by overriding the `MaxAutoLockRenewalDuration`.

snippet: custom-auto-lock-renewal

NOTE: Message lock renewal is initiated by client code, not the broker. If the request to renew the lock fails after all the SDK built-in retries (.e.g due to a connection-loss), the lock won't be renewed, and the message will become unlocked and available for processing by competing consumers. Lock renewal should be treated as best-effort and not as a guaranteed operation.