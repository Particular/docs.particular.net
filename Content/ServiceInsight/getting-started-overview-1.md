---
title: ServiceInsight Overview
summary: A short overview on ServiceInsight.
tags: 
- ServiceInsight

---
#Introduction

The [NServiceBus](../NServiceBus/overview.md "NServiceBus Overview") platform can provide your system with all the benefits of a distributed, messaged based, fault tolerant and distributed architecture.  Visualizing and verifying the functionality of such a distributed system can be real challenge.  

From design all the way through to production, ServiceInsight lives up to its name by providing powerful insight and information about the system in a concise and user friendly way. 

Let's review some of the features and benefits...

1.  [ServiceInsight and the NServiceBus Platform](#the-nservicebus-platform "The NServiceBus Platform")
2.  [Visualizing The System](#visualizing-the-system "Visualizing your system in ServiceInsight")
3.  [The Message Window](#the-message-window "The Message Window")
4.  [Endpoint Explorer](#endpoint-explorer "Endpoint Explorer")
5.  [Flow Diagram](#flow-diagram "The graphical flow diagram")
6.  [Message Properties](#message-properties "The Message Properties Window")
7.  [Saga View](#saga-view "The Saga View")
8.  [Body and Log Views](#body-and-log-views "The Body and Log Tabs")
9.  [Errors and Retries](#errors-and-retries "Visualizing and Dealing with Errors")
10. [Next Steps](#next-steps "Next Steps")

##The NServiceBus Platform
ServiceInsight is just one of the tools created to complement NServiceBus from design all the way through IT operations.  [ServiceMatrix](../ServiceMatrix/index.md "ServiceMatrix") will accelerate the design of your system.  The audit and error aggregation power of [ServiceControl](../ServiceControl/index.md "ServiceControl") is leveraged by [ServicePulse](../ServicePulse/index.md "ServicePulse") for operational monitoring and alerting. ServiceInsight provides the detailed view of messages in their system context. From endpoints to sagas, ServiceInsight will show the relationships and data.  

#Visualizing the System
The ServiceInsight user interface provides multiple views of your distributed system.  Using information collected in ServiceControl, ServiceInsight is able enumerate your endpoints and provide detailed message and relationship data.  Platform specific tools are no longer needed thanks to the formatted view of XML, JSON and binary messages.   

A detailed visual overview of the messages collated by conversation is provided in the flow diagram.  Using this view you can see the flow in your system and which endpoints raised specific events and sent or handled messages.  The Saga view provides an singular view into the start of a saga, the timeouts and interactions. 

As you select endpoints the other views within ServiceInsight will respond and show only messages pertaining to that endpoint. 
##The Message Window
Visibility and access to messages details is critical in a distributed NServiceBus system.  The Messages window provides a detailed grid view indicated the status of a message, type, time stamps and service level information.  The list can be filtered based on specific message content.  Searches include all message data, not just the common fields that are displayed.    

This ability to filter and locate is very helpful at design time.  When used in conjunction with ServiceMatrix debugging, ServiceInsight will be launched automatically and will filter the messages to include only your current debug session.

Similarly, the ServicePulse monitoring platform provides the ability to open ServiceInsight to a specific message in order to drill in for more detail.  

![Message List View](images/overview-messagedetailwindow.png)

This message view list has a relationship to the other panels in the user interface.  When you select specific messages the related properties window and flow diagram will change to illustrate details of the selected message. 
##Endpoint Explorer
The Endpoint Explorer indicates the connection to the ServiceControl instance providing data to ServiceInsight.  Underneath it the tree-view enumerates the endpoints contained in the system.

![Endpoint Explorer](images/overview-endpointexplorer.png)

The message list described above is filtered based on your endpoint selection.  If you select a specific endpoint the message list will only list messages handled by that endpoint.   Selecting the root ServiceControl connection and the tree view will expand the list to include all messages.

##Flow Diagram
A picture is said to be worth 1000 words.   The flow diagram provides extensive message and system information in a single picture.
As you select messages in message list the flow diagram illustrates the message but it doesn't stop there.  All related messages from the same conversation are illustrated along with the nature of the messages and the endpoints involved.

![The Flow Diagram](images/overview-flowdiagram-wpopup.png)

Each message is represented by a box indicating the message type and illustrating a variety of useful details including the endpoints involved and time information.  Published events and sent commands have diferent icons and illustrations. Dropdowns provide even more message detail and convenient links to search based on this message, copy message details and even retry messages.

##Message Properties
Each message in NServiceBus contains extensive detail.  As you select messages in the flow diagram or in the list an additional Message Properties window provides a listing of all the detailed properties for the message.  Much like in Visual Studio, the properties are structured in convenient groups.
![The Message Properties Window](images/overview-messageproperties.png)

##The Saga View
Saga can play a critical role in an NServiceBus system.  As coordinators of processes they are started by certain messages and can interact with a variety of additional messages and services.  Once again, ServiceInsight provides a graphical view for sagas that provide an efficient illustration of what is happening.  By clicking on a message in the flow diagram that is handled by a saga, the view is opened.  
![The Saga View](images/overview-sagaview.png)

The saga illustrates not only how the saga was initiated, it illustrates any other messages that were sent or handled.  Detailed message data, time information, and even the details on saga data changes are conveniently visible. 




