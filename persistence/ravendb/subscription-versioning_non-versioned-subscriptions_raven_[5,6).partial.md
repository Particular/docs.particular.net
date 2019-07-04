## Non-versioned subscriptions support

In Versions 4.2 and above support for non-versioned subscriptions has been introduced. Non-versioned subscription are an opt-in feature due to their incompatibility with the default subscription storage behavior. To enable support for storing subscriptions without storing the message version change endpoint configuration as follows:

snippet: DisableSubscriptionVersioning

NOTE: Due to the breaking nature of this feature it is important to take into account the impact of enabling it on pre-existing installations. Once enabled all the stored subscriptions, if any, will be immediately ignored and publishers will stop publishing events until all subscribers are restarted and new subscription requests reach the publisher endpoint. Opting in this new feature has no effects for newly deployed publisher endpoints that never received any subscription requests.

In Version 5.0 and above choosing a subscription versioning strategy is required to use RavenDB subscription storage. In order to continue using legacy versioned subscriptions, change the endpoint configuration as follows:

snippet: LegacySubscriptionVersioning

WARNING: If neither subscription versioning option is selected, an exception will be thrown.
