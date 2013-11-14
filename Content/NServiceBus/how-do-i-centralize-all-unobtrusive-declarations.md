<!--
title: "How to Centralize All Unobtrusive Declarations?"
tags: ""
summary: "<p>When working with NServiceBus in unobtrusive mode you may feel that you are repeating the conventions over and over again on all the endpoints.</p>
"
-->

When working with NServiceBus in unobtrusive mode you may feel that you are repeating the conventions over and over again on all the endpoints.


The
[IWantToRunBeforeConfiguration](https://github.com/NServiceBus/NServiceBus/blob/develop/src/NServiceBus.Core/IWantToRunBeforeConfiguration.cs) interface is a great help when embracing the DRY (don't repeat yourself) principle. 

Just define your implementation in an assembly referenced by all the endpoints:

<script src="https://gist.github.com/Particular/6092089.js"></script>


