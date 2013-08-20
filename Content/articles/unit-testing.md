<!--
title: "Unit Testing"
tags: 
-->
Developing enterprise-scale distributed systems is hard and testing them is just as challenging a task. The architectural approach supported by NServiceBus makes these challenges more manageable. And the testing facilities provided actually make unit testing endpoints and workflows easy. You can now develop your service layers and long-running processes using test-driven development.

Unit testing the service layer
------------------------------

The service layer in an NServiceBus application is made from message handlers. Each class typically handles one specific type of message. Testing these classes usually focuses on their externally visible behavior: the types of messages they send or reply with. This is as simple to test as could be expected:

<p>
<script src="https://gist.github.com/Particular/6033906.js"></script>
</p> This test says that when a message of the type YourRequestMessage is processed by YourMessageHandler, it responds with a message of the type YourResponseMessage. Also, if the request message's String property value is "hello" than that is also the value of the String property of the response message.

<p>
<script src="https://gist.github.com/Particular/6034023.js"></script>
</p> Testing header manipulation
---------------------------

It is the responsibility of the message handlers in the service layer is to use data from headers found in the request to make decisions, and to set headers on top of the response messages. This is how this kind of functionality can be tested:

<p>
<script src="https://gist.github.com/Particular/6034076.js"></script>
</p> This test asserts that the value of the outgoing header has been set.

Injecting additional dependencies into the service layer
--------------------------------------------------------

Many of the message handling classes in the service layer make use of other objects to perform their work. When testing these classes, replace those objects with "stubs" so that the class under test is isolated. Here's how:

<p>
<script src="https://gist.github.com/Particular/6034088.js"></script>
</p>

Many message handlers are dependent on the bus. That dependency is filled automatically by the testing infrastructure and does not need to be tested.

Other service layer testing functionality
-----------------------------------------

For every method on the bus there is a corresponding test method that sets up an expectation for that call.

-   See [all methods available to unit test
    handlers](http://github.com/NServiceBus/NServiceBus/blob/master/src/testing/Handler.cs)
-   See [all methods available to unit test
    sagas](http://github.com/NServiceBus/NServiceBus/blob/master/src/testing/Saga.cs)


