---
title: How to See the Queues and Messages on a Machine?
summary: See queues on local machine using Server Explorer in Visual Studio, The most interesting property is BodyStream.
originalUrl: http://www.particular.net/articles/how-can-i-see-the-queues-and-messages-on-a-machine
tags: []
createdDate: 2013-05-22T05:10:48Z
modifiedDate: 2013-07-29T14:14:20Z
authors: []
reviewers: []
contributors: []
---

You can see all the queues on the local machine using Server Explorer in Visual Studio:

<center>
![Server Explorer](ServerExplorer.jpg "Server Explorer")

</center> If there is a message in one of the queues, select it and view the properties of the message in the property panel in Visual Studio
(usually on the bottom right):

<center>
![Visual Studio properties](VisualStudioProperties.jpg "Visual Studio properties")

</center> The most interesting property is the BodyStream as it allows you to see the contents of the message:

<center>
![Message contents](BodyStream.jpg "Message contents")

</center>


