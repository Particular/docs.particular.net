<!--
title: "How To Debug RavenDB Through Fiddler Using NServiceBus"
tags: 
-->

To set up an NServiceBus endpoint to make all calls to RavenDB through Fiddler, configure the proxy for your endpoint by adding the following code to app.config:







With the proxy setup, change the RavenDB connection string to go through Fiddler by adding this:





