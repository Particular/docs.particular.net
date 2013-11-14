<!--
title: "How to See the Queues and Messages on a Machine?"
tags: 
-->

You can see all the queues on the local machine using Server Explorer in Visual Studio:

<center>
![Server Explorer](https://particular.blob.core.windows.net/media/Default/images/ServerExplorer.jpg "Server Explorer")

</center> If there is a message in one of the queues, select it and view the properties of the message in the property panel in Visual Studio
(usually on the bottom right):

<center>
![Visual Studio properties](https://particular.blob.core.windows.net/media/Default/images/VisualStudioProperties.jpg "Visual Studio properties")

</center> The most interesting property is the BodyStream as it allows you to see the contents of the message:

<center>
![Message contents](https://particular.blob.core.windows.net/media/Default/images/BodyStream.jpg "Message contents")

</center>


