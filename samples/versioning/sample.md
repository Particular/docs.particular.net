---
title: Versioning
summary: Message evolution over time using interfaces.
reviewed: 2016-03-21
component: Core
redirects:
 - nservicebus/versioning-sample
---

In this sample there are two message projects: `V1.Messages` and `V2.Messages`:

snippet:V1Message

The Version 2 message schema inherits from the version 1 schema as shown below, adding another property on top of the properties in the version 1 schema.

snippet:V2Message

There are two subscribers as before, but now one subscriber is subscribed to the version 1 message schema, `V1Subscriber`; and the other subscriber is subscribed to the Version 2 message schema, `V2Subscriber`.

NOTE: Subscribers have a message handler for the messages from their respective versions. Yet there is a slight difference in their config files; `V1Subscriber` has the following in its `UnicastBusConfig`:

snippet:V1SubscriberMapping

While `V2Subscriber` has this in its `UnicastBusConfig`:

snippet:V2SubscriberMapping

The only difference is that each subscriber maps the version of the schema on which it is dependent.

Look at `V2Publisher`, which is very similar to the publisher from the PubSub sample. The only thing that `V2Publisher` is doing is publishing a message from the Version 2 schema. However, the sample is run, `V1Subscriber` receives these messages as well:


## Publisher output

```no-highlight
Press 'Enter' to publish a message, Ctrl + C to exit.
Published event.
```


## V1Subscriber output

```no-highlight
Press any key to stop program
Something happened with some data 1 and no more info
```


## V2Subscriber output

```no-highlight
Press any key to stop program
Something happened with some data 1 and more information It's a secret.
```

NOTE: When each subscriber processes the event, each sees it as the schema version it is compiled against. In this manner, publishers can be extended from one version to the next without breaking existing subscribers, allowing new subscribers to be created to handle the additional information in the new version of the events.