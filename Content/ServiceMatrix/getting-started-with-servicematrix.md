---
title: Getting Started with NServiceBus using ServiceMatrix 2.0 - send and receive
summary: 'Getting Started with ServiceMatrix 2.0 and a send and receive example.'
tags:
- ServiceMatrix
- Send and Receive
- Visual Studio
authors: [Sean Farmer, Joe Ratzloff]
reviewers: []
contributors: []
---

ServiceMatrix is a Visual Studio integrated development environment for developing NServiceBus distributed System.

This step-by-step guide will walk you through the creation of a send-and-receive NServiceBus distributed application using ServiceMatrix v2.0 for Visual Studio 2012, using the following steps:

1.  [Installing ServiceMatrix](#Installing%20ServiceMatrix)
2.  [Creating a new project](#Creating%20a%20new%20project)
3.  [Creating Endpoints](#Creating%20Endpoints)
4.  [Creating Services](#Creating%20Services)
5.  [Deploying Components](#Deploying%20Components)
6.  [Sending a Message](#Sending%20a%20Message)
7.  [Running the Application](#Running%20the%20Application)

The complete solution code can be found
[here.](https://github.com/sfarmar/Samples/tree/master/ServiceMatrixIntro)

The example demonstrates the integration of an online sales web store with a backend system using the request - response pattern and NServiceBus.

<a id="Installing ServiceMatrix" name="Installing ServiceMatrix"> </a> Installing ServiceMatrix v2.0 for Visual Studio 2012 
---
System requirements:

-   Visual Studio 2010 or Visual Studio 2012

-   ASP.NET MVC 4 ([http://www.asp.net/downloads](http://www.asp.net/downloads))

To install ServiceMatrix:

1.  Download the latest version from [http://particular.net/downloads](http://particular.net/downloads)

2.  Run the installer.

**NOTE** : If you have both Visual Studio 2010 and Visual Studio 2012 installed on your machine, you can install ServiceMatrix for one Visual Studio version. This document guides the use of ServiceMatrix v2.0 for Visual Studio 2012.

<a id="Creating a new project" name="Creating a new project"> </a> Creating a new project
---

To get started with ServiceMatrix, create a new project:

1. In Visual Studio select 'File\> New\> Project' and Select 'NServiceBus System' under the Visual C\# project type. Target the .NET Framework 4.5 for this project. ![NewProject](http://raw2.github.com/Particular/docs.particular.net/Drafts/Content/NServiceBus/servicematrix/images/servicematrix-reqresp-newproject.png)
2. In the Solution name, type 'OnlineSales' (or any other name) for the name of your application.  
You'll see that a solution folder has been created for your solution, as shown. ![New Solution](http://raw2.github.com/Particular/docs.particular.net/Drafts/Content/NServiceBus/servicematrix/images/servicematrix-reqresp-freshsolution.png)

A number of projects have been created for you, as shown in the Solution Explorer pane.
The '.nuget' (we use Nuget for our dependencies and the '.nuget' folder is there to support enabling Package Restore and 'Solution Items' folders are part of the ServiceMatrix infrastructure

Two important folders are the 'Contract' and 'InternalMessages' projects as they are where all message types are placed:

-   All defined events will be put in the 'Contract' project.
-   All commands will be put in the 'InternalMessages' project. 

Later you will see how messages from different services are partitioned in these projects. 

Take a look at the design environment. The <a href="http://raw2.github.com/Particular/docs.particular.net/Drafts/Content/NServiceBus/servicematrix/images/servicematrix-solutionbuilder.png" target = "_blank">Solution Builder</a> on the left provides a hierarchy of the logical elements of the solution. If you  don't see a docked window in Visual Studio called Solution Builder,  open it via the View menu.

You should see folders in Solution Builder called 'Infrastructure', 'Libraries', 'Endpoints', and 'Services'.

-   Infrastructure is where cross-cutting concerns like authentication and auditing are handled.

-   Libraries are units of code that can be reused, including logging and data access.

-   Endpoints are where code is deployed as executable processes. They can be web applications (both Web Forms and MVC) or NServiceBus
Hosts (a special kind of Windows Service that allows you to debug it
as a Console Application).

-   Services are logical containers for code that provide the structure for publish/subscribe events and command-processing.Services are made of Components which will be shown later.

The <a href="http://raw2.github.com/Particular/docs.particular.net/Drafts/Content/NServiceBus/servicematrix/images/servicematrix-canvas.png" target= "_blank">NServiceBus Canvas </a> is in the center of the solution as shown above.   The endpoints, services, components and messages that comprise our solution will be created and illustrated here.

This dashed areas within the canvas and the buttons at the top are used to start building our solution.   **NOTE**: Alternatively, they can also be created using the Solution Builder tree view.  However since this is a visual tool, we will demonstrate using the canvas.  As items are added to the canvas they will appear in the Solution Builder as well as in the Solution Explorer project.

Building the Online Sales Solution
---
Our online sales example involves a website that collects online orders and a backend order processing system that processes them.  

To build the solution we will define and endpoint for the website and another endpoint for the order processing system.  A new 'Sales' service will define components for submitting and processing orders as well as a command message to represent the order submission.  

<a id="Creating the Endpoints" name="Creating Endpoints"></a> Creating Endpoints
----
First we will create the endpoints for selling and processing. 

1. To create and endpoint on the canvas either select the dashed 'New Endpoint' area on the canvas or the button at the top of the canvas.<br>
![New Endpoint Popup](http://github.com/Particular/docs.particular.net/raw/Drafts/Content/NServiceBus/servicematrix/images/servicematrix-newendpoint.png)
2. Name the endpoint 'ECommerce' and choose ASP.NET MVC as the endpoint host.  **NOTE: ** MVC Endpoints require that ASP.NET MVC be installed on the local machine. If you haven't installed ASP MVC on your machine, choose a Web Forms host for the endpoint instead.  Both work equally well. 
3. We will examine the generated code in detail later to understand    how things work behind the scenes.  For now, notice how Service Matrix has created the ECommerce Endpoint on the canvas, in the Solution Builder and in the Visual Studio Project.  In the solution builder notice that this endpoint has a folder to contain components.  Components contain the code for specific services.  They can only send commands to other components in the same service.  However, they can subscribe to events that are published by components in *any* service. Soon our Sales components will be deployed to our endpoints.
4. Create another endpoint called 'OrderProcessing'.  This time select 'NServiceBus Host' as the host.  

At this point your Solution should have both endpoint on the NServiceBus canvas [as shown](http://github.com/Particular/docs.particular.net/raw/Drafts/Content/NServiceBus/servicematrix/images/servicematrix-canvaswithendpoints.png "Endpoints On NServiceBus Canvas"). Notice how you can control the zoom with your mouse scroll wheel and drag the boxes around.   You will have to rearrange the canvas as more things are added to it.  

<a id="Creating Services" name="Creating Services"></a> Creating Services 
----
Next we will create a 'Sales' Service that will facilitate the communication between our website and order processing.

1. At the top of the canvas select the 'New Service' button and name your new service 'Sales' as shown. <BR> ![New Sales Service](http://github.com/Particular/docs.particular.net/raw/Drafts/Content/NServiceBus/servicematrix/images/servicematrix-newsalesservice.png)
2. The canvas will illustrate the new Sales service.  It's shown in a 'Undeployed Components' box.  This is because we have yet to define and deploy any of them for this service.  Similarly, no code has yet been generated in the Visual Studio project.  As we add our command this will change!
3. Notice the drop down next to the title in the undeployed 'Sales' service.  Click on [the dropdown](http://github.com/Particular/docs.particular.net/raw/Drafts/Content/NServiceBus/servicematrix/images/servicematrix-sales-newcommand.png "Sales Send Command Menu") and select 'Send Command'.  Name your new command 'SubmitOrder'.  ![Submit Order Command](http://github.com/Particular/docs.particular.net/raw/Drafts/Content/NServiceBus/servicematrix/images/servicematrix-salessendcommand.png)   
4. Notice that several things have happened. The Sales services has had two new components added to it as shown below. ![Undeployed Sales Service](http://github.com/Particular/docs.particular.net/raw/Drafts/Content/NServiceBus/servicematrix/images/servicematrix-sales-undeployed.png)<br> The service now has a 'Sender' component named after our command that will enable an endpoint to send 'SubmitOrder' messages.  Similarly there is a 'Processor' component that will enable an endpoint to process these messages. <br><br> In addition to illustrating these in the canvas the [Solution Builder](http://github.com/Particular/docs.particular.net/raw/Drafts/Content/NServiceBus/servicematrix/images/servicematrix-solutionbuilder-salesservice.png "Solution Builder With Sales") now shows the 'SubmitOrder' command in the commands folder.  It also illustrates the components and the fact they send and process the 'SubmitOrder' command accordingly. <BR> You will notice there is now code that has been generated in the Visual Studio project as well. 
5. The 'SubmitOrder' command is meant to communicate the order between our endpoints.   To view the generated class file, click on the drop down menu of the SubmitOrder command and select 'View Code' [as shown](http://github.com/Particular/docs.particular.net/raw/Drafts/Content/NServiceBus/servicematrix/images/servicematrix-submitorderviewcode.png "View SubmitOrder Code").<br> You can add all sorts of properties to your message to represent the order data: strings,  integers, arrays, dictionaries, etc. Just make sure to provide both a get accessor and a set accessor to each property. 
 
```C#
using System;

namespace OnlineSales.InternalMessages.Sales
{
    public class SubmitOrder
    {
    }
}

```
The code for the Sales components is not created until they are deployed to an endpoint. So, next we will go ahead and deploy them.

<a id="Deploying Components" name="Deploying Components"></a> Deploying Components
----
Remember that in our example we want to send orders from the front end ECommerce website to the back end for processing via our bus.  To make this happen we need to deploy the 'Sender' component to the ECommerce endpoint and the 'Processor' to OrderProcessing endpoint.  

By deploying these components to each endpoint, the 'Sales' service will have afforded our systems the capability to easily communicate reliably and durably using a command message containing the submitted order.  

Let's deploy!

1. To deploy our sender use the drop down menu of the 'SubmitOrderSender' component as shown below.   Select 'Deploy Component' and choose to deploy it to the 'ECommerce' endpoint using the list provided. ![Deploy the Sales Components](http://github.com/Particular/docs.particular.net/raw/Drafts/Content/NServiceBus/servicematrix/images/servicematrix-salesdeploycomponent.png)<br>**NOTE:** You cannot build the solution with components that aren't deployed.  If you try to build at this point you will get an error indicating that the 'Sales.SubmitOrderProcessor' must be allocated to an endpoint. 
2. Deploy the 'SubmitOrderProcessor' to the 'OrderProcessing' endpoint by choosing the drop down menu.  At this point, with a little re-organizing, the canvas should nicely illustrate the ECommerce and OrderProcessing endpoints using the Sales service components to send and process the SubmitOrder command. <br> ![Canvas with Service Deployed to Endpoints](http://github.com/Particular/docs.particular.net/raw/Drafts/Content/NServiceBus/servicematrix/images/servicematrix-canvaswiredup.png)
3. Now build the solution and see how everything turns out.
4. Look at the 'SubmitOrderProcessor' code by selecting its drop down and choosing 'Open Code'.  As you can see below, there isn't much here.  A partial class has been created where you would need to add your order processing logic.  You can view the partial class counterpart by clicking F12 while highlighting the class name.  There isn't much to see there either; just a class that implements 'IHandleMessages<submitorder>' and has a reference to 'IBus' that you can use from within your partial class to send out other messages, publish events, or to reply to commands.
    
```C#
using System;
using NServiceBus;
using OnlineSales.InternalMessages.Sales;


namespace OnlineSales.Sales
{
    public partial class SubmitOrderProcessor
    {
		
        // TODO: SubmitOrderProcessor: Configure published events' properties implementing the partial Configure[EventName] method.

        partial void HandleImplementation(SubmitOrder message)
        {
            // TODO: SubmitOrderProcessor: Add code to handle the SubmitOrder message.
            Console.WriteLine("Sales received " + message.GetType().Name);
        }

    }
}

```

<a id="Sending a Message" name="Sending a Message"></a> Sending a Message 
---
The last thing to do is to review how the 'ECommerce' website sends a message.  When ServiceMatrix generated the MVC endpoint it also created a demonstration site already capable of sending the commands created using the tool. 

If you chose web forms to host the ECommerce endpoint instead of MVC, see the section for [Web Forms Users](#webformsreview "Web Forms Review") below.

Find the **TestMessagesController.generated.cs** file in the Controllers folder in the OnlineSales.ECommerce project.  This file is generated by ServiceMatrix. Notice the MvcApplication.Bus.Send method that sends the command message SubmitOrder.

```C#
namespace OnlineSales.ECommerce.Controllers
{
    public partial class TestMessagesController : Controller
    {
        //
        // GET: /TestMessages/

        public ActionResult Index()
        {
            return View();
        }

		
        //
        // POST: /TestMessages/SendMessageSubmitOrder
          
		    [HttpPost]
        public ActionResult SendMessageSubmitOrder(SubmitOrder SubmitOrder)
        {
            ConfigureSubmitOrder(SubmitOrder);
            MvcApplication.Bus.Send(SubmitOrder);

            ViewBag.MessageSent = "SubmitOrder";

            return View("Index");
        }

		
        partial void ConfigureSubmitOrder(SubmitOrder message);
      
    }
} 
```
    
Before the Bus.Send the code invokes a partial method called **ConfigureSubmitOrder** that accepts our SubmitOrder message as a parameter.  This can be implemented by you inside the **TestMessagesController.cs** file in the same directory.  The following code snippet illustrates how that can be done.  

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSales.InternalMessages.Sales;

namespace OnlineSales.ECommerce.Controllers
{
    public partial class TestMessagesController
    {
        // TODO: OnlineSales.ECommerce: Configure sent/published messages' properties implementing the partial Configure[MessageName] method.");
        partial void ConfigureSubmitOrder(SubmitOrder message)
        {
            //This is where we can get access the message and mutate it etc.  as we see fit before it is sent.
            //You need to add a Name property to the SubmitOrder class for this to work. 
			message.Name = "I was mutated in the MVC Application!";

        }

    }
}
```

###<a id="webformsreview" name="webformsreview">Web Forms Users</a>

Make sure the page is in "Design" view. Open Default.aspx and drag a button object from the toolbox onto the page. Double click the button you just added, which opens the code-behind button-click handling method.

In the method, type:


```C#
Global.Bus.Send(new SubmitOrder());

```


<a id="Running the Application" name="Running the Application"></a> Running the Application
-------------------------------------------------------------------------------------------

Now press F5 to debug the application. You should see something similar to the image shown: a new tab in your browser and a console application. If you click "About" in the UI a couple of times, you should see the console application get a message each time.

If you're in a regular ASP.NET web project, you'll see a different image, so just click the button on the form. 


Congratulations! You've just built your first NServiceBus application. Wasn't that easy?

Next steps
----------

The production-time benefits of NServiceBus (let's face it, interprocess communication isn't that exciting and has been done many times before): see how NServiceBus handles [Fault Tolerance](getting-started-with-nservicebus-using-servicematrix-2.0---fault-tolerance.md)
.

