---
title: Compacting the ServiceControl RavenDB database
summary: How to compact (release disk space to OS) the RavenDB database backing the ServiceControl
tags:
- ServiceControl
- RavenDB
---

## Overview

ServiceControl 1.4 introduced a database maintenance feature which allows ServiceControl to be run with all features except for RavenDB Studio disabled. While in this mode no messages are ingested from the queuing system.

Once ServiceControl is running in this mode the following procedure can be used to compact the embedded RavenDB database.


## Step 1: Start ServiceControl in the maintenance mode

- Open the ServiceControl Management utility
- Stop the Service from the actions icons
- Note down:
	- the installation path for the service.
	- the database path for the service.
- Open command line, as Administrator, at installation path for the service
- Type `ServiceControl -maint`
- ServiceControl will start in the maintenance mode with RavenDB studio exposed on `http://localhost:{selected port}/storage`.


## Step 2: Export the current database

- Open a browser and navigate to `http://localhost:{selected port}/storage`
- Export the existing ServiceControl database.

![](export-database-step1.png)

- Click Ok

![](export-database-step2.png)

- Select the directory where you want to store the exported data file.

![](export-database-step3.png)

- Wait for the export operation to complete.

![](export-database-step4.png)

- Once the export operation is complete, stop ServiceControl (press `<enter>` in the console).


## Step 3: Delete the existing database

NOTE: At this point it is advisable to take a backup copy of the existing database folder as re-importing can fail. To do this ensure that ServiceControl is not running and the copy the contents of the database directory.

- Delete the database directory contents.
- Start ServiceControl, again in the maintenance mode. This will populate the database folder with a blank database.


## Step 4: Import the exported data

- Go to the RavenDB studio `http://localhost:{selected port}/storage` and perform Import steps.
- Select the `Tasks` tab and select all the checkboxs
![](import-database-step1.png)

- Click OK to proceed.

![](import-database-step2.png)

- Select the file where the exported data was stored.

![](import-database-step3.png)

- Wait for the operation to complete.

- After the operation has completed wait for the stale index count in the footer to indicate there are no stale indexes. 

![](import-database-step4.png)

- Stop ServiceControl (press `<enter>` in the console).

NOTE: If you get an `System.OutOfMemoryException` during import you can work around this error by reducing the batch size in advanced settings.

![](import-database-note.png)

## Step 5: Restart ServiceControl

- Start the ServiceControl Windows Service.
