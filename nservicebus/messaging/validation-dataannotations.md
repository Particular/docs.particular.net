---
title: DataAnnotations message validation
summary: Validate message using DataAnnotations.
reviewed: 2018-06-02
component: DataAnnotations
related:
---

Uses [System.ComponentModel.DataAnnotations](https://msdn.microsoft.com/en-us/library/cc490428) to validate incoming and outgoing messages.

DataAnnotations message validation can be enabled using the following:

snippet: DataAnnotations

include: validationexception

By default only incoming messages are validated. To also validate outgoing messages use the following:

snippet: DataAnnotations_outgoing

include: validationoutgoing

Messages can then be decorated with DataAnnotations attributes. For example to make a property required use  the [RequiredAttribute](https://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.requiredattribute.aspx):

snippet: DataAnnotations_message