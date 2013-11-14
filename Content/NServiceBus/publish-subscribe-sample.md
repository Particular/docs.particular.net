<!--
title: "Publish/Subscribe Sample"
tags: 
-->

![pubsub solution](https://particular.blob.core.windows.net/media/Default/images/pub_sub_solution.png)Open the solution in Visual Studio. You should see the picture on the left.

Before running the sample, look over the solution structure, the projects, and the classes. The projects "MyPublisher", "Subscriber1", and "Subscriber2" are their own processes, even though they look like regular class libraries.

The "Messages" project contains the definition of the messages that are sent between the processes. Open the "Messages.cs" class to see the
"EventMessage" class and the "IEvent" interface.

The "MyPublisher" process publishes each of these message alternately, every time you click Enter in its console window.

The "Subscriber1" process subscribes to "EventMessage", while
"Subscriber2" subscribes to the interface "IEvent". Since the
"EventMessage" class implements the "IEvent" interface, when the
"MyPublisher" process publishes "EventMessage", both subscribers receive it.

When running the sample, you'll see three open console applications, and many log messages on each. Almost none of these logs represent messages sent between the processes.

Run the sample
--------------

Run the code and see the three console applications open.

Identify the "MyPublisher" process by its window title, as shown:

![pubsub sample running](https://particular.blob.core.windows.net/media/Default/images/pubsub_nservicebus_running.png "pubsub sample running")

Spread out the various console windows so that you can see all three fully.

Click Enter repeatedly in the "MyPublisher" processes console window, and notice how the messages appear in the other console windows. One click appears on only one other console, and the next click causes messages to be sent to both subscriber consoles.

Let's see what else NServiceBus can do.

Fault-tolerant messaging
------------------------

Pick one of the subscriber processes (say, Subscriber1) and close it. Now go back to the "MyPublisher" process, and click Enter several more times. In Visual Studio, right click the project of the closed subscriber, and start it up again by selecting 'Debug' and then 'Start new instance', as shown:



 ![rerun subscriber](https://particular.blob.core.windows.net/media/Default/images/pubsub_nservicebus_rerun_subscriber.png "rerun subscriber")

See how the subscriber processes all the messages that were sent by the
"MyPublisher" process while it was down. This is how you can be sure that even when processes or machines restart, NServiceBus ensures that your messages don't get lost.

Examine some more failure scenarios.

Durable subscriptions
---------------------

Restart the "MyPublisher" process and bring it up as described above. Click Enter several times in the publisher's console window.

See that the subscribers are no longer receiving these events. You might not have expected this. It is reasonable to assume that a publisher
"remembers" its subscribers even if it restarts. That would require that the publisher stores which events each subscriber wants to receive on some durable medium.

Luckily, NServiceBus has two durable subscription storage options in addition to the default in-memory storage: one makes use of MSMQ, the other makes use of a database. The MSMQ option is suitable for integration environments where you want to test various kinds of fault scenarios but do not require scalability. To scale out a publisher over multiple machines, the MSMQ subscription storage does not work correctly; for that, you need the DB subscription storage.

To switch from the in-memory storage to storage suitable for integration and production, use "profiles", which are preconfigured combinations of infrastructure technologies suitable for various scenarios. Change the publisher's profile to integration.

In Visual Studio, stop debugging, double click the "Properties" node located immediately under the "MyPublisher" process, and click the
"Debug" tab. In the "Start Options" section, in the "Command line arguments" textbox, type "NServiceBus.Integration", as shown:

<center>
![Set the Integration profile](http://images.nservicebus.com/Integration_Profile.png "Set the Integration profile")

</center> Run it again and click Enter several times in the publisher's console, checking that the events show up in the subscribers.

Restart the publisher. Now when you click Enter in the publisher's console, you should see that the subscribers still receive the events.

Next steps
----------

Scale out your publishers and subscribers.

See the other NServiceBus pieces that handle this for you in [how pub/sub works](how-pub-sub-works.md).

