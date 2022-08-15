
snippet: LockRenewalFeature

The Azure Service Bus transport sets `LockDuration` to 5 minutes by default, so the default `LockDuration` for the feature has the same value. `ExecuteRenewalBefore` is a `TimeSpan` specifying how soon to attempt lock renewal before the lock expires. The default is 10 seconds. Both settings may be overridden using the `EndpointConfiguration` API.

snippet: override-lock-renewal-configuration

In the sample, `LockDuration` is set to 30 seconds, and `ExecuteRenewalBefore` is set to 5 seconds.

### Behavior

The `LockRenewalFeature` uses the two settings to register the `LockRenewalBehavior` pipeline behavior. With `LockDuration` set to 30 seconds and `ExecuteRenewalBefore` set to 5 seconds, the lock will be renewed every 25 seconds (`LockDuration` - `ExecuteRenewalBefore`).
