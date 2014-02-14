---
title: ServiceInsight Overview
summary: A short overview on ServiceInsight.
tags: []
---

If you have ever had to go though the pain of debugging though a distributed application, you know that it is not exactly a trivial task, so we at ParticularSoftware created ServiceInsight to help you with that job. This application provides help on various aspects of debugging and visualizing your NServiceBus based solution. Let' see some of its benefits here.


Fully Integrated Platform
-------------------------
ServiceInsight is just one of the tools we have created on top of NServiceBus all of which create a platform for your distributed applications. You can now have a seamless experience, from designing of your system in ServiceMatrix to finding issues and receiving alerts in ServicePulse to finally investigating more on those alerts and issues in ServiceInsight. You can share the data you have found with your colleagues and team members just by sending them a Message Uri. Upon opening the link to the message Uri, they can see the exact message in the context of the application they are opening it with. 


Visualizing your distributed system
-----------------------------------
If you want to see your message payloads, you don't need to use other platform specific tools to see the messages in your queues, ServiceInsight provides formatted view of the messages for XML and JSON and binary messages. Another ServiceInsight feature is that it gives you a visual overview of the messages in your system. You can use various diagrams to see the message flow in your distributed system, see who originated a command or raised an event and what messages were created as a result of that and by which endpoints.

![Flow Diagram](004_flowdiagram.png)


Error handling and retries
--------------------------
Error handling is another example of a type of work that is hard to do on a distributed system. Although NServiceBus is designed with this in mind, ServiceInsight builds on top of that and gives you more fine-grain information about the error, exception information and stack trace. Error messages can be seen clearly in message flow, to see what type of work flow has caused a message to fail and also you can filter the messages in the message list based on their failure status. This status is inline with First and Second Level Retries. Once the issue is resolved the ability to have the message retried is just a click away. Say goodbye to running scripts, or using command prompt tools to have your messages retried.

![Message ContextMenu](002_messagemenu.png)

Endpoint Interactions
---------------------
ServiceInsight gives you a clear representation of message processing Endpoints in your distributed application. The endpoint explorer, sets the context for message list display and search operation, meaning you can perform those operation by selecting a specific endpoint or on all the available endpoints (by selecting the parent ServiceControl node). The explorer also has information about the physical location of the Endpoint.

![Endpoint Explorer](006_endpointexplorer.png)


Message Properties
------------------
Even though you might have a queue specific tools that you use to view the message payload, some of the NServiceBus messages metadata used by the framework is stored in the message header. ServiceInsight provides easy access to all those properties where you can easily find the information you are looking for. You can filter and search for a specific property and it looks and behaves like your favorite IDE's property grid so you'll feel right at home.

![Message Properties](003_messageproperties.png)

Timing and Performance
----------------------
There are quite a few performance related information in the message header. You can sort all the messages in an endpoint based on the time the messages were sent, critical time, processing time or delivery time. The sort operation is done on the backend and is not limited to the information that is displayed on the screen only, but to all the messages that are passed from an endpoint. These information combined with Message Type and Id give you an opportunity to easily locate the messages have taken a lot of time to process, or say if a particular message occasionally takes more time to get delivered.

![Message List And Search Bar](005_messagelist.png)

Search and Sort
---------------
Initial version of ServiceInsight also has a search feature where you can perform full-text search over all the messages across all the endpoints. This is a handy feature if you want to search for all the correlated messages where using conventional tools might not be possible. You can either search for a piece of informat ion in the message contents or a value in the message header. You can also sort the message list on any of the available columns. As mentioned before, the filtering, sorting and searching is all done on all the messages on a selected endpoint, or across all the endpoints, if no endpoint is selected.


Auto-Refresh and live update
----------------------------
The auto refresh feature would automatically refresh the view of your distributed system, be that the message list or the flow diagram, and it is performant so you get the updates almost at real-time when a message is processed in any of the endpoints. 

Supporting all transports
-----------------------------
Latest version of NServiceBus supports various transport protocols. Be it MSMQ, ActiveMQ, RabbitMQ or even non queue based transports like SQL Server, now all you need is ServiceInsight at your tool belt to see what's going on in your distributed system.  


Next steps
----------

-   [Installation and Troubleshooting](getting-started--installation-and-troubleshooting.md)

