---
title: Send and Receive with ServiceMatrix V2.X
summary: 'Getting Started with the ServiceMatrix V2.X for Visual Studio 2012 and 2013 using a send and receive example.'
tags:
- ServiceMatrix
- Send and Receive
- Visual Studio
related:
- servicematrix/getting-started-with-nservicebus-using-servicematrix-2.0-fault-tolerance
include: sm-discontinued
---

This step-by-step guide to the Particular Service Platform walks you through the creation of a send-and-receive NServiceBus distributed application using ServiceMatrix V2.X for Visual Studio 2012 & 2013.

The example demonstrates the integration of an online sales web store with a backend system using the request-response pattern and NServiceBus.


## Installing ServiceMatrix for Visual Studio

ServiceMatrix is a Visual Studio integrated development environment for developing the NServiceBus distributed system.

This document reviews the use of ServiceMatrix for Visual Studio, assuming ServiceMatrix has already been installed.

ServiceMatrix can be installed using the Particular Platform Installer. To download and install ServiceMatrix separately, [follow the instructions](installing-servicematrix-2.0.md "Installing ServiceMatrix").

For this example you need to meet the following system requirements:
- Visual Studio 2012 or 2013 (Professional or above - not Express)
- ASP.NET MVC 4 ([http://www.asp.net/downloads](http://www.asp.net/downloads))

NOTE: If you have both Visual Studio 2010 and Visual Studio 2012/13 installed on your machine, you can install ServiceMatrix for one Visual Studio version only. This document reviews the use of ServiceMatrix V2.0 for Visual Studio 2012 or 2013.


## Creating a New Project

To get started with ServiceMatrix, create a new project.


### Create an NServiceBus project

In Visual Studio, select `File\New\Project` and select 'NServiceBus Version 5 System' under the Visual C\# project type. Target the .NET Framework 4.5 for this project.

![NewProject](images/servicematrix-reqresp-newprojectv2.2.0.png)

In the Solution name, type 'OnlineSales' (or any other name) for the name of your application.

NOTE: It is recommended that you use the 'NServiceBus Version 5 System' project template unless you need to remain on NServiceBus Version 4 for some particular reason. NServiceBus Version 5 is wire compatible with earlier versions, so you should be able to use the latest version without breaking compatibility with services that may already be deployed and running on an earlier version. [Read this document](/nservicebus/upgrades/4to5.md) for an overview of the differences in Version 5 from Version 4.


### Review The Solution

You'll see that a solution folder has been created for your solution, as shown.

![New Solution](images/servicematrix-reqresp-freshsolutionv2.2.0.png)

A number of projects have been created for you, as shown in the Solution Explorer pane. The `Solution Items` folder is part of the ServiceMatrix infrastructure.

Two important folders are the `Contracts` and `Internal` projects as they are where all message types are placed:
-   All defined events will be put in the `Contracts` project.
-   All commands will be put in the `Internal` project.

Later you will see how messages from different services are partitioned in these projects.

Look at the design environment. The [Solution Builder](images/servicematrix-solutionbuilderv2.2.0.png "Solution Builder") on the left provides a hierarchy of the logical elements of the solution. If you  don't see a docked window in Visual Studio called Solution Builder,  open it via the View menu.

You should see folders in Solution Builder called 'Infrastructure',  'Endpoints', and 'Services'.
-   Infrastructure is where cross-cutting concerns like authentication and auditing are handled.
-   Endpoints are where code is deployed as executable processes. They can be MVC web applications or [NServiceBus Hosts](/nservicebus/hosting/nservicebus-host/).
-   Services are logical containers for code that provide the structure for publish/subscribe events and command-processing.

The [NServiceBus Canvas](images/servicematrix-canvas.png "NServiceBus Canvas") is in the center of the solution as shown above. The endpoints, services, components, and messages that comprise your solution will be created and illustrated here.

To start building your solution, use the dashed areas within the canvas and the buttons at the top.

Alternatively, you can create them using the Solution Builder tree view. However, since this is a visual tool, this example demonstrates on the canvas. As you add items to the canvas they will appear in the Solution Builder as well as in the Solution Explorer project.


## Building the Online Sales Solution

This online sales example involves a website that collects online orders, and a backend order processing system that processes them. 

To build the solution you will define an endpoint for the website and another endpoint for the order processing system. A new 'Sales' service will define components for submitting and processing orders as well as a command message to represent the order submission. 


## Creating Endpoints

First you will create the endpoints for selling and processing.


### New Endpoint

To create an endpoint on the canvas either select the dashed 'New Endpoint' area on the canvas or the button at the top of the canvas.

![New Endpoint Popup](images/servicematrix-newendpoint.png)

Name the endpoint `ECommerce` and choose ASP.NET MVC as the endpoint host. 

NOTE: MVC Endpoints require that ASP.NET MVC be installed on the local machine. You can [install ASP.NET MVC from here](http://www.asp.net/downloads) or use the Web Platform Installer.


### Review the Endpoint

You will examine the generated code in detail later to understand how things work behind the scenes. For now, notice how ServiceMatrix has created the ECommerce Endpoint on the canvas, in the Solution Builder and in the Visual Studio Project.

In the Solution Builder, notice that this endpoint has a folder to contain components. Components contain the code for specific services. They can only send commands to other components in the same service. However, they can subscribe to events that are published by components in *any* service. Soon your sales components will be deployed to your endpoints.


### Create OrderProcessing Endpoint

Create another endpoint called `OrderProcessing`. This time select 'NServiceBus Host' as the host. 

At this point your solution should have both endpoints on the NServiceBus canvas.

![Canvas With Endpoints](images/servicematrix-canvaswithendpoints.png)

Notice how you can control the zoom with your mouse scroll wheel, and drag the boxes around. This is how you rearrange the canvas when you add more things to it. Also notice that the Solution Builder has been updated with your new endpoints.

![Solution Builder With Endpoints](images/servicematrix-solnbuilderwithendpointsv2.2.0.png)


## Creating a Message

To facilitate communication between the website and the backend `OrderProcessing` endpoint, use a command message. Create this message using the drop-down menu of the `ECommerce` endpoint, and select `Send Command` as shown. 

![Send Command](images/servicematrix-ecommercesendcommand.png)


## Creating Services

As you create the new command message, you are prompted for the name of a service. In NServiceBus a service contains components responsible for facilitating the communication between the website and order processing. Name the new service `Sales` and the command `SubmitOrder` as shown. The 'Handled with' is a drop down list of all the other message handler components that belong to the selected service. By default, this is '[new handler]' and choosing this option will create a new message handler component for handling the `SubmitOrder`command.

![Sales Service and SubmitOrder Command](images/servicematrix-sales-submitorderv2.2.0.png)
 
The canvas now illustrates the new Sales service with two components. The `SubmitOrderSender` component sends the command and is deployed to the `ECommerce` endpoint. The `SubmitOrderHandler` component receives the command message and is shown in an 'Undeployed Components' box. 

![Undeployed Sales Service](images/servicematrix-sales-undeployed.png)

## Deploying Components

You cannot build the solution with components that are not deployed. If you try to build at this point you will get an error indicating that the `Sales.SubmitOrderHandler` must be allocated to an endpoint. Deploy the `SubmitOrderHandler` component using its drop-down menu and the `Deploy Component` option. When prompted, deploy the component to the `OrderProcessing` endpoint.

![Deploy Component](images/servicematrix-deploysubmitorder.png)

At this point, with a little reorganizing, the canvas should illustrate the `ECommerce` and `OrderProcessing` endpoints using the `Sales` service components to send and process the `SubmitOrder` command.

![Canvas with Service Deployed to Endpoints](images/servicematrix-canvaswiredup.png)

By deploying these components to each endpoint, the `Sales` service affords your systems the capability to easily communicate reliably and durably, using a command message containing the data for the submitted order. 

In addition to illustrating them in the canvas, the [Solution Builder](images/servicematrix-solutionbuilder-salesservice.png "Solution Builder With Sales") now shows the `SubmitOrder` command in the commands folder. It also illustrates the components and the fact they send and process the `SubmitOrder` command accordingly. You can also see code that has been generated in the Visual Studio project.


## Review the Message

The `SubmitOrder` command is a simple message meant to communicate the order between your endpoints. To view the generated class file, click the drop-down menu of the `SubmitOrder` command and select View Code [as shown](images/servicematrix-submitorderviewcode.png "View SubmitOrder Code"). This is a very simple C# class. You can add all sorts of properties to your message to represent the order data: strings, integers, arrays, dictionaries, etc. Just make sure to provide both a get accessor and a set mutator for each property.

```C#
namespace OnlineSales.Internal.Commands.Sales
{
    public class SubmitOrder
    {
		//Put your properties in the class.
		//public string CustomerName { get; set; }
    }
}
```


## Handling a Message

Now build the solution and see how everything turns out. Look at the `SubmitOrderHandler` code by selecting its drop-down menu and choosing 'View Code'. As you can see below, there is not much there. A partial class has been created where you can add your order processing logic.

```C#
namespace OnlineSales.Sales
{
    public partial class SubmitOrderHandler
    {
        partial void HandleImplementation(SubmitOrder message)
        {
            // TODO: SubmitOrderHandler: Add code to handle the SubmitOrder message.
            Console.WriteLine("Sales received " + message.GetType().Name);
        }
    }
}
```

You can locate the ServiceMatrix-generated partial class counterpart in the `OnlineSales.OrderProcessing` project and the `Infrastructure\Sales` folder. There is not much to see; just a class that implements `IHandleMessages<submitorder>` and has a reference to `IBus` that you can use from within your partial class to send out other messages, publish events, or to reply to commands. The partial method `HandleImplementation(message)` is a call to the implementation above. To learn more about the way to use the generated code, see [Using ServiceMatrix Generated Code](customizing-extending.md). 
   
```C#
namespace OnlineSales.Sales
{
    public partial class SubmitOrderHandler : IHandleMessages<SubmitOrder>
    {
		public void Handle(SubmitOrder message)
		{
			// Handle message on partial class
			this.HandleImplementation(message);
		}

		partial void HandleImplementation(SubmitOrder message);
        public IBus Bus { get; set; }
    }
}

```


## Sending a Message

Lastly, review how the 'ECommerce' website sends a message. When ServiceMatrix generated the MVC endpoint, it created a demonstration site already capable of sending the commands created using the tool.


### Review MVC Code

Find the `TestMessagesController.generated.cs` file in the Controllers folder in the OnlineSales.ECommerce project. ServiceMatrix generates this file as part of the MVC application. Notice the `SubmitOrderSender.Send` method that sends the command message `SubmitOrder`. This method was generated in a different partial class file located in the `Infrastructure\Sales\SubmitOrderSender.cs` file.

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


    public ISubmitOrderSender SubmitOrderSender { get; set; }

    // POST: /TestMessages/SendMessageSubmitOrder

    [HttpPost]
    public string SendMessageSubmitOrder(SubmitOrder SubmitOrder)
    {
      ConfigureSubmitOrder(SubmitOrder);
      SubmitOrderSender.Send(SubmitOrder);
      return "<p> SubmitOrder command sent</p>";
    }


    // Send Commands

    partial void ConfigureSubmitOrder(SubmitOrder message);

    // Publish Events
  }
}
``` 

This is a demonstration site that provides an initial reference application in MVC. Any modifications to this file will be overwritten by subsequent regeneration of the demonstration site. To accomodate your changes, before the `SubmitOrderSender.Send` is called, the code invokes a partial method called `ConfigureSubmitOrder` that accepts your `SubmitOrder` message as a parameter. You can implement this in the `SubmitOrderSender.cs` file in the `\Sales` directory of the `OnlinesSales.ECommerce` project, as shown in the following code snippet: 

```C#
namespace OnlineSales.Sales
{
    public partial class SubmitOrderSender
    {
        //You can add the partial method to change the submit order message.
        partial void ConfigureSubmitOrder(SubmitOrder message)
        {
            message.CustomerName="John Doe";
        }
    }
}
```

## Selecting a Persistence Store

NServiceBus requires a persistence store. By default, ServiceMatrix provisions your solution to use the `InMemoryPersistence` class, but only if the Visual Studio debugger is attached. If you attempt to run your project without the debugger attached, you will receive an exception informing you to choose a durable persistence class.

### Selecting Persistence for ECommerce MVC Endpoint

Each endpoint should be configured. For your ECommerce MVC endpoint, you will find the setup in `Infrastructure\WebGlobalInitialization.cs`.

````C#
if (Debugger.IsAttached)
{
  // For production use, please select a durable persistence.
  // To use RavenDB, install-package NServiceBus.RavenDB and then use configuration.UsePersistence<RavenDBPersistence>();
  // To use SQLServer, install-package NServiceBus.NHibernate and then use configuration.UsePersistence<NHibernatePersistence>();	
  config.UsePersistence<InMemoryPersistence>();

  // In Production, make sure the necessary queues for this endpoint are installed before running the website
  // While calling this code will create the necessary queues required, this step should
  // ideally be done just one time as opposed to every execution of this endpoint.
  config.EnableInstallers();
}
````

Open the NuGet Package Manager Console: `Tools > NuGet Package Manager > Package Manager Console`.

Type the following command at the Package Manager Console:

    PM> Install-Package NServiceBus.RavenDB

NOTE: When prompted to reload the project, click reload

Because `Infrastructure\WebGlobalInitialization.cs` is an auto-generated code file by ServiceMatrix, you should not edit it directly (or else your changes will be gone the next time it is rebuilt). Instead, add a new class file named `ConfigurePersistence.cs` to the Infrastructure folder of the ASP.NET project. Update it to initialize the RavenDBPersistence class as follows:

````C#
using NServiceBus;
using NServiceBus.Persistence;

namespace OnlineSalesV5.eCommerce.Infrastructure
{
  public class ConfigurePersistence : INeedInitialization
  {
    public void Customize(BusConfiguration config)
    {
      config.UsePersistence<RavenDBPersistence>();
    }
  }
}
````

### Selecting Peristence for OrderProcessing NServiceBus Host Endpoint

In your OrderProcessing endpoint, you will find the setup in `EndpointConfig.cs`.

````C#
// For production use, please select a durable persistence.
// To use RavenDB, install-package NServiceBus.RavenDB and then use configuration.UsePersistence<RavenDBPersistence>();
// To use SQLServer, install-package NServiceBus.NHibernate and then use configuration.UsePersistence<NHibernatePersistence>();
if (Debugger.IsAttached)
{
  configuration.UsePersistence<InMemoryPersistence>();
}
````

Repeat the steps above to install the NServiceBus.RavenDB NuGet package into your OnlineSales.OrderProcessing project and modify the code in `EndpointConfig.cs`.

````C#
configuration.UsePersistence<RavenDBPersistence>();
````

You will need to add the following using statement at the top of EndpointConfig.cs as well:

````C#
using NServiceBus.Persistence;
````


### Installing RavenDB 2.5

An NServiceBus Version 5 ServiceMatrix project requires RavenDB Version 2.5. Download the installer from [ravendb.net](http://ravendb.net/download) and select "Development" for the target environment.

NOTE: If you already have RavenDB 2.0 installed, you can uninstall the service by finding the Raven.Server.exe executable on your machine and running it from the command line with /uninstall.

For more information on installing RavenDB for use with NService bus, refer to [this document](/nservicebus/ravendb/installation.md).


## Running the Application

Now press `F5` or press the 'Play' button in Visual Studio to debug the application. You should see both the eCommerce website launched in your default browser and a console window for the NServiceBus host that is running your OrderProcessing endpoint. 


### eCommerce Website

The ECommerce website generated by ServiceMatrix should look like the image below.

![ECommerce Website](images/servicematrix-demowebsite.png)

Notice the 'Try NServiceBus' box and the 'Test Messages' button on the right side. When you click the button another page opens with a button to publish the `SubmitOrder` to the bus as shown.

![Send Message MVC](images/servicematrix-demowebsite-sendmvc.png)

To send the `SubmitOrder` message just click the word 'Send!'. Go ahead and click to send a few times.


### Order Processing

Since you selected the NServiceBus host for your OrderProcessing endpoint it is launched as a console application for convenient development. Your console window should look like this.

![OrderProcessing Console](images/servicematrix-reqresp-orderprocessor.png)

As you click the Send button in the website, you will see the console indicate that the `OrderProcessing` endpoint has received the messages.


## Using ServiceInsight

By default, when you run a ServiceMatrix project, [ServiceInsight](/serviceinsight) is launched. ServiceInsight is another Particular Service Platform application that provides a detailed runtime view of your solution. It will illustrate and enumerate the messages, endpoints and data in your solution as you create an debug it. To understand how to use ServiceInsight to complement ServiceMatrix please see [this article on that topic](servicematrix-serviceinsight.md "Using ServiceInsight and ServiceMatrix"). 

![ServiceInsight](images/serviceinsight-screen.jpg)


### Congratulations!

You've just built your first NServiceBus application. Wasn't that easy?


## Next Steps

We mentioned that [ServiceInsight](/serviceinsight) can be a valuable tool in the design process and where to [learn more about it](servicematrix-serviceinsight.md). For runtime monitoring of an NServiceBus solution, the platform also includes [ServicePulse](/servicepulse). 

In this article you saw how to use ServiceMatrix to connect a front end website and a backend processing system using NServiceBus.

What's so exciting about that?  After all inter-process communication has been done many times before.

One answer is ***fault tolerance***. Next, you can explore the fault tolerance and durability features NServiceBus offers.
