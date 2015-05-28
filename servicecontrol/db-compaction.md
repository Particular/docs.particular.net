---
title: Compacting the ServiceControl RavenDB database
summary: How to compact (release disk space to OS) the RavenDB database backing the ServiceControl
tags:
- ServiceControl
- RavenDB
---

## Overview

ServiceControl 1.4 introduced a database maintenance feature which allows ServiceControl to be run with all features except for RavenDB Studio disabled.   In this mode the following procedure can be used to compact the embedded RavenDB database.

## Step 1: Start ServiceControl in the maintenance mode

- Stop the ServiceControl Windows Service
- Open command line and go to ServiceControl binary folder
- Type `ServiceControl -maint`
- ServiceControl will start in the maintenance mode  with RavenDB studio exposed on `http://localhost:{selected port}/storage`.  Assuming a default installation this URL would be `http://localhost:33333/storage`

## Step 2: Export the current database

- Open a browser and navigate to `http://localhost:33333/storage`
- Export the existing ServiceControl database. 

![](export-database-step1.png)

- Click Ok

![](export-database-step2.png)

- Select the directory where you want to store the exported data file.

![](export-database-step3.png)

- Wait for the export operation to complete.

![](export-database-step4.png)

## Step 3: Delete the existing database

- Once the export operation is complete, stop ServiceControl (press `<enter>` in the console).
- Delete the ServiceControl data file (localhost-33333) located at `C:\ProgramData\Particular\ServiceControl`
- Start ServiceControl, again in the maintenance mode.

## Step 4: Import the backed up data

- Go to the RavenDB studio `http://localhost:33333/storage` and perform Import steps.
- Select the `Tasks` tab and select all the checkboxes
![](import-database-step1.png)

- Click Ok to proceed.

![](import-database-step2.png)

- Select the file where the exported data was stored.

![](import-database-step3.png)

- Wait for the operation to complete.

![](import-database-step4.png)

- Stop ServiceControl (press `<enter>` in the console).

## Step 5: Restart ServiceControl

- Start the ServiceControl WindowsService.