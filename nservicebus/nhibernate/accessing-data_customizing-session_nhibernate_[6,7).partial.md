## Customizing the session

Version 6 supports customizing the instantiation of the `ISession`. This is done by hooking up to the creation process by providing a custom delegate:

snippet:CustomSessionCreation

NOTE: Customizing the way session is opened works only for the 'shared' session that is used to access business/user, [Saga](/nservicebus/sagas/) and [Outbox](/nservicebus/outbox/) data. It does not work for other persistence concerns such as [Timeouts](/nservicebus/sagas/timeouts.md) or [Subscriptions](/nservicebus/messaging/publish-subscribe/). Also note that this is no longer possible in Version 7.