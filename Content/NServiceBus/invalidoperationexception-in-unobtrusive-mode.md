<!--
title: "InvalidOperationException in Unobtrusive Mode"
tags: ""
summary: "<p>To avoid an InvalidOperationException, .DefiningMessagesAs (or any one of the DefiningXXXAs) should come in the Fluent configuration before
.UnicastBus(). </p>
<p>You can read more about the <a href="unobtrusive-mode-messages.md">Unobtrusive mode</a> or see the <a href="unobtrusive-sample.md">Unobtrusive Sample</a> .</p>
"
-->

To avoid an InvalidOperationException, .DefiningMessagesAs (or any one of the DefiningXXXAs) should come in the Fluent configuration before
.UnicastBus(). 

You can read more about the [Unobtrusive mode](unobtrusive-mode-messages.md) or see the [Unobtrusive Sample](unobtrusive-sample.md) .

See how the server side in the unobtrusive sample might look when self hosting (instead of being hosted in NServiceBus Host):

<script src="https://gist.github.com/Particular/6059976.js?file=Server.cs"></script>


