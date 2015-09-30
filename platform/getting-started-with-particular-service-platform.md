---
title: Getting Started with the Particular Service Platform
summary: 'Introduction to the Particular ServicePlatform a simple send and receive example.'
tags:
- Platform
- Send and Receive
- Visual Studio
---

This step-by-step guide to the Particular Service Platform walks you through the creation of a send-and-receive NServiceBus distributed application using ServiceMatrix v2.0 for Visual Studio 2012, using the following steps:

1.  [Creating a New Project](#creating-a-new-project)
1.  [Creating Endpoints](#creating-endpoints)
1.  [Creating a Message](#creating-a-message)
1.  [Creating Services](#creating-services)
1.  [Deploying Components](#deploying-components)
1.  [Handling a Message](#handling-a-message)
1.  [Running the Application](#running-the-application)
1.  [Using ServiceInsight](#using-serviceinsight)
1.  [Next Steps](#next-steps)

The example demonstrates the integration of an online sales web store with a backend system using the request-response pattern and NServiceBus.

NOTE: For a code only introduction to NServiceBus, see:

* [NServiceBus Overview](/nservicebus/architecture/) 
* [NServiceBus Step by Step Guide](/samples/step-by-step/) 

## Creating a New Project

To get started open up Visual Studio 2015 and create a new Blank Solution called `OnlineSales`.

Add two new Class Library Projects `OnlineSales.Internal` and `OnlineSales.Contract` and delete `Class1` from each of them.


The `Contract` and `Internal` projects as they are where all message types are placed:
-   All defined events will be put in the `Contract` project.
-   All commands will be put in the `Internal` project. 

Later you will see how messages from different services are partitioned in these projects. 

## Building the Online Sales Solution

Our online sales example involves a website that collects online orders and a back-end order processing system that processes them.  

To build the solution you will define and endpoint for the website and another endpoint for the order processing system.  A new 'Sales' service will define components for submitting and processing orders as well as a command message to represent the order submission.  


## Creating Endpoints

First you will create the endpoints for selling and processing.


### New endpoint

In Visual Studio add a new Empty ASP.NET MVC Project called `ECommerce`
 

### Create OrderProcessing endpoint

In Visual Studio add a new Class Library Project called `OnlineSales.Sales`.  

Open up Package Manager Console making sure that the highlighted project is `OnlineSales.Sales` and run this command 

`Install-Package NServiceBus`

This will install nServiceBus into your project.


## Creating a Message

Communication between the website and the back-end `OrderProcessing` endpoint will be done with a command message. 

## Creating Services 

#PLACEHOLDER

## Deploying Components

#PLACEHOLDER

## Create a Message

In the `OnlineSales.Internal` project add a class with this code.

```C#
namespace OnlineSales.Internal
{
    public class SubmitOrder
    {
		//Put your properties in the class.
		//public string CustomerName { get; set; }
    }
}
```


## Handling a Message

In the `OnlineSales.Sales` project add a class with this code. You will have to reference `OnlineSales.Internal` to get `SubmitOrder`.

```C#
using System;
using NServiceBus;
using OnlineSales.Internal;

namespace OnlineSales.Sales
{
    public class SubmitOrderHandler : IHandleMessages<SubmitOrder>
    {
        public void Handle(SubmitOrder message)
        {
             Console.WriteLine("Sales received " + message.GetType().Name);
        }
		
		public IBus Bus { get; set; }
    }
}
```



## Sending a Message 

The last thing to do is for the 'ECommerce' website sends a message. 


### Review MVC code

Find the `TestMessagesController.generated.cs` file in the Controllers folder in the OnlineSales.ECommerce project.  This file is generated as part of the MVC application by ServiceMatrix. Notice the `SubmitOrderSender.Send` method that sends the command message `SubmitOrder`.  This method was generated in a different partial class file located in the `Infrastructure\Sales\SubmitOrderSender.cs` file.  

```C#
namespace OnlineSales.ECommerce.Controllers
{
    public partial class TestMessagesController : Controller
    {
        // GET: /TestMessages/

        public ActionResult Index()
        {
            return View();
        }

        // POST: /TestMessages/SendMessageSubmitOrder
          
        public ISubmitOrderSender SubmitOrderSender { get; set; }
          
        [HttpPost]
        public ActionResult SendMessageSubmitOrder(SubmitOrder SubmitOrder)
        {
            ConfigureSubmitOrder(SubmitOrder);
            SubmitOrderSender.Send(SubmitOrder);

            ViewBag.MessageSent = "SubmitOrder";

            return View("Index");
        }
        partial void ConfigureSubmitOrder(SubmitOrder message);
    }
}
```  

This is a demonstration site that provides an initial reference application in MVC.  Any modifications to this file will be overwritten by subsequent regeneration of the demonstration site.  To accomodate any changes you wish to make, just before the `SubmitOrderSender.Send` is called the code invokes a partial method called `ConfigureSubmitOrder` that accepts your `SubmitOrder` message as a parameter.  This can be implemented by you inside the `SubmitOrderSender.cs` file in the `\Sales` directory of the `OnlinesSales.ECommerce` project.  The following code snippet illustrates how that can be done.  

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


## Running the Application

Now press `F5` or press the 'Play' button in Visual Studio to debug the application. You should see both the eCommerce website launched in your default browser and a console window for the NServiceBus host that is running your OrderProcessing endpoint.  


### eCommerce website

To send the `SubmitOrder` message just click the word 'Send!'. Go ahead and click to send a few times.


### Order processing

#PLACEHOLDER

As you click the Send button in the website, you will see the console indicate that the `OrderProcessing` endpoint has received the messages.


## Using ServiceInsight

#PLACEHOLDER


### Congratulations

You've just built your first NServiceBus application. Wasn't that easy?


## Next steps

We've mentioned that [ServiceInsight](/serviceinsight) can be a valuable tool in the design process and where to [learn more about it](/servicematrix/servicematrix-serviceinsight.md).  For runtime monitoring of an NServiceBus solution the platform also includes [ServicePulse](/servicepulse "ServicePulse for Monitoring").  

In this article you have seen how ServiceMatrix can be used to connect a front end website and a backend processing system using NServiceBus. 
What's so exciting about that?  After all inter-process communication has been done many times before. 

One answer is ***fault tolerance***.  Next we'll explore the fault tolerance and durability features NServiceBus offers.



#mORE sTUFF TO FIX
~Continue on and let's review the fault tolerance in our [next article](/servicematrix/getting-started-with-nservicebus-using-servicematrix-2.0-fault-tolerance.md "Getting Started with Fault Tolerance").~
