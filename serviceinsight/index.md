---
title: ServiceInsight
reviewed: 2021-01-18
component: ServiceInsight
isLearningPath: true
redirects:
- serviceinsight/getting-started-overview
- servicepulse/introduction-and-installing-servicepulse
---

include: serviceinsight


## Visualizing the system

The ServiceInsight user interface provides multiple views of a distributed system. Using information collected in ServiceControl, ServiceInsight enumerates the endpoints and provides detailed message and relationship data, with formatted views of XML, JSON, and binary messages.

The flow diagram provides a detailed visual overview of the messages, collated by conversation. This view shows the flow in the system, which endpoints raised specific events, and sent or handled messages. The Saga view illustrates the start of a saga, the timeouts, and interactions. The sequence diagram shows the order in which messages have been sent and handled by endpoints in the system.

As endpoints are selected, the other views within ServiceInsight respond and filter the information to show only messages pertaining to the selected endpoint.

NOTE: Endpoint lists, message information, and message flows will not be populated until messages have been successfully processed by ServiceControl.

### Relationship between ServiceInsight, ServiceControl, and Endpoints

```mermaid
graph RL
EP[Endpoint]-- Audits/SagaAudits -->AQ
EP-- Errors -->EQ
EP-- SagaAudits -->SCQ
AQ(Audit Queue)-- Ingests -->SC
EQ(Error Queue)-- Ingests -->SC
SCQ(ServiceControl Queue)-- Ingests -->SC
SC[ServiceControl]-- HTTP API ---SI[ServiceInsight]
```

Note: In versions of ServiceControl prior to version 4.13.0, saga state change (SagaAudit) information can only be processed via the `ServiceControl Queue` (the input queue of the main ServiceControl instance). Starting with version 4.13.0, the SagaAudit data can also be processed by the ServiceControl audit instance via the `audit` queue. The latter approach is recommended.

## The Messages window

The Messages window is a detailed grid view indicating message status, type, time stamp, and service level information. Filter the list based on specific message content, searching for all message data, not just commonly displayed fields.

![Message List View](images/overview-messagedetailwindow.png 'width=500')

ServicePulse also supports opening ServiceInsight to a specific message to allow drill down for more detail.

NOTE: A messages body is searchable only if the body size is under 85kB, under the [ServiceControl/MaxBodySizeToStore](/servicecontrol/creating-config-file.md#performance-tuning-servicecontrolmaxbodysizetostore) size limit, and is a non-binary content type.

### Refresh and Auto-refresh

The Refresh and Auto-refresh toolbar buttons enable allow updating the displayed information with the latest updates from the ServiceControl database.

Auto-refresh keeps the information in ServiceInsight automatically refreshed, delivering near real-time information to the views.

The auto-refresh rate can be specified in the `View -> Options -> Auto-Refresh Timer` setting (default is auto-refresh every 15 seconds), or specify the auto-refresh rate using the ServiceInsight invocation parameter (see [ServiceInsight Invocation](application-invocation.md))


### Timing and performance

Use the performance-related information in the message header to sort the messages in an endpoint based on the time the messages were sent, critical time, processing time, delivery time, message type and ID.

The message timing measurements include:

 * **Processing Time**: the amount of time taken to process the message within the processing endpoint by the message processing handler method

Using the messages window column headers the messages can be sorted in ascending or descending order (the sorting operation applies on all the relevant messages in the underlying ServiceControl instance, and is not limited to the currently displayed messages).

Select a specific message; the related properties window and flow diagram change to illustrate details of the selected message.


## Endpoint Explorer

The Endpoint Explorer indicates the connection to the ServiceControl instance providing data to ServiceInsight. The list enumerates the endpoints and the machine where they are deployed.

![Endpoint Explorer](images/overview-endpointexplore-machinename.png)

Select endpoints to filter the message list. Select the root ServiceControl connection and the tree view to make the list expand to include all messages.

### Multiple ServiceControl connections

Starting with version 2.4.0, ServiceInsight can be connected to more than one ServiceControl instance at a time. Each ServiceControl connected instance will be displayed in the Endpoint Explorer and endpoints belonging to each instance will be grouped under the instance node. Selecting an instance allows to disconnect ServiceInsight from that ServiceControl instance.

## Flow diagram

The flow diagram provides extensive message and system information. When messages are selected in the message list, the flow diagram illustrates the message and all related messages from the same conversation, along with the nature of the messages and the endpoints involved.

![The Flow Diagram](images/overview-flowdiagram-wpopup.png 'width=500')

Each message is represented by a box indicating the message type and illustrating details including the endpoints and time information. Published events and sent commands have different icons and illustrations. Dropdowns provide more message details and links to search based on this message, copy message details, and retry messages.


## Message properties

Each message in NServiceBus contains extensive detail. As messages are selected in the flow diagram or in the list, an additional Message Properties window lists the properties for the message.

![The Message Properties Window](images/overview-messageproperties.png 'width=500')


## The Saga view

Sagas play a critical role in NServiceBus systems. As coordinators of processes they are started by certain messages and interact with a variety of messages and services. To open a graphical view of Sagas, click a message in the flow diagram that is handled by a saga.

![The Saga View](images/overview-sagaview.png 'width=500')

The saga illustrates how the saga was initiated and other messages that were sent or handled, with detailed message data, time information, and details on saga data changes.


## Sequence diagram

While a flow diagram is useful for showing *why* each message in a conversation was sent, a sequence diagram is better for understanding *when* messages were sent and handled.

![The Sequence Diagram](images/overview-sequence-diagram.png 'width=500')

Read more about the [Sequence Diagram](/serviceinsight/sequence-diagram/)


## Body view

ServiceInsight can show the body of a message in either `XML` or `JSON` format, and in raw `HEX`.

![Body Tab](images/overview-bodyview.png 'width=500')

## Custom message viewers

ServiceInsight has an extensibility point that allows creating custom message viewers. These are suitable when a custom serializer is used or when the message is partially or fully encrypted and access to messages in in clear text is required in ServiceInsight.

Read more about the [custom message viewers](/serviceinsight/custom-message-viewers.md) and the plugin model.

## Log view

ServiceInsight leverages the ServiceControl API to retrieve information. The Log tab of the Flow Diagram window displays details of the interactions as ServiceInsight polls ServiceControl for more data.

![Log View Tab](images/overview-logview.png 'width=500')
