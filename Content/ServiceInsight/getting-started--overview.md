---
title: ServiceInsight Overview
summary: A short overview on ServiceInsight.
tags: 
- ServiceInsight

---

If you have ever gone though the pain of debugging through a distributed application, you know that it is not exactly a trivial task, so we at Particular Software created ServiceInsight to help you with that job. This application helps with various aspects of debugging and visualizing your NServiceBus based solution. Let's see some of its benefits.


Fully integrated platform
-------------------------
ServiceInsight is just one of the tools we have created on top of NServiceBus, all of which create a platform for your distributed applications. You can now have a seamless experience, from designing your system in ServiceMatrix, to finding issues and receiving alerts in ServicePulse, to finally investigating those alerts and issues in ServiceInsight. You can share the data you have found with your colleagues and team members just by sending them a Message URi. Upon opening the link to the message URi, they can see the exact message in the context of the application they use to open it. 


Visualizing your distributed system
-----------------------------------
To see your message payloads in your queues, you don't need other platform-specific tools, because ServiceInsight provides a formatted view of the messages for XML, JSON, and binary messages. Another ServiceInsight feature is that it gives you a visual overview of the messages in your system. You can use various diagrams to see the message flow in your distributed system, see who originated a command or raised an event and what messages were created as a result, by which endpoints.

![Flow Diagram](images/004_flowdiagram.png)


Error handling and retries
--------------------------
Error handling is another example of a type of work that is hard to do on a distributed system. Although NServiceBus is designed with this in mind, ServiceInsight builds on top of it and gives you more fine-grained information about the error, exception information, and stack trace. Error messages can be seen clearly in the message flow, to see what type of work flow caused a message to fail. You can also filter the messages in the message list based on their failure status, which is inline with First and Second Level Retries. Once the issue is resolved the ability to retry the message is just a click away. Say goodbye to running scripts or using command prompt tools to retry your messages.

![Message ContextMenu](images/002_messagemenu.png)

Endpoint interactions
---------------------
ServiceInsight gives you a clear representation of message processing endpoints in your distributed application. The endpoint explorer sets the context for message list display and search operations, meaning you can perform the operation by selecting a specific endpoint or all the available endpoints (by selecting the parent ServiceControl node). The explorer also provides information about the physical location of the endpoint.

![Endpoint Explorer](images/006_endpointexplorer.png)


Message properties
------------------
Even though you might have queue-specific tools for viewing the message payload, some of the NServiceBus messages metadata used by the framework is stored in the message header. ServiceInsight provides easy access to all those properties where you can easily find your information. You can filter and search for a specific property and it looks and behaves like your favorite IDE property grid so you'll feel right at home.

![Message Properties](images/003_messageproperties.png)

Timing and performance
----------------------
There is a lot of performance-related information in the message header. You can sort all the messages in an endpoint based on the time the messages were sent, critical time, processing time, or delivery time. The sort operation is done on the backend and is not limited to the information displayed on the screen only, but to all the messages passed from an endpoint. This information, combined with Message Type and ID, give you an opportunity to easily locate the messages that have taken too long to process, or if a particular message occasionally takes more time to get delivered.

![Message List And Search Bar](images/005_messagelist.png)

Search and sort
---------------
The initial version of ServiceInsight also has a search feature where you can perform full-text searches over all the messages across all the endpoints. This is a handy feature if you want to search for all the correlated messages where using conventional tools might not be possible. You can either search for a piece of information in the message contents or a value in the message header. You can also sort the message list on any of the available columns. As mentioned before, the filtering, sorting, and searching are done on all the messages on a selected endpoint, or across all the endpoints if no endpoint is selected.


Auto-refresh and live update
----------------------------
The auto-refresh feature automatically refreshes the view of your distributed system, whether it is the message list or the flow diagram, and it is performant so you get the updates almost at real-time when a message is processed in any of the endpoints. 

Supporting all transports
-----------------------------
The latest version of NServiceBus supports various transport protocols. For MSMQ, ActiveMQ, RabbitMQ, or even non-queue based transports such as SQL Server, now all you need is ServiceInsight in your toolkit to see what's going on in your distributed system.  
