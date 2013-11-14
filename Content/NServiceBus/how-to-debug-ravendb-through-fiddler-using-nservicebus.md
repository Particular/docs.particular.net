<!--
title: "How To Debug RavenDB Through Fiddler Using NServiceBus"
tags: ""
summary: "<p>To set up an NServiceBus endpoint to make all calls to RavenDB through Fiddler, configure the proxy for your endpoint by adding the following code to app.config:</p>
<p><script src="https://gist.github.com/Particular/6059949.js?file=defaultProxy.xml"></script> With the proxy setup, change the RavenDB connection string to go through Fiddler by adding this:</p>
"
-->

To set up an NServiceBus endpoint to make all calls to RavenDB through Fiddler, configure the proxy for your endpoint by adding the following code to app.config:

<script src="https://gist.github.com/Particular/6059949.js?file=defaultProxy.xml"></script> With the proxy setup, change the RavenDB connection string to go through Fiddler by adding this:

<script src="https://gist.github.com/Particular/6059949.js?file=connectionStrings.xml"></script>


