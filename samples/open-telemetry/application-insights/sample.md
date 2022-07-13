---
title: Tracing in Application Insights
summary: How to configure an endpoint to export OpenTelemetry traces to Application Insights
reviewed: 2022-07-13
component: Core
related:
---

## Introduction

This sample shows how to capture NServiceBus OpenTelemtry traces and export them to Application Insights in Azure.

## Prerequisites

This sample requires an Application Insights connection string.

## Running the sample

1. Create an Application Insights resource in Azure
2. Copy the connection string from the Azure portal dashboard into the sample
3. Start the sample endpoint
4. Press any key to send a message, or ESC to quit
5. On the Aure portal dashboard, open the Monitoring -> Logs panel
6. Run the query `requests` to see a list of the requests. Review the `customDimensions` attribute to see NServiceBus headers

## Code walk-through
