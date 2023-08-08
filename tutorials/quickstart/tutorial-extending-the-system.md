---
title: "NServiceBus Quick Start: Extending the system"
reviewed: 2022-06-29
isLearningPath: true
summary: "Part 3: Learn how easy it is to extend a distributed system by adding new functionality without affecting the other components of the system"
extensions:
- !!tutorial
  nextText: "Next: NServiceBus from the ground up"
  nextUrl: tutorials/nservicebus-step-by-step/1-getting-started
---

In the [first part of the tutorial](/tutorials/quickstart), we saw that publishing events using the [Publish-Subscribe pattern](/nservicebus/messaging/publish-subscribe/) reduces coupling and makes maintaining a system easier in the long run. Next, we saw [how to react to failures](/tutorials/quickstart/tutorial-reliability.md) gracefully. Let's now look at how we can add an additional subscriber without needing to modify any existing code.

{{NOTE:
If you didn't already download the quick start project in the [previous lesson](/tutorials/quickstart), you can download it now:

downloadbutton
}}

As shown in the diagram, we'll be adding a new messaging endpoint called **Shipping** that will subscribe to the `OrderPlaced` event.

![Completed Solution](after.svg "width=680")

NOTE: In this tutorial, we'll use terminal commands like [`dotnet new`](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new), [`dotnet add package`](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-add-package), and [`dotnet add reference`](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-add-reference), but you can do the same things using the graphical tools in Visual Studio if you prefer.

### Create a new endpoint

First we'll create the **Shipping** project and set up its dependencies.

First let's make sure both browser windows and all console applications are closed, and in the terminal,  we're in the root of the project where the **RetailDemo.sln** file is located:

```shell
> cd tutorials-quickstart
```

Next, we'll create a new Console Application project named **Shipping** and add it to the solution:

```shell
> dotnet new console --name Shipping --framework net6.0
> dotnet sln add Shipping
```

Now, we need to add references to the **Messages** project, as well as the NuGet packages we will need.

```shell
> dotnet add Shipping reference Messages

> dotnet add Shipping package NServiceBus
> dotnet add Shipping package NServiceBus.Extensions.Hosting
> dotnet add Shipping package NServiceBus.Heartbeat
> dotnet add Shipping package NServiceBus.Metrics.ServiceControl
```

Now that we have a project for the Shipping endpoint, we need to add some code to configure and start an `NServiceBus` endpoint. In the **Shipping** project, find the auto-generated **Program.cs** file and replace its contents with the following.

snippet: ShippingProgram

Take special note of the comments in this code, which annotate the various parts of the NServiceBus configuration we're using.

We want the **Shipping** endpoint to run when you debug the solution, so use Visual Studio's [multiple startup projects](https://docs.microsoft.com/en-us/visualstudio/ide/how-to-set-multiple-startup-projects) feature to configure the **Shipping** endpoint to start along with **ClientUI**, **Sales**, and **Billing**.

NOTE:  To launch the Shipping endpoint with the rest of the solution when using Visual Studio Code, navigate to the _Run and Debug_ tab and select the _Debug All + Shipping_ launch configuration from the dropdown list.

### Create a new message handler

Next, we need a message handler to process the `OrderPlaced` event. When NServiceBus starts, it will detect the message handler and handle subscribing to the event automatically.

To create the message handler:

1. In the **Shipping** project, create a new class named `OrderPlacedHandler`.
1. Mark the handler class as public, and implement the `IHandleMessages<OrderPlaced>` interface.
1. Add a logger instance, which will allow us to take advantage of the logging system used by NServiceBus. This has an important advantage over `Console.WriteLine()`: the entries written with the logger will appear in the log file in addition to the console. Use this code to add the logger instance to the handler class:
    ```cs
    static ILog log = LogManager.GetLogger<OrderPlacedHandler>();
    ```
1. Within the `Handle` method, use the logger to record when the `OrderPlaced` message is received, including the value of the `OrderId` message property:
    ```cs
    log.Info($"Shipping has received OrderPlaced, OrderId = {message.OrderId}");
    ```
1. Since everything we have done in this handler method is synchronous, return `Task.CompletedTask`.

When complete, the `OrderPlacedHandler` class should look like this:

snippet: OrderPlacedHandler

### Run the updated solution

Now run the solution, and assuming you remembered to [update the startup projects](https://msdn.microsoft.com/en-us/library/ms165413.aspx), a window for the **Shipping** endpoint will open in addition to the other three.

![Addition of Shipping endpoint](add-shipping-endpoint-2.png)

As you place orders by clicking the button in the **ClientUI** web view, you will see the **Shipping** endpoint reacting to `OrderPlaced` events:

```
INFO Shipping has received OrderPlaced, OrderId = 25c5ba63
```

**Shipping** is now receiving events published by **Sales** without having to change the code in the **Sales** endpoint. Additional subscribers could be added, for example, to email a receipt to the customer, notify a fulfillment agency via a web service, update a wish list or gift registry, or update data on items that are frequently bought together. Each business activity would occur in its own isolated message handler and doesn't depend on what happens in other parts of the system.

NOTE: You may also want to take a look at the ServicePulse window, where you should now be able to see heartbeat and endpoint monitoring information for the new endpoint as well.

## Summary

In this tutorial, we explored the basics of how a messaging system using NServiceBus works.

We learned that asynchronous messaging failures in one part of a system can be isolated and prevent complete system failure. This level of resilience and reliability is not easy to achieve with traditional REST-based web services.

We saw how automatic retries provide protection from transient failures like database deadlocks. If we implement a multi-step process as a series of message handlers, then each step will be executed independently and can be automatically retried in case of failures. This means that a stray exception won't abort an entire process, leaving the system in an inconsistent state.

We saw how the tooling in the Particular Service Platform makes running a distributed system much easier. ServicePulse gives us critical insights into the health of a system, and allows us to diagnose and fix systemic failures. We don't have to worry about data loss—once we redeploy our system, we can replay failed messages in batches as if the error had never occurred.

We also implemented an additional event subscriber, showing how to decouple independent bits of business logic from each other. The ability to publish one event and then implement resulting steps in separate message handlers makes the system much easier to maintain and evolve.

SUCCESS: Now that you've seen what NServiceBus can do, take the next step and learn how to build a system like this one from the ground up. In the next tutorial, find out how to build the same solution starting from **File** > **New Project**.

<style type="text/css">
  .btn-outline {
    border: 1px solid #00A3C4;
    background-color: #fff;
    color: #00A3C4;
    margin-right: 15px;
    padding-left: 45px;
    background: url('tweet.svg') no-repeat left 15px top 11px / 22px 22px;
  }

  .btn-outline:hover {
    background: url('tweet-hover.svg') no-repeat left 15px top 11px / 22px 22px, #00A3C4;
  }
</style>

<script src="//platform.twitter.com/oct.js" type="text/javascript"></script>
<script async src="https://www.googletagmanager.com/gtag/js?id=AW-691241604"></script>
<script type="text/javascript">
  // Twitter view
  window.twttr && twttr.conversion.trackPid('o3bkg', { tw_sale_amount: 0, tw_order_quantity: 0 });
  window.dataLayer = window.dataLayer || [];
  function gtag(){dataLayer.push(arguments);}
  gtag('js', new Date());
  gtag('config', 'AW-691241604', {'transport_type': 'beacon'});
  // Google view
  gtag('event', 'conversion', {'send_to': 'AW-691241604/vSZvCJ-K78kBEISFzskC'});
  (function () {
    var onJQuery = function () {
      $('.inline-download .dropdown-menu a:first').click(function(e) {
        // Twitter download
        twttr.conversion.trackPid('o3ay4', { tw_sale_amount: 0, tw_order_quantity: 0 });
        // Google download
        gtag('event', 'conversion', {
          'send_to': 'AW-691241604/ERjYCIn31ckBEISFzskC',
          'transaction_id': ''
        });
      });
      $(function () {
        $('.tutorial-actions').prepend('<a href="#" id="tweet-completion" class="btn btn-outline btn-info btn-lg">Share your accomplishment</a>');
        $('#tweet-completion').on('click', function (e) {
          e.preventDefault();
          gtag('event','quick_start_tweet_completion_click', { 'send_to': 'G-GMZ1FS541B' });
          window.ga && window.ga('send', 'event', 'QuickStart', 'TweetCompletionClick');
          window.open('https://twitter.com/intent/tweet?text=' + encodeURIComponent('I just completed the #NServiceBus Quick Start tutorial at docs.particular.net/tutorials/quickstart'));
        });
      });
    };
    var init = function () {
      if(window.$) {
        onJQuery();
      } else {
        setTimeout(function() { init(); }, 500);
      }
    };
    init();
  }());
</script>
