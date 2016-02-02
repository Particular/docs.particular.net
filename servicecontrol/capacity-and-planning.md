---
title: ServiceControl Capacity Planning
summary: Details the ServiceControl capacity, throughput, and storage considerations to plan and support production environments
tags:
- ServiceControl
---

ServiceControl is a monitoring tool for production environments. As with other production monitoring tools, you must plan for and maintain the deployment over time.

The primary job of ServiceControl is to collect information on system behavior in production. It does so by collecting error, audit and health messages from dedicated queues. ServiceControl reads the messages flowing into those queues and stores them in its embedded database. In a production environment (and to a lesser degree in development, staging and testing environments), ServiceControl has an impact on the disk space where its data is stored, and its throughput capacity needs to be considered with regard to the overall system load and throughput.

### Storage

#### Location

Each ServiceControl instance stores its data in a RavenDB embedded instance. The location of the database has a significant impact on the overall system behavior in terms of performance and throughput. You should configure the embedded database files in a high-performance storage device that is connected to the ServiceControl machine with a high-throughput connection.

#### Size

The storage size that ServiceControl requires depends on the production load and is directly related to the quantity and size of messages that flow into the system.

Since ServiceControl is intended to be a recent-history storage to support ServicePulse and ServiceInsight monitoring and debugging activity. This is different from a long-term data archiving system, that is intended to provide extremely long term archiving and storage solutions (measured in years, subject to various business or regulatory requirements).

ServiceControl is configured with a default expiration policy that deletes old messages after a predefined time. The expiration policy can be customized to decrease or increase the amount of time data is retained, which impacts the storage requirements of ServiceControl.

To limit the rate at which the database grows the body of an audit messages can be truncated if it exceeds a configurable threshold. 

Refer to Data Retention section of [Customizing ServiceControl Configuration](creating-config-file.md) for details on these settings.


**NOTE**

* The maximum supported size of the RavenDB embedded database is 16TB.
* Failed messages are *never* expired and are retained indefinitely in the ServiceControl database.

### Accessing data and audited messages

#### Alternate Audit and Error queues

You can configure ServiceControl to forward any consumed messages into alternate queues, so that a copy of any message consumed by ServiceControl is available from these alternate queues.

For more details, see [Forwarding Queues](errorlog-auditlog-behavior.md)

#### Query the ServiceControl HTTP API

This provides a JSON stream of audited and error messages (headers, body, and context) that can be imported into another database.

NOTE: ServiceControl HTTP API is subject to changes and enhancements that may not be fully backwards compatible. Use of this HTTP API is discouraged by 3rd parties at this time.

### Throughput

ServiceControl consumes audited, error and control messages in its database. It does so for all the endpoints that are configured to forward these messages to the queues monitored by ServiceControl. This means that the throughput (measured in received and processed messages per second) required by ServiceControl is the aggregate throughput of all the endpoints forwarding messages to its queues.

The throughput of ServiceControl is dependent on multiple factors. Messages size and network bandwidth have significant affect on throughput. Another factor is the transport type used by your system.

#### Transport type

Different transports provide different throughput capabilities.

The transports supported by ServiceControl out-of-the-box (i.e. MSMQ, RabbitMQ, SQL Server and Azure Queues and Azure Service Bus) provide varying throughput numbers, with MSMQ and SQL Server providing the highest throughput numbers.

Azure Queues and Service Bus throughput varies significantly based on deployment options and multiple related variables inherent to cloud deployment scenarios.

It is recommended that you plan and perform realistic throughput tests on ServiceControl using the transport of your choice and deployment options that are as close as possible to your planned production deployment. For additional questions or information please [contact Particular Software](http://particular.net/contactus).

#### ServiceControl in Practice

Often customers don't need all the features available in ServiceControl in one single environment. Its a tool that can accommodate many situations and sometimes some features get used in situations where their value proposition is not particularly strong.

For example, in your development environment you probably want a lot of logging information to support problem analysis. For many users the [Debug Session](/servicecontrol/plugins/debug-session.md) plugin is really useful at this stage of the application life-cycle but you would never put that plugin into production. For the same reason, you would never use the [Saga Audit](/servicecontrol/plugins/saga-audit.md) plugin outside of development.

Making decisions about the use of the other plugins and features requires a little more thought to balance the smooth running of your system with your actual requirements. The temptation to just use them all as a kind of insurance policy is not a good choice. The cost of having some of these features in play can actually cause issues down the track if you're not prepared.
 
[Auditing](/nservicebus/operations/auditing.md) is a good example of how the cost of having a record of everything may cause significant impact on the system. It might be worth considering turning audit off in some or all endpoints in production. Or if you absolutely need it move the auditing to another place like your Business Intelligence database or somewhere that has better resources dedicated to the problem.

It is technically possible to run two instances of ServiceControl one for audits and one for active monitoring and retries. Whether this makes sense for your situation is another discussion entirely.

[Heartbeats](/servicepulse/intro-endpoints-heartbeats.md) and [Custom Checks](/servicepulse/intro-endpoints-custom-checks.md) are great for knowing when an endpoint is up or down but they add extra noise to your system. Definitely think hard about what the business ramifications are of having each endpoint available. Often, not all endpoints are mission critical, the default heartbeat is forty seconds. Maybe ten second updates are right for some endpoints but two minute intervals could be better for others. Remember also that all communication from endpoints with ServiceControl is performed via messaging. Adding a message to the queue every second may have little impact on when the notification shows up in ServicePulse. Remember, the last heartbeat is really the only one you care about. If you stop ServiceControl those heartbeats are going to bank up in the queue. You might want to consider flushing the heartbeats from the queue before starting ServiceControl again. Having a lot of noisey messages that add no value may overwhelm ServiceControl and causing it fail. 

##### Sometimes slower is better.

Moving forward pace your adoption of plugins and features: 

- Turn off auditing on all endpoints as well as heartbeats and custom checks.
- If you are using the NServiceBus.Host set the log levels to ['production'](/nservicebus/hosting/nservicebus-host/profiles.md#logging-behaviors)
- Perform a load test to baseline your solution.
- When you are comfortable with the performance of the system try adding Heartbeats. That will allow you to monitor your system
- Try increasing the heartbeat interval to around a minute maybe even more. Ideally you will not want to have a heartbeat update more frequently than ServiceControl can process it or more that your Operations staff are prepared to look at ServicePulse.
- With each additional change perform your load test again adjusting the heartbeat interval until you get a satisfactory result.
- Do the same with Custom Checks if you have them.
- After all that I would consider adding audit back into the mix if it is really required and then test again.
