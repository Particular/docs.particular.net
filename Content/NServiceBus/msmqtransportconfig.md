<!--
title: "MsmqTransportConfig"
tags: ""
summary: "<p>The configuration section defines properties of the MSMQ transport. See background on <a href="msmq-information.md">MSMQ</a> .</p>
<p>Example of MsmqTransportConfig:</p>
"
-->

The configuration section defines properties of the MSMQ transport. See background on [MSMQ](msmq-information.md) .

Example of MsmqTransportConfig:


```XML
<MessageForwardingInCaseOfFaultConfig ErrorQueue="error"/>
```

 ErrorQueue
----------

Beginning with NServiceBus V3, use the configuration section to declare an error queue:


```XML
<section name="MessageForwardingInCaseOfFaultConfig" 
 type="NServiceBus.Config.MessageForwardingInCaseOfFaultConfig, NServiceBus.Core" />
```

 To define the value:


```XML
<MsmqTransportConfig ErrorQueue="error" NumberOfWorkerThreads="1" MaxRetries="5"/>
```

 The ErrorQueue in MsmqTransportConfig is for compatibility with earlier versions.

The ErrorQueue defines the name of the queue to which messages are transferred if they cannot be processed successfully. This may be a queue on the local machine—or on a remote machine, in which case the value should be based on the template "queueName@remoteMachineName" where "queueName" is the name of the error queue (often "error") and
"remoteMachineName" is the name of the remote machine on which the error queue resides.

If no error queue is defined, NServiceBus fails to start with the exception: "Could not find backup configuration section
'MsmqTransportConfig' in order to locate the error queue."

Read more about [messages whose processing fails](how-do-i-handle-exceptions.md) .

NumberOfWorkerThreads
---------------------

This property dictates the number of threads that receive messages from the input queue. This property has no impact on the number of threads that can use the bus to send or publish messages.

MaxRetries
----------

This property is related to the ErrorQueue property, defining the number of times to retry a message whose processing fails before it is moved to the error queue.

Default value: 5.

Changes to MsmqTransportConfig in V4.0
--------------------------------------

The MsmqTransportConfig configuration section became obsolete in V4.0. Use the TransportConfig section instead:


```XML
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="MessageForwardingInCaseOfFaultConfig" type="NServiceBus.Config.MessageForwardingInCaseOfFaultConfig, NServiceBus.Core" />
    <section name="TransportConfig" type="NServiceBus.Config.TransportConfig, NServiceBus.Core"/>
  </configSections>

  <MessageForwardingInCaseOfFaultConfig ErrorQueue="error"/>
  <TransportConfig MaximumConcurrencyLevel="5" MaxRetries="2" MaximumMessageThroughputPerSecond="0"/>
 
</configuration>
```


**MaximumConcurrencyLevel** - This is the same as the NumberOfWorkerThreads property in MsmqTransportConfig.

**MaximumMessageThroughputPerSecond -** This sets a limit on how quickly messages can be processed between all threads. Use a value of 0 to have no throughput limit. 



