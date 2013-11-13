---
title: "NServiceBus Step by Step Guide"
tags: 
---

In this tutorial we are going to create a very simple ordering system that will send messages from a client to a server.

#### Creating the Client

Lets start by creating a **Client** project that will send order requests to a NServiceBus endpoint.

To start create an empty Visual Studio solution, call it
**OrderingSolution** , and then add a *Class Library* project to it and call it **Client** .

We now need to add references the NServiceBus assemblies and the quickest and easiest way to do that is to open the NuGet Package Manager Console and type:

<div class="nuget-badge">
`PM> Install-Package NServiceBus.Host`{style="background-color: rgb(32, 32, 32); border: 4px solid rgb(192, 192, 192); border-top-left-radius: 5px; border-top-right-radius: 5px; border-bottom-right-radius: 5px; border-bottom-left-radius: 5px; box-shadow: rgb(110, 110, 110) 2px 2px 3px; color: rgb(226, 226, 226); display: block; font-size: 1.2em; font-family: 'andale mono', 'lucida console', monospace; line-height: 1.2em; overflow: auto; padding: 1px;"}


Open the EndpointConfig.cs file that was just created for you and replace

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server

<span>with</span>

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Client

We will add more code to the **Client** project later on but now we are going to concentrate on the area that will handle our order requests.

#### Creating the Message

First lets add a new *Class Library* project and call it **Messages** .

Install the NServiceBus nuget package on this new project:

<div class="nuget-badge">
`PM> Install-Package NServiceBus.Interfaces -ProjectName Messages`{style="background-color: rgb(32, 32, 32); border: 4px solid rgb(192, 192, 192); border-top-left-radius: 5px; border-top-right-radius: 5px; border-bottom-right-radius: 5px; border-bottom-left-radius: 5px; box-shadow: rgb(110, 110, 110) 2px 2px 3px; color: rgb(226, 226, 226); display: block; font-size: 1.2em; font-family: 'andale mono', 'lucida console', monospace; line-height: 1.2em; overflow: auto; padding: 1px;"}


And finally replace the whole *Class1.cs* (if you want you can rename the file to PlaceOrder.cs) file with:

    namespace Messages
    {
      using NServiceBus;
      public class PlaceOrder : ICommand
      {
        public string Product { get; set; }
      }
    }

#### Creating the Server

We are now ready to create our orders processing server, start by adding a *Class Library* project and call it **Server** , and execute the following nuget command:

<div class="nuget-badge">
`PM> Install-Package NServiceBus.Host -ProjectName Server`{style="background-color: rgb(32, 32, 32); border: 4px solid rgb(192, 192, 192); border-top-left-radius: 5px; border-top-right-radius: 5px; border-bottom-right-radius: 5px; border-bottom-left-radius: 5px; box-shadow: rgb(110, 110, 110) 2px 2px 3px; color: rgb(226, 226, 226); display: block; font-size: 1.2em; font-family: 'andale mono', 'lucida console', monospace; line-height: 1.2em; overflow: auto; padding: 1px;"}


Then we need to add a reference to the **Messages** project create above.

Once that is done copy and paste the following code into the *Class1.cs*
(if you want you can rename the file to PlaceOrderHandler.cs) file:

    namespace Server
    {
      using System;
      using Messages;
      using NServiceBus;
      public class PlaceOrderHandler : IHandleMessages
      {
        public void Handle(PlaceOrder message)
        {
          Console.Out.WriteLine(@"Order for ""{0}"" placed.", message.Product);
        }
      }
    }

#### Sending the order

We nearly done, all it is left to do is to go back to the **Client** project add a reference to the **Messages** project and copy and paste the following code into the *Class1.cs* (if you want you can rename the file to SendOrder.cs) file:

    namespace Client
    {
      using Messages;
      using NServiceBus;
      public class SendOrder : IWantToRunWhenBusStartsAndStops
      {
        public IBus Bus {  get; set; }
        public void Start()
        {
          Bus.Send("Server", new PlaceOrder() {Product = "New shoes"});
        }
        public void Stop()
        {
        }
      }
    }

NOTE: In version 4.0, the interface IWantToRunAtStartup has been replaced with IWantToRunWhenBusStartsAndStops

#### Running the solution

To run the **Client** and **Server** projects together so you can see it all working, right click on the **OrderingSolution** and select "Set StartUp Projects..." in that screen select "Multiple startup projects" and set the **Client** and **Server** action to be "Start". 

Finally press "F5" to run the solution.

Two console application windows should start up and you should see
"Order for "New shoes" placed." in one of them.

**Congratulations - you've just built your first NServiceBus application.**

**Wasn't that easy?**

\* If you see some warnings on the consoles, these warnings are just NServiceBus telling you that it couldn't find the queues it needs, 

so it went ahead and created them for you.

