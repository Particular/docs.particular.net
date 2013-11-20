---
title: "Unit Testing"
tags: ""
summary: "Developing enterprise-scale distributed systems is hard and testing them is just as challenging a task. The architectural approach supported by NServiceBus makes these challenges more manageable. And the testing facilities provided actually make unit testing endpoints and workflows easy. You can now develop your service layers and long-running processes using test-driven development."
---

Developing enterprise-scale distributed systems is hard and testing them is just as challenging a task. The architectural approach supported by NServiceBus makes these challenges more manageable. And the testing facilities provided actually make unit testing endpoints and workflows easy. You can now develop your service layers and long-running processes using test-driven development.

Unit testing the service layer
------------------------------

The service layer in an NServiceBus application is made from message handlers. Each class typically handles one specific type of message. Testing these classes usually focuses on their externally visible behavior: the types of messages they send or reply with. This is as simple to test as could be expected:



```txt
public class TestHandler
{
    [Test]
    public void Run()
    {
        Test.Initialize();

        Test.Handler<MyHandler>()
            .ExpectReply<ResponseMessage>(m => m.String == "hello")
            .OnMessage<RequestMessage>(m => m.String = "hello");
    }

    public class MyHandler :
        IHandleMessages<RequestMessage>
    {
        public IBus Bus { get; set; }

        public void Handle(RequestMessage message)
        {
            var reply = new ResponseMessage
                        {
                            String = message.String
                        };
            Bus.Reply(reply);
        }
    }

    public class ResponseMessage : IMessage
    {
        public string String { get; set; }
    }

    public class RequestMessage : IMessage
    {
        public string String { get; set; }
    }

}

```



This test says that when a message of the type YourRequestMessage is processed by YourMessageHandler, it responds with a message of the type YourResponseMessage. Also, if the request message's String property value is "hello" than that is also the value of the String property of the response message.



```txt
[Test]
public void Run()
{
    Test.Initialize();
    Test.Saga<MySaga>()
            .ExpectReplyToOrginator<MyResponse>()
            .ExpectTimeoutToBeSetIn<StartsSaga>((state, span) => span == TimeSpan.FromDays(7))
            .ExpectPublish<MyEvent>()
            .ExpectSend<MyCommand>()
        .When(s => s.Handle(new StartsSaga()))
            .ExpectPublish<MyEvent>()
        .WhenSagaTimesOut()
            .AssertSagaCompletionIs(true);
}

public class MySaga : NServiceBus.Saga.Saga<MySagaData>,
                      IAmStartedByMessages<StartsSaga>,
                      IHandleTimeouts<StartsSaga>
{
    public void Handle(StartsSaga message)
    {
        ReplyToOriginator(new MyResponse());
        Bus.Publish(new MyEvent());
        Bus.Send(new MyCommand());
        RequestTimeout(TimeSpan.FromDays(7), message); //RequestUtcTimeout in 3.3
    }

    public void Timeout(StartsSaga state)
    {
        Bus.Publish<MyEvent>();
        MarkAsComplete();
    }
}
class MyCommand : ICommand
{
}

class MyEvent : IEvent
{
}
public class StartsSaga : ICommand
{
}

public class MyResponse : IMessage
{
}

public class MySagaData : IContainSagaData
{
    public Guid Id { get; set; }
    public string Originator { get; set; }
    public string OriginalMessageId { get; set; }
}

```



Testing header manipulation
---------------------------

It is the responsibility of the message handlers in the service layer is to use data from headers found in the request to make decisions, and to set headers on top of the response messages. This is how this kind of functionality can be tested:



```txt
[Test]
public void Run()
{
    Test.Initialize();

    Test.Handler<MyMessageHandler>()
        .SetIncomingHeader("Test", "abc")
        .OnMessage<RequestMessage>(m => m.String = "hello");

    Assert.AreEqual("myHeaderValue", Test.Bus.OutgoingHeaders["MyHeaderKey"]);
}

class MyMessageHandler : IHandleMessages<RequestMessage>
{
    public IBus Bus { get; set; }
    public void Handle(RequestMessage message)
    {
        Bus.OutgoingHeaders["MyHeaderKey"] = "myHeaderValue";
        Bus.Reply(new ResponseMessage());
    }
}

class RequestMessage : IMessage
{
    public string String { get; set; }
}

class ResponseMessage : IMessage
{
    public string String { get; set; }
}
```



This test asserts that the value of the outgoing header has been set.

Injecting additional dependencies into the service layer
--------------------------------------------------------

Many of the message handling classes in the service layer make use of other objects to perform their work. When testing these classes, replace those objects with "stubs" so that the class under test is isolated. Here's how:



```txt
[Test]
public void RunWithConstructorDependency()
{
    Test.Initialize();

    var mockService = new MyService();
    Test.Handler(bus => new WithConstructorDependencyHandler(mockService));
    //Rest of test
}

class WithConstructorDependencyHandler : IHandleMessages<MyMessage>
{
    MyService myService;

    public WithConstructorDependencyHandler(MyService myService)
    {
        this.myService = myService;
    }

    public IBus Bus { get; set; }

    public void Handle(MyMessage message)
    {
    }
}

[Test]
public void RunWithPropertyDependency()
{
    Test.Initialize();

    var mockService = new MyService();
    Test.Handler<WithPropertyDependencyHandler>()
        .WithExternalDependencies(handler => handler.MyService = mockService);
    //Rest of test
}

class WithPropertyDependencyHandler : IHandleMessages<MyMessage>
{
    public MyService MyService { get; set; }

    public void Handle(MyMessage message)
    {
    }
}


```




Many message handlers are dependent on the bus. That dependency is filled automatically by the testing infrastructure and does not need to be tested.

Other service layer testing functionality
-----------------------------------------

For every method on the bus there is a corresponding test method that sets up an expectation for that call.

-   See [all methods available to unit test handlers](http://github.com/NServiceBus/NServiceBus/blob/master/src/testing/Handler.cs)
-   See [all methods available to unit test sagas](http://github.com/NServiceBus/NServiceBus/blob/master/src/testing/Saga.cs)


