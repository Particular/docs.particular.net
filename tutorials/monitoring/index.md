---
title: Monitoring NServiceBus solutions
reviewed: 2017-10-10
summary: Getting started with the monitoring capabilities built in to the Particular Service Platform.
suppressRelated: true
---

include: monitoring-intro-paragraph

downloadbutton


### Using the sample solution

The endpoints in the sample solution communicate using the SQL Transport. Each endpoint will create it's own tables the first time it is run. It uses these tables like queues to store incoming messages.

Before running the solution the first time, you need to create the database and schema that all endpoints will use. Included in the solution download is a set-up script.

TODO: Details on how to run the setup script


## Overview

- **[Lesson 1: Component overview](1-component-overview/)** - learn about all of the monitoring components in the Particular Service Platform.

- **[Lesson 2: Setting up monitoring environment](2-setting-up-environment/)**  - learn how set up all of the server side monitoring components of the Particular Service Platform.

- **[Lesson 3: Configuring endpoints to send data to ServiceControl](3-configuring-endpoints/)** - learn how to configure endpoints to send monitoring data to the Particular Service Platform.

- **Lesson 4: Throughput and processing time** - learn how measure individual endpoint performance by examining message throughput and processing time.

- **Lesson 5: Queue length and critical time** - learn how to measure inter-endpoint performance by examining queue length and critical time.

- **Lesson 6: Failure rates** - learn how to detect hidden problems in your solution by watching failure rates.

**Go to [**Lesson 1: Component overview**](1-component-overview/) to begin.**
