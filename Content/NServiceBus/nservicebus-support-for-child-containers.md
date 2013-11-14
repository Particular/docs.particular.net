<!--
title: "NServiceBus Support for Child Containers"
tags: ""
summary: "<p>Child containers are essentially a snapshot of the main container where transient instances are treated as as singletons within the scope of the child container. This is useful when you want to scope instances for the duration of a web request or the handling of a message in NServiceBus. While this was possible before, child containers bring one more important feature to the table.</p>
<p><strong>NOTE</strong>: Child containers are not supported by spring.net, so if you plan to take advantage of it, use one of the other containers supported by NServiceBus.</p>
"
-->

Child containers are essentially a snapshot of the main container where transient instances are treated as as singletons within the scope of the child container. This is useful when you want to scope instances for the duration of a web request or the handling of a message in NServiceBus. While this was possible before, child containers bring one more important feature to the table.

**NOTE**: Child containers are not supported by spring.net, so if you plan to take advantage of it, use one of the other containers supported by NServiceBus.

Deterministic disposal
----------------------

Instance lifetime is usually not tracked by the container (Windsor is an exception) and that means that you have to manually call dispose any instance that needs deterministic disposal. Child containers solve this issue by automatically disposing all transient objects created within each specific child container.

This is very handy when it comes to managing things like the NHibernate session. See a more in-depth [description of child containers](http://codebetter.com/jeremymiller/2010/02/10/nested-containers-in-structuremap-2-6-1/).

NServiceBus creates a child container for each transport message that is received, remembering that transport messages can contain multiple
“user defined messages”. This means that all transient instances created during message processing are scoped as singletons within the child container. This allows you to easily share, for example, the NHibernate session between repositories, without messing around with thread static caching.

When the message finishes processing, the child container and all transient instances are disposed. So if you need deterministic disposal, implement IDisposable.

Beginning with NServiceBus V3, you can get a “session per transport message” by configuring the session as transient. This example uses StructureMap:

    var container = new Container(x =>
    {
        x.ForSingletonOf().Use(ConfigureSessionFactory());
        x.For().Use(ctx => ctx.GetInstance().OpenSession());
    });

This code allows you to inject your session to all components involved in processing each message:


    public class NHibernateMessageHandler:IHandleMessages
    {
        readonly ISession session;
        public NHibernateMessageHandler(ISession session)
        {
            this.session = session;
        }
        public void Handle(NHibernateMessage message)
        {
            session.Save(new PersistentEntity
                {
                    Data = "Whatever " + DateTime.Now.ToShortTimeString()
                });
        }
    }


When the message is processed, the session is disposed and all resources such as database connections are released.

Child containers are a powerful feature that can simplify your code and should definitely be in your toolbox.

If you configure your components using the NServiceBus configure API, it's possible to<span style="background-color:Lime;"> configure instance lifecyle to be per unit</span> of work, using this:


        Configure.Instance.Configurer.ConfigureComponent(DependencyLifecycle
    .InstancePerUnitOfWork);




