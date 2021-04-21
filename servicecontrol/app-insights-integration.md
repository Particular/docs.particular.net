---
title: Connect to Application Insights Azure Monitor
summary: How to use Application Insights From A Handler
component: ServiceControl
reviewed: 2021-04-21
related:
 - samples/servicecontrol/azure-monitor-events
 - samples/logging/application-insights
 - samples/tracing
---

## Connect to Application Insights Azure Monitor

To use Application Insights, the instrumentation key must be provided. 

By default the sample code loads this key from an environment variable called ApplicationInsightKey. Either set this environment variable or paste the instrumentation key in the following section.

The instrumentation key can be retrieved from the Azure Portal by locating the Application Insights instance, and then navigating to the Properties view.

snippet: AppInsightsSdkSetup

The handler below creates a custom telemetry event and pushes it to Application Insights.

snippet: AzureMonitorConnectorEventsHandler
