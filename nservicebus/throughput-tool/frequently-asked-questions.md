---
title: Frequently Asked Questions
summary: Answers to frequently asked questions related to the EndpointThroughputCounter
reviewed: 2023-01-25
related:
  - nservicebus/throughput-tool
---
**Q: What does the EndpointThroughputCounter do?**

**A:** The tool attempts to query the appropriate api endpoint or database queue tables in order to measure the number of endpoints in use and the max total daily throughput of all endpoints. After gathering this information it outputs a report to a local file in the directory where the EndpointThroughputCounter was ran. The tool does not transmit any data to Particular Software or any other party, the data collected is only used to generate the local report.


**Q: Why should I run the EndpointThroughputCounter?**

**A:** The tool is provided to assist customers in accurately gathering information needed to answer the following annual licensing questions. 
    1. How many logical endpoints are running in the production environment? 
    1. What is the maximum total daily message throughput of all the endpoints in the production environment? 

**Q: Does the EndpointThroughputCounter need to run on my server?**

**A:** In most cases No. Depending on the option chosen the tool will need to access an api or database to gather throughput metrics. The api must be accessible to the host machine that the tool is run on. In most sceanrios a developer's computer will be appropriate. 

**Q: What should I do if I have problems running the EndpointThroughputCounter?**

**A:** Email the account manager, or open a [non-critical support case](https://particular.net/support)

**Q: How will the EndpointThroughputCounter affect my system?**

**A:** In all cases the impact to the system should be insignificant and likely unnoticable. When using Azure Service Bus, RabbitMQ, or ServiceControl, monitoring API endpoints are queried only a few times during the running of the application. Similarly in the case of Sql a few simple queries are run with no lock two times during the entire execution of the tool. 

**Q: Why does the tool report that it will run for 24 hours?**

**A** In certain instances preaggregarted metrics for a 24 hour periods do not exist from the metrics source. In these instances the tool will run a short query process once on startup, and once after 24 hours has elapsed.Then it will measure the throughput over the time, and complete. 

**Q: Why do I get the error "The certificate chain was issued by an authority that is not trusted"?**

In recent versions of Microsoft's Sql Server drivers encryption has been enabled by default. When trying to connect to a Sql Server instance that uses a self-signed cerftificate, the tool may display an exception stating The certificate chain was issued by an authority that is not trusted. To bypass this exception update the connection string to include ;Trust Server Certificate=true.

**Q: How does the EndpointThroughputCounter measure throughput?**

**A:** That depends on which transport is being measureed. The EndpointThroughputCounter can measure throughput for the following transports.

**AzureServiceBus** The tool querires AzureServiceBus to get the queue names. Then for each queue it queries the [Azure Monitor Metrics endpoint](https://learn.microsoft.com/en-us/rest/api/monitor/metrics/list?tabs=HTTP) to determine the maximum daily throughput in the previous 30 days for each queue. 

**RabbitMQ** The tool querires the [Management API](https://www.rabbitmq.com/management.html#http-api) to get a list of queues, Then, for each queue, it queries the [RabbitMQ monitoring queue metrics endpoint](https://www.rabbitmq.com/monitoring.html#queue-metrics) to retrieve the queue throughput at the beginning of the run, and then after 24 hours the process is repeated. Finally it measures the throughput by calculating the difference in throughput counts between the first and second run for each queue. 

**SQS** The tool queries AWS using the [ListQueues api](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/APIReference/API_ListQueues.html) to find all the queue names. Then it queries each queue using the [AWS Cloudwatch GetMetricStatistics Api](https://docs.aws.amazon.com/AmazonCloudWatch/latest/APIReference/API_GetMetricStatistics.html) to gather daily throughput measurements for each queue.

**Sql Server** The tool queries Sql Server for queue tables, and gathers the throughput for each table. This process happens at once when first executing the tool then again after 24 hours. Measurements are then made based on the difference in throughput between the first and second process of gathering throughput.  

**Q: May I see the source code for the EndpointThroughputCounter?**

**A:** Yes, the source is currently in a public [repository on GitHub](https://github.com/Particular/Particular.EndpointThroughputCounter). 