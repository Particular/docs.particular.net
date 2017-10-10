---
title: "Monitoring NServiceBus solutions: Sample solution"
reviewed: 2017-10-10
summary: A sample solution that you can run to demo the monitoring features of the Particular Service Platform.
extensions:
- !!tutorial
  nextText: "Next Lesson: Component overview"
  nextUrl: tutorials/monitoring/1-component-overview
---

include: monitoring-intro-paragraph

downloadbutton


### Using the sample solution

The endpoints in the sample solution communicate using the SQL Transport. Each endpoint will create it's own tables the first time it is run. It uses these tables like queues to store incoming messages.

Before running the solution the first time, you need to create the database and schema that all endpoints will use. Included in the solution download is a set-up script.

TODO: Details on how to run the setup script