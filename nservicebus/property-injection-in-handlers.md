---
title: Property injection in handlers
summary: How to configure property injection for handlers
tags: 
- Dependency Injection
- IOC
---

INFO: This is relevant to versions 5.2 and above.

In previous versions of NServiceBus, to inject property values into a handler type you could do the following:
<!-- import ConfigurePropertyInjectionForHandlerBefore --> 
This is no longer supported.

From v5.2 and above a new and more explicit API has been introduced.

### Here is how to use the new API
Given the following handler class:
<!-- import PropertyInjectionWithHandler --> 

To inject a value into both `SmtpAddress` and `SmtpPort` you need to at configuration time call `InitializeHandlerProperty<THandler>` with the handler and values you intend to inject, here is an example:
<!-- import ConfigurePropertyInjectionForHandler --> 