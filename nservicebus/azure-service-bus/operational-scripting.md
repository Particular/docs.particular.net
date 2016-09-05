---
title: Azure Service Bus Transport Operational Scripting
summary: Examples and scripts that facilitate deployment and operational actions against Azure Service Bus
component: ASB
reviewed: 2016-09-05
tags:
- Azure
- Cloud
- Scripting
---

## Azure Service Bus : Scripting

This document refers to example code and scripts that facilitate deployment and operational actions against an Azure Service Bus namespace.

An [nservicebus specific sample](/samples/azure/native-integration-asb/) is available that shows how to send and receive messages from using the [Azure Service Bus NuGet Package](https://www.nuget.org/packages/WindowsAzure.ServiceBus/).

Besides the .NET package, this SDK is also available for Java, Node.Js, PHP, Python and Ruby. For each of the languages, the MSDN Documentation offers a page that shows how to leverage the respective SDK to manage entities and send/receive messages.

 * [.NET](https://azure.microsoft.com/en-us/documentation/articles/service-bus-dotnet-get-started-with-queues/)
 * [Java](https://azure.microsoft.com/en-us/documentation/articles/service-bus-java-how-to-use-queues/)
 * [Node.js](https://azure.microsoft.com/en-us/documentation/articles/service-bus-nodejs-how-to-use-queues/)
 * [PHP](https://azure.microsoft.com/en-us/documentation/articles/service-bus-php-how-to-use-queues/)
 * [Python](https://azure.microsoft.com/en-us/documentation/articles/service-bus-python-how-to-use-queues/)
 * [Ruby](https://azure.microsoft.com/en-us/documentation/articles/service-bus-ruby-how-to-use-queues/)

In case an application uses another programming language, it is possible to perform all of these actions using the [REST API](https://msdn.microsoft.com/library/hh780717.aspx) as well.

If operations need to be executed at the command line, refer to the [Powershell Samples](https://azure.microsoft.com/en-us/documentation/articles/service-bus-powershell-how-to-provision/) or the cross platform [Azure XPLAT CLI](https://www.npmjs.com/package/azure-cli) for more information.