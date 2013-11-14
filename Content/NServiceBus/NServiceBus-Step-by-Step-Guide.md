<!--
title: "NServiceBus Step by Step Guide - code first"
tags: ""
summary: "<p>In this tutorial we are going to create a very simple ordering system that will send messages from a client to a server. The ordering system includes three projects: Client, Server, and Messages, to complete this tasks we will execute the following steps:</p>
<ol>
<li><a href="#Creating%20the%20Client%20project">Creating the Client project</a></li>
<li><a href="#Message">Creating the Messages project</a></li>
<li><a href="#Server">Creating the Server project</a></li>
<li><a href="#Sending">Sending the order</a></li>
<li><a href="#Running">Running the solution</a></li>
<li><a href="#Next%20Steps">Next Steps</a></li>
</ol>
"
-->

In this tutorial we are going to create a very simple ordering system that will send messages from a client to a server. The ordering system includes three projects: Client, Server, and Messages, to complete this tasks we will execute the following steps:

1.  [Creating the Client project](#Creating%20the%20Client%20project)
2.  [Creating the Messages project](#Message)
3.  [Creating the Server project](#Server)
4.  [Sending the order](#Sending)
5.  [Running the solution](#Running)
6.  [Next Steps](#Next%20Steps)

The complete solution code can be found
[here](https://github.com/sfarmar/Samples/tree/master/Ordering)

### <a id="Client" name="Client"> </a> Creating the Client project

Let's start by creating a 'Client' project that will send order requests to a NServiceBus endpoint.

Open Visual Studio as administrator, create a new Project name it
'Ordering.Client', and name the solution 'Ordering'.


[![](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding/001%20new%20solution.png)](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding/001%20new%20solution.png)

We now need to add references the NServiceBus assemblies and the quickest and quickest way to do that is to use NuGet Package Manager Console.

Open the NuGet Package Manager Console: Tools -\> Library Package Manager -\> Package Manager Console.

[![](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding/Package%20manager%20console.png)](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding/Package%20manager%20console.png)

Type the following command at the Package Manager Console:

<div class="nuget-badge">
`PM> Install-Package NServiceBus.Host`{style="background-color: rgb(32, 32, 32); border: 4px solid rgb(192, 192, 192); border-top-left-radius: 5px; border-top-right-radius: 5px; border-bottom-right-radius: 5px; border-bottom-left-radius: 5px; box-shadow: rgb(110, 110, 110) 2px 2px 3px; color: rgb(226, 226, 226); display: block; font-size: 1.2em; font-family: 'andale mono', 'lucida console', monospace; line-height: 1.2em; overflow: auto; padding: 1px;"}


NOTE: When prompted to reload the project, click reload

The package installation process adds references to NServiceBus assemblies and creates several boiler template files in the Client project.

For example, 'EndpointConfig.cs' is used to configure the project endpoints, and by default the configuration is set to Server.

To change the configuration to 'Client', open the 'EndpointConfig.cs' file that was just created for you and replace this line:

<script src="https://gist.github.com/Particular/6083117.js?file=AsA_Server.cs"></script> with

<script src="https://gist.github.com/Particular/6083117.js?file=AsA_Client.cs"></script> You will add more code to the 'Client' project later on but now we are going to concentrate on the area that will handle our order requests.

### <a id="Message" name="Message"> </a> Creating the Messages Project

First lets add a new Class Library project and call it
'Ordering.Messages'.


[![](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding/Creat%20Messages.png)](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding/Creat%20Messages.png)

The Messages project is the container of message definitions. This project will be shared between the client and server so both sides agree on the typed message descriptions.

Install the 'NServiceBusInterfaces' NuGet package for this new project:

At the Package Manager Console, type:

<div class="nuget-badge">
`PM> Install-Package NServiceBus.Interfaces -ProjectName Ordering.Messages`{style="background-color: rgb(32, 32, 32); border: 4px solid rgb(192, 192, 192); border-top-left-radius: 5px; border-top-right-radius: 5px; border-bottom-right-radius: 5px; border-bottom-left-radius: 5px; box-shadow: rgb(110, 110, 110) 2px 2px 3px; color: rgb(226, 226, 226); display: block; font-size: 1.2em; font-family: 'andale mono', 'lucida console', monospace; line-height: 1.2em; overflow: auto; padding: 1px;"}


Add a command with a property to hold a product name:

Delete 'Class1.cs' and add a class and name it 'PlaceOrder.cs' (or if you want you can rename the file to 'PlaceOrder.cs').

Implement the PlaceOrder command in 'PlaceOrder.cs'.

Replace the content of 'PlaceOrder.cs' with the following code:

<script src="https://gist.github.com/Particular/6083117.js?file=PlaceOrderCommand.cs"></script>
### <a id="Server" name="Server"> </a> Creating the Server Project

You are now ready to create the orders processing server, add a new project and name is 'Ordering.Server'.


[![](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding/Creat%20Server.png)](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding/Creat%20Server.png)

Install the 'NServiceBusHost' NuGet package for this new project:

At the Package Manager Console, type:

<div class="nuget-badge">
`PM> Install-Package NServiceBus.Host -ProjectName Ordering.Server`{style="background-color: rgb(32, 32, 32); border: 4px solid rgb(192, 192, 192); border-top-left-radius: 5px; border-top-right-radius: 5px; border-bottom-right-radius: 5px; border-bottom-left-radius: 5px; box-shadow: rgb(110, 110, 110) 2px 2px 3px; color: rgb(226, 226, 226); display: block; font-size: 1.2em; font-family: 'andale mono', 'lucida console', monospace; line-height: 1.2em; overflow: auto; padding: 1px;"}


NOTE: When prompted to reload the project, click reload

For the server side to understand and interpret the message content, add a reference to the 'Messages' project you created earlier:

Right click References in the 'Ordering.Server' Project -\> Add Reference -\> Ordering.Messages.

Replace the content of 'PlaceOrderHandler.cs' with the following code:

<script src="https://gist.github.com/Particular/6083117.js?file=CommandHandler.cs"></script>
### <a id="Sending" name="Sending"> </a> Sending the order

We nearly done, all it is left to do is to go back to the 'Client' project add a reference to the 'Ordering.Messages' project and copy and paste the following code into the 'Class1.cs' (if you want you can rename the file to 'SendOrder.cs') file:

<script src="https://gist.github.com/Particular-gist/6964764.js?file=ClientBootstrap.cs"></script> NOTE: The code above is version 4.x, the 3.x interface
'IWantToRunAtStartup' has been replaced with
'IWantToRunWhenBusStartsAndStops'

### <a id="Running" name="Running"> </a> Running the solution

You’ve completed coding the example and now it’s time to run the solution. 

To see the complete system, run both the Client and the Server projects together:

To run the 'Client' and 'Server' projects together so you can see it all working, right click on the 'Ordering' solution and select 'Set StartUp Projects...'


[![](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding/002%20strartup.png)](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding/002%20strartup.png)

in that screen select 'Multiple startup projects' and set the
'Ordering.Client' and 'Ordering.Server' action to be 'Start'.


[![](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding/003%20strartup.png)](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding/003%20strartup.png)

Finally click 'F5' to run the solution.

Two console application windows should start up




[![](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding/run_1.png)](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding/run_1.png)

Hit enter (while the Client console is in focus) and you should see
'Order for Product: New shoes placed' in one of them.

[![](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding/run_2.png)](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding/run_2.png)

Congratulations - you've just built your first NServiceBus application.
-----------------------------------------------------------------------

Wasn't that easy?
-----------------

\* If you see some warnings on the consoles, these warnings are just NServiceBus telling you that it couldn’t find the queues it needs, so it went ahead and created them for you.

### <a id="Next Steps" name="Next Steps"> </a> Next Steps

-   Go to [NServiceBus Step by Step Guide - Fault Tolerance - code
    first](NServiceBus-Step-by-Step-Guide-fault-tolerance-code-first.md)
-   Read about [NServiceBus and SOA Architectural
    Principles](architectural-principles.md)
-   Try our [Hands on Labs](http://particular.net/HandsOnLabs)
-   Check out our [Videos and
    Presentations](http://particular.net/Videos-and-Presentations)
-   See the
    [Documentation](http://particular.net/documentation/NServiceBus)
-   Join our [community](http://particular.net/DiscussionGroup)



