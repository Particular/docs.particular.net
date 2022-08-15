
The feature does not require any additional configuration.

### Behavior

The lock will be renewed 10 seconds before the `LockUntil` value from the message. By default the lock will thus be renewed after 4m50s.

The sample overrides the queue lock duration to 30 seconds for demo purposes which means in the sample the lock is renewed every 20s (30s-10s). The buffer value is hardcoded in the sample.
