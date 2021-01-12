---
title: Versioning
summary: Message evolution over time using interfaces.
reviewed: 2021-01-01
component: Core
redirects:
 - nservicebus/versioning-sample
---

This sample shows how to handle message schema evolution in a backward-compatible manner. The project consists of a publishing endpoint that has evolved from one version of the schema to the next. The newer subscriber has access to the additional information in the newest version of the scheam while the older keeps operating without interruptions.

In this sample there are two message projects: `V1.Messages` and `V2.Messages`:

snippet: V1Message

The Version 2 message schema inherits from the Version 1 schema as shown below, adding another property on top of the properties in the Version 1 schema.

snippet: V2Message

There are two subscribers, one subscribed to the version 1 message schema, `V1Subscriber`; and the other subscribed to the Version 2 message schema, `V2Subscriber`.

NOTE: Subscribers have a message handler for the messages from their respective versions. Yet there is a slight difference in their subscriptions configuration; `V1Subscriber` has:

snippet: V1SubscriberMapping

While `V2Subscriber` has:

snippet: V2SubscriberMapping

The only difference is that each subscriber declares the version of the schema on which it dependents.

`V2Publisher` is publishing a message from the Version 2 schema only. However, when the project is run, `V1Subscriber` receives these messages as well:


## Publisher output

```
Press 'Enter' to publish a message, Ctrl + C to exit.
Published event.
```


## V1Subscriber output

```
Press any key to stop program
Something happened with some data 1 and no more info
```


## V2Subscriber output

```
Press any key to stop program
Something happened with some data 1 and more information It's a secret.
```
