<!--
title: "In a Distributor Scenario, What Happens to the Message if a Worker Goes Down?"
tags: ""
summary: "<p>The master forwards the message to the worker and relies on the store and forward messaging provided by MSMQ to make sure that it gets there. The message is processed when the worker returns to working order.</p>
<p>The master keeps the messages in its queue until an available worker checks in for a unit of work (each thread in each worker is a separate
&quot;check in&quot;). So, in essence, the workers pull messages from the master.</p>
"
-->

The master forwards the message to the worker and relies on the store and forward messaging provided by MSMQ to make sure that it gets there. The message is processed when the worker returns to working order.

The master keeps the messages in its queue until an available worker checks in for a unit of work (each thread in each worker is a separate
"check in"). So, in essence, the workers pull messages from the master.

The solution to a "broken worker" or a "worker node’s hard disk going up in smoke" is virtualization, where worker nodes run in a VM whose image is on a SAN somewhere.

Any messages stored locally in the worker node end up physically stored in the image on the SAN.

The hypervisor then brings up the VM that went down on a different machine and all the messages are then processed.

