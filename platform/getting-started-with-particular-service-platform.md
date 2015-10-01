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

### New Endpoint

In Visual Studio add a new Empty ASP.NET MVC Project called `ECommerce`
 
Open up Package Manager Console making sure that the highlighted project is `OrderProcessing` and run this command 

`Install-Package NServiceBus`
`Install-Package Autofac`
`Install-Package Autofac.Mvc5`
`Install-Package NServiceBus.Autofac `

Also reference `System.Configuration` and `OnlineSales.Internal`

Replace the code in `Global.asax` with this

```C#
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using NServiceBus;

namespace ECommerce
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private ISendOnlyBus _bus;

        protected void Application_Start()
        {
         

            ContainerBuilder builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Set the dependency resolver to be Autofac.
            IContainer container = builder.Build();

            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("Samples.MvcInjection.WebApplication");
            // ExistingLifetimeScope() ensures that IBus is added to the container as well,
            // allowing you to resolve IBus from your own components.
            busConfiguration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.EnableInstallers();

            _bus = Bus.CreateSendOnly(busConfiguration);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }


        public override void Dispose()
        {
            _bus?.Dispose();
            base.Dispose();
        }
    }
}
```





### Create OrderProcessing endpoint

In Visual Studio add a new Console Application Project called `OrderProcessing`.  

Open up Package Manager Console making sure that the highlighted project is `OrderProcessing` and run this command 

`Install-Package NServiceBus`

This will install nServiceBus into your project.

Also reference `System.Configuration` and `OnlineSales.Internal`

Add a class called `ConfigErrorQueue` with this code.

```C#
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

class ConfigErrorQueue : IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
{
    public MessageForwardingInCaseOfFaultConfig GetConfiguration()
    {
        return new MessageForwardingInCaseOfFaultConfig
        {
            ErrorQueue = "error"
        };
    }
}
```

And replace the contents of `Program.cs` with this code

```C#
using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("OrderProcessing");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("\r\nBus created and configured; press any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}
```

With `OrderProcessing` Set as your Startup project hit `F5` it should compile and run with a bunch of info flashing by as the Endpoint prepares to receive messages. `Hit a key` to Quit.

You'll note that we are using `InMemoryPersistence` here. It is really handy for developing your ideas and testing but it is not what you would want to use in production.


## Creating a Message

Communication between the website and the back-end `OrderProcessing` endpoint will be done with a command message. 

In the `OnlineSales.Internal` project add a class with this code.

```C#
public class SubmitOrder
{
	//Put your properties in the class.
	public string CustomerName { get; set; }
}

```

## Handling a Message

In the `OrderProcessing` project add a class with this code. You will have to reference `OnlineSales.Internal` to get `SubmitOrder`.

```C#
using System;
using NServiceBus;

public class SubmitOrderHandler : IHandleMessages<SubmitOrder>
{
    public void Handle(SubmitOrder message)
    {
         Console.WriteLine("Sales received " + message.GetType().Name);
    }
	
	public IBus Bus { get; set; }
}

```

## Sending a Message 

The last thing to do is get the `ECommerce` website to send a message. 

You'll remember that we added an IoC container to the `ECommerce` site. This will inject the bus into your controller so you can use it to send your message to your handler. 

Add these lines of code to the IndexController.

```C#

readonly IBus _bus;

public HomeController(IBus bus)
{
    this._bus = bus;
}


[AllowAnonymous]
public ActionResult Send()
{
    _bus.Send("OrderProcessing", new SubmitOrder());
    return RedirectToAction("Index", "Home");
}

```

Add a send button on the index view `Index.cshtml`

```HTML

<div class="row">
    <div class="col-md-12">
    	<h2>Send a Message</h2>
        <p><a class="btn btn-default" href="@Url.Action( "Send", "Home")">Send!</a></p>
    </div>
</div>

```

## Running the Application

Right click on the solution and goto `Properties`. Edit the startup project to be multiple projects and set  action to Start for `ECommerce` and `OrderProcessing` so that they with both run when you hit `F5`.

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
