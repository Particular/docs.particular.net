<!--
title: "How to See the Queues and Messages on a Machine?"
tags: ""
summary: "You can see all the queues on the local machine using Server Explorer in Visual Studio:"
-->

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


