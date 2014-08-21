---
title: Hosting NServiceBus v3.x in Process
summary: Fluent configuration API to get transnational one-way messaging, referencing only three assemblies.
tags: []
---

Lighter-weight than BizTalk and more powerful than WCF, NServiceBus comes with its own host process and allows you to host it in your own process.

Requiring as few as three assemblies to be referenced, the Fluent configuration API can get you up and running with transactional one-way messaging in a snap.

Assembly references
-------------------

![Assembly references](webapp_references.png)

To host NServiceBus in your own process, the assemblies shown on the left need to be referenced:

-   Log4Net is the industry-standard logging library used by NServiceBus.
-   NServiceBus.dll contains the main interfaces developers should be programming against.
-   NServiceBus.Core.dll contains all the runtime elements needed for execution.

The [AsyncPages sample](https://github.com/Particular/NServiceBus/tree/3.3.8/Samples) demonstrates this configuration for v3.x.


NServiceBus initialization
--------------------------

In the `ApplicationStart` method of your Global.asax file in a web application, or in the `Main` method of your `Program` file for console or Windows Forms applications, include the following initialization code:

For NServiceBus v3.x:

```C#
NServiceBus.Configure.With()
    .DefaultBuilder()
    .Log4Net()
    .XmlSerializer()
    .MsmqTransport()
    .UnicastBus()
    .CreateBus()
    .Start();
```

Here are some usage samples:

NServiceBus v3.x in ASP.Net MVC:

```C#
protected void Application_Start()
{
    AreaRegistration.RegisterAllAreas();

    RegisterGlobalFilters(GlobalFilters.Filters);
    RegisterRoutes(RouteTable.Routes);
   
    // NServiceBus configuration
    Configure.With()
        .DefaultBuilder()
        .ForMvc()
        .JsonSerializer()
        .Log4Net()
        .MsmqTransport()
            .IsTransactional(false)
            .PurgeOnStartup(true)
        .UnicastBus()
            .ImpersonateSender(false)
        .CreateBus()
        .Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());
}
```

NServiceBus v3.x in ASP.Net Web:

```C#
public class Global : HttpApplication
{
    public static IBus Bus { get; private set; }

    protected void Application_Start(object sender, EventArgs e)
    {
        Bus = Configure.With()
            .Log4Net()
            .DefaultBuilder()
            .XmlSerializer()
            .MsmqTransport()
                .IsTransactional(false)
                .PurgeOnStartup(false)
            .UnicastBus()
                .ImpersonateSender(false)
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());
    }

    protected void Application_End(object sender, EventArgs e)
    {

    }
}
```

Configuration Code
------------------

Most of the methods are extensions for the `NServiceBus.Configure` class provided by the specific components packaged in the `NServiceBus.Core` assembly. You can similarly configure your own components by writing your own extension methods.

-   `Log4Net()` tells NServiceBus what to [log](logging-in-nservicebus.md) with.
-   `DefaultBuilder()` tells NServiceBus to use the default(Autofac) dependency injection framework. Other [dependency injection frameworks](containers.md) are available as well.
-   `UnicastBus()` tells NServiceBus to use unicast messaging. This is currently the only option available out of the box. `LoadMessageHandlers()` readies the bus for invoking message handlers when a message arrives.
-   `CreateBus()` takes all the previous options and wires up a bus object for you to use. You can store the reference returned from this call for sending messages.
-   `Start()` tells the bus object created by `CreateBus()` to start its threads for listening and processing messages.

For NServiceBus v3.x:

-   `XmlSerializer()` tells NServiceBus to serialize messages as XML. Additional option is to specify `BinarySerializer()`, which does binary serialization of messages.
-   `MsmqTransport()` tells NServiceBus to use MSMQ as its transactional messaging transport. NServiceBus also supports Azure queues (see sample [here](https://github.com/Particular/NServiceBus.Azure.Samples)) as transport mechanisms.

In addition to the above initialization code, NServiceBus requires certain configuration data to be available. By default, it retrieves this information from the application config file, though this can be changed with the `CustomConfigurationSource()` method.


Configuration files
-------------------

To use the initialization code above, provide configuration for the `MsmqTransport` and processing of faults; specifically, the number of threads it runs, and where it sends messages that cannot be processed.

Include these configuration sections:

For NServiceBus v3.x:

```XML
<section name="MsmqTransportConfig" type="NServiceBus.Config.MsmqTransportConfig, NServiceBus.Core"/>
<section name="MessageForwardingInCaseOfFaultConfig" type="NServiceBus.Config.MessageForwardingInCaseOfFaultConfig, NServiceBus.Core" />

<!-- Specify the configuration data, as follows: -->

<MsmqTransportConfig NumberOfWorkerThreads="1" MaxRetries="5"  />
<MessageForwardingInCaseOfFaultConfig ErrorQueue="error"/>
```

If an exception is thrown during the processing of a message, NServiceBus automatically retries the message (as it might have failed due to something transient like a database deadlock). MaxRetries specifies the maximum number of times this is done before the message is moved to the ErrorQueue.

Routing configuration
---------------------

While you can tell NServiceBus to which address to send a message using the API: `Bus.Send(toDestination, message);` NServiceBus enables you to keep your code decoupled from where endpoints are deployed on the network through the use of routing configuration. Include this configuration section:

```XML
<section name="UnicastBusConfig" type="NServiceBus.Config.UnicastBusConfig, NServiceBus.Core"/>

<!-- And then specify the configuration data like this: -->

<UnicastBusConfig>
<MessageEndpointMappings>
    <add Messages="MessageDLL" Endpoint="DestinationQueue@TargetMachine"/>
</MessageEndpointMappings>
</UnicastBusConfig>  
```

This tells NServiceBus that all messages in the MessageDLL assembly should be routed to the queue called DestinationQueue on the machine TargetMachine. You can send messages from that assembly, like this: `Bus.Send(messageFromMessageDLL)`;



