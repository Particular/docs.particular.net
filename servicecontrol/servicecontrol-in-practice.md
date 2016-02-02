title: ServiceControl in Practice
summary: Details how to think about the use of ServiceControl in production environments
tags:
- ServiceControl
---

ServiceControl is a tool that can accommodate many situations. Certain features come with a cost and in some cases the cost of having a feature active is not worth the performance impact on the system.

For example, in your development environment you will want a lot of logging information to support problem analysis. For many users the [Debug Session](/servicecontrol/plugins/debug-session.md) plugin is really useful at this stage of the application life-cycle but you would never put that plugin into production. For the same reason, we advise that customers never use the [Saga Audit](/servicecontrol/plugins/saga-audit.md) plugin outside of development.

Making decisions about the use of the other plugins and features requires a little more thought to balance the smooth running of a system with actual business requirements. The temptation to 'just use them all' as a catch-all insurance policy is not a good choice. The cost of having some of these features in play can actually cause issues later on if you are not prepared.

[Auditing](/nservicebus/operations/auditing.md) is a good example of how the cost of having a record of everything may cause significant impact on the system. It might be worth considering turning audit off in some or all endpoints in production. Or if you absolutely need it move the auditing to another place like your Business Intelligence database or somewhere that has better resources dedicated to the problem. Remember audits in ServiceControl are primarily for performing system analysis using ServiceInsight. If you are not using ServiceInsight in production then do not use audits in production without having good reason.

[Heartbeats](/servicepulse/intro-endpoints-heartbeats.md) and [Custom Checks](/servicepulse/intro-endpoints-custom-checks.md) are great for knowing when an endpoint is up or down but they add extra noise to your system. Definitely think hard about what the business ramifications are of having each endpoint available. Often, not all endpoints are mission critical, the default heartbeat is forty seconds. Maybe ten second updates are right for some endpoints but two minute intervals could be better for others. Remember also that all communication from endpoints with ServiceControl is performed via messaging. Adding a message to the queue every second may have little impact on when the notification shows up in ServicePulse. Remember, the last heartbeat is really the only one you care about. If you stop ServiceControl those heartbeats are going to bank up in the queue. You might want to consider flushing the heartbeats from the queue before starting ServiceControl again. Having a lot of noisy messages that add no value may overwhelm ServiceControl and cause it fail.

##### Less can be more

Moving forward, take a thoughtful approach in the adoption of plugins and features:

- [Turn off auditing](http://docs.particular.net/nservicebus/operations/auditing#turning-off-auditing) on all endpoints as well as [heartbeats and custom checks](http://docs.particular.net/servicepulse/how-to-configure-endpoints-for-monitoring).
- Perform load tests to baseline the solution.
- When comfortable with the performance of the system try adding [Heartbeats](http://docs.particular.net/servicepulse/intro-endpoints-heartbeats). That will allow you to monitor your system
- Try increasing the [heartbeat interval](http://docs.particular.net/servicecontrol/plugins/heartbeat#configuration-heartbeat-interval). Ideally you will not want to have a heartbeat update more frequently than ServiceControl can process it or more than your Operations staff are prepared to look at ServicePulse.
- With each additional change perform a load test again adjusting the heartbeat interval satisfied with the result.
- Repeat the process of considering the business relevance, system impact and load testing with each [Custom Check](http://docs.particular.net/servicecontrol/plugins/custom-checks) in your system.
- Repeat the process of considering the business relevance, system impact and load testing with each Endpoint [Audit](http://docs.particular.net/nservicebus/operations/auditing) in your system.
