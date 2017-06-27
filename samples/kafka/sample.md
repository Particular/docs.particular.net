---
title: Kafka Transport Usage
summary: A simple send and receive scenario with the Kafka Transport
reviewed: 2017-01-16
component: Kafka
related:
- transports/kafka
---


## Prerequisites

Ensure an instance of Kafka is installed and accessible in '127.0.0.1:9092'.

Useful links:

 * [Setting Up and Running Apache Kafka on Windows OS](https://dzone.com/articles/running-apache-kafka-on-windows-os)
 * [Kafka Tool: GUI for managing and using Kafka clusters](http://www.kafkatool.com/)


## Running the project

 1. Start both the Sender and Receiver projects.
 1. At startup Sender will send a message to Receiver.
 1. Receiver will handle the message.


## Code Walk-through


### Configure the Kafka

snippet: TransportConfiguration
