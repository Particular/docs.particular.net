<!--
title: "Versioning Sample"
tags: ""
summary: "<p>This sample is based on the <a href="how-pub-sub-works.md">Pub/Sub sample</a>. If you have not yet gone through it, please start there.</p>
<p></p>
"
-->

This sample is based on the [Pub/Sub sample](how-pub-sub-works.md). If you have not yet gone through it, please start there.

![Versioning sample](https://particular.blob.core.windows.net/media/Default/images/Versioning.png "Versioning sample")

In this sample, there are two message projects: V1.Messages and V2.Messages:

    namespace V1.Messages
    {
        public interface SomethingHappened : IMessage
        {
            int SomeData { get; set; }
        }
    }

The version 2 message schema inherits from the version 1 schema as shown below, adding another property on top of the properties in the version 1 schema.

    namespace V2.Messages
    {
        public interface SomethingHappened : 
                        V1.Messages.SomethingHappened
        {
            string MoreInfo { get; set; }
        }
    }

There are two subscribers as before, but now one subscriber is subscribed to the version 1 message schema, V1Subscriber; and the other subscriber is subscribed to the version 2 message schema, V2Subscriber.

**NOTE**: Both of the subscribers have an EndpointConfig file that inherits IConfigureThisEndpoint, AsAServer. Subscribers have a message handler for the messages from their respective versions. Yet there is a slight difference in their config files; V1Subscriber has the following in its UnicastBusConfig:






While V2Subscriber has this in its UnicastBusConfig:






The only difference is that each subscriber maps the version of the schema on which it is dependent.

Look at V2Publisher, which is very similar to the publisher from the PubSub sample. The only thing that V2Publisher is doing is publishing a message from the version 2 schema. However, the sample is run, V1Subscriber receives these messages as well:

![Versioning sample running](https://particular.blob.core.windows.net/media/Default/images/Versioning_running.png "Versioning sample running")

**NOTE**: When each subscriber processes the event, each sees it as the schema version it is compiled against. In this manner, publishers can be extended from one version to the next without breaking existing subscribers, allowing new subscribers to be created to handle the additional information in the new version of the events.

