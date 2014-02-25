---
title: ServiceInsight Application Invocation
summary: How the integration between ServicePulse and ServiceInsight works and how to use the parametrized invocation of ServiceInsight.
tags: 
- Integration
- Application Invocation
---

One thing we've been thinking about when building the NServiceBus suit of tools was the integration between them. Since the tools are conceptually built for different purposes, we still need the people using them be able to collaborate.

To give you a better understanding, let's see a scenario. Supposing your support team are using ServicePuls to monitor your distributed system for errors, when they find an error message, they might need to have it investigated a bit more by a developer. What they can do is to use "Open in ServiceInsight" link when would open ServiceInsight and bring up that particular message that has failed so they you can see why from the exception information provided by ServiceInsight.

![ServicePulse Error Messages](007_servicepulseerrormessages.png)

Let's see how this is working internally and how you can benefit from it, even without having ServicePulse installed.

When you click the link on the SerivcePulse, it opens a link in your browser that looks like this:


    si://localhost:33333/api?search=487b5055-11bb-4a70-a4fd-a2c00125aa43


This is the URI that ServiceInsight listens to and it basically launches ServiceInsight and tells service insight to do the following:


- Automatically connect to localhost on port 33333
- Perform a search on a specific Message Id

This means that a specific message that was in the error queue will be displayed as soon the ServiceInsight is opened and you can see the causation of it.

As yo can see, there's no magic here and you can do this as well. Just by constructing the right URI and sending it via email to your development team, they can then paste the whole thing in the Run window (Win + R) or their browser and this will have the exact same effect.

Here is the list of all the supported parameters and a description of what the effect of them is:

- Search [String]: Performs a search using the full-text search feature of ServiceInsight. This can be a message identifier, a part of the message payload or any value in the message header.

- EndpointName [String]: Name of the endpoint to select upon startup. When you select an endpoint, the operations and message lists are filtered to the endpoint you have selected.

- AutoRefresh [Integer]: Turns the AutoRefresh option on, so that the view is refreshed based on the value provided here (in seconds).

When ServiceInsight is launched using these parameters, you should be able to see the effects from the UI (having the endpoint selected, message selected, etc.). If a passed-in argument is not supported, ServiceInsight it will notify you once it runs.