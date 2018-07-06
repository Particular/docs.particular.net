## Customizing the session

Version 6 supports customizing the instantiation of the `ISession`. This is done by hooking up to the creation process by providing a custom delegate:

snippet: CustomSessionCreation

NOTE: Customizing the way a session is opened works only for the 'shared' session that is used to access business, [saga](/nservicebus/sagas/), and [outbox](/nservicebus/outbox/) data. It does not work for other persistence concerns such as [timeouts](/nservicebus/sagas/timeouts.md) or [subscriptions](/nservicebus/messaging/publish-subscribe/). Also note that this is no longer possible in version 7.