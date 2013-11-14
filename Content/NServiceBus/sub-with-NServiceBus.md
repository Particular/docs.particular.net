<!--
title: "How to Publish/Subscribe to a Message"
tags: ""
summary: "<h2>How to publish a message?</h2>
<p><script src="https://gist.github.com/Particular/6059857.js?file=Bus.Publish.cs"></script> OR instantiate and publish all at once:</p>
"
-->

How to publish a message?
-------------------------

<script src="https://gist.github.com/Particular/6059857.js?file=Bus.Publish.cs"></script> OR instantiate and publish all at once:

<script src="https://gist.github.com/Particular/6059857.js?file=IMyMessage.cs"></script> How to subscribe to a message?
------------------------------

To manually subscribe and unsubscribe from a message:

<script src="https://gist.github.com/Particular/6059857.js?file=SubUnsub.cs"></script> To subscribe to a message, you must have a UnicastBusConfig entry, as follows:

<script src="https://gist.github.com/Particular/6059857.js?file=UnicastBusConfig.xml"></script> When subscribing to a message, you will probably have a [message handler](how-do-i-handle-a-message.md) for it. If you do, and have the UnicastBusConfig section mentioned above, you do not have to write Bus.Subscribe, as NServiceBus invokes it automatically for you.

You can also choose to **not** have the infrastructure automatically subscribe by calling .DoNotAutoSubscribe() after .UnicastBus() in the Fluent configuration API.

