## Non-versioned subscriptions support

As of `NServiceBus.RavenDB` V4.2.0 support for non-versioned subscriptions has beed introduced. Non-versioned subscription are an opt-in feature due to their incompatibility with the default subscription storage behavior. To enable support for storing subscriptions without storing the message version change endpoint configuration as follows:

```
<insert code snippet here>
```

NOTE: Due to the breaking nature of this feature it is important to take into account the impact of enabling it on pre-existing installations. Once enabled all the stored subscriptions, if any, will be immidiately ignored and publishers will stop publishing events until all subscribers are restarted and new subscription requests reach the pusblisher endpoint. Opting in this new feature has no effects for newly deployed publisher endpoints that never received any subscription requests.
