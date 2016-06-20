---
title: NServiceBus and BizTalk
summary: NServiceBus guides away from dangerous anti-patterns while providing messaging patterns and integration.
redirects:
 - nservicebus/nservicebus-and-biztalk
---

BizTalk is a good centralized message broker with many adapters for third party applications, but service buses are inherently distributed, not centralized. Logical centralization leads to spaghetti code.

Mixing logical orchestration and routing with business logic, data access, and web services calls is not a good idea and leads to slow, unmaintainable code.

NServiceBus guides away from these dangerous anti-patterns while still providing messaging patterns and integration.


## Best of both worlds

In many cases it is necessary to integrate code with existing systems and legacy applications possibly running on different technologies and proprietary protocols. This is a classical Enterprise Application Integration (EAI) situation and is not what service buses are meant to address.

In these cases, between the high-level business services NServiceBus can be used, and within the relevant services, behind the service boundary, BizTalk can be used to perform the integration with existing systems and legacy applications. Here's how it looks:

![How NServiceBus and BizTalk fit together in an architecture](nservicebus-biztalk.png)

Note the use of BizTalk behind a service boundary is something of an implementation/integration detail. By keeping the scope of the problem domain small, using BizTalk for a small orchestration to synchronize customer information between Oracle PeopleSoft and SalesForce won't run into either performance or maintainability problems.


## Next steps

Sometimes a hammer is required, sometimes a screwdriver, and sometimes both. While a Swiss army knife may appear to do both, it is a poor choice for any but the most trivial undertakings.

To learn more about dividing up an architecture into high-level business services, see the [presentation Udi gave on SOA](principles.md).

