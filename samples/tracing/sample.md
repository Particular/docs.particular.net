---
title: Distributed tracing with Azure Monitor Application Insights
summary: How to extend NServiceBus pipeline and send trace information to Azure Monitor Application Insights
component: Core
reviewed: 2020-09-04
related:
 - monitoring/metrics
---

## Introduction

.Net Core 3.1 introduces support for distributed tracing. Dedicated types in the [System.Diagnostics](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics?view=dotnet-plat-ext-3.1) namespace enable exposing and correlating execution information in a distributed system. This sample shows how to extend NServiceBus pipeline with custom behaviors that publish tracing information and how to push that data to [Azure Application Insight](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview) for further processing and visualization. 

## Running the sample 

Running the sample requires Application Insights resource. [Instrumentation Key](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-resources-app-insights-keys?view=azure-bot-service-4.0#instrumentation-key) has to be provided by setting `ApplicationInsightKey` environment variable.

After starting, the sample requires pressing <kbr>Enter</kbr> to generate `InitialCommand` that when handled results in `FollowUpEvent` publication. Trace of each execution is pushed to Application Insights and available as [end-to-end transaction](https://docs.microsoft.com/en-us/azure/azure-monitor/app/transaction-diagnostics#transaction-diagnostics-experience) trace.

NOTE: It may take [a couple of minutes](https://github.com/MicrosoftDocs/azure-docs/issues/14183) before the data is available on the dashboard

![NServiceBus exectuion trace](sample-trace.png "Sample execution trace")

## Code overview

### Pipeline behaviors

Tracing information gets exposed by custom pipeline behaviors. Both behaviors define a dedicated [DiagnosticSource](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.diagnosticsource?view=netcore-3.1).

snippet: DiagnosticSourceDefinition

When a behavior is executed the logic checks if there is any listener subscribed to the source and if that's the case an instance of [Activity](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.activity?view=netcore-3.1) class gets created and signaled through the diagnostic source.

snippet: ActivityCreation

Newly created activity will inherit any existing tracing context (stored in `Activity.Current`) making sure that `ParentId` property gets initialized and the new activity will be linked to the parent one. Message headers are stored in the `Tags` collection which enables passing them to the 3rd party system -  Application Insights in this case.

Finally, in the outgoing behavior, the activity id gets stored in the outgoing message header to make sure that the recipient can link it's activities to the message sender.

### Application Inisights Integration

Tracing information produced by the behaviors is consumed by tracing listeners registered in `NServiceBusInsightsModule`:

snippet: TracingListenerRegistration

When an activity is signaled from the pipeline a listener gets notified via `OnNext` method. The activity is pushed to the Application Insights via `TelemetryClient` API: 

snippet: ActivityProcessing
