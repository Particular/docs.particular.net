<!--
title: "Relational Persistence Using NHibernate"
tags: ""
summary: "<p>If you require that your data persist in a relational database, NServiceBus provides a separate assembly with added support for NHibernate-based storage:</p>
<ul>
<li>
If you downloaded NServiceBus from this site (rather than via NuGet)
you have to add a reference to NServiceBus.NHibernate.dll (found in
the binaries folder). You also need to download and reference
version 3.3.0.4000 of NHibernate.
</li>
<li>
If you used NuGet, you only need to install NServiceBus.NHibernate,
like this:
</li>
</ul>
"
-->

If you require that your data persist in a relational database, NServiceBus provides a separate assembly with added support for NHibernate-based storage:

-   If you downloaded NServiceBus from this site (rather than via NuGet)
    you have to add a reference to NServiceBus.NHibernate.dll (found in
    the binaries folder). You also need to download and reference
    version 3.3.0.4000 of NHibernate.
-   If you used NuGet, you only need to install NServiceBus.NHibernate,
    like this:


~~~~ {.brush:csharp; style="margin-left: 40px;"} PM> Install-Package NServiceBus.NHibernate

This automatically sets up all the dependencies and is the recommended way of using NHibernate support.


Subscriptions
-------------

To store subscriptions using NHibernate, use this configuration:

<p>



<script src="https://gist.github.com/Particular-gist/6341414.js?file=001RelationalPersistenceUsingNHibernate_338.cs"></script>
</p> NServiceBus then picks up the connection setting from your app.config. Here is an example (using SqlLite):

<p>



<script src="https://gist.github.com/Particular-gist/6341414.js?file=002RelationalPersistenceUsingNHibernate_338.xml"></script>
</p> Read about the [available properties](http://nhforge.org/doc/nh/en/index.html#configuration-xmlconfig)
.

Sagas
-----

To store sagas using NHibernate, use this configuration:

<p>



<script src="https://gist.github.com/Particular-gist/6341414.js?file=003RelationalPersistenceUsingNHibernate_saga_338.cs"></script>
</p> Example configuration:

<p>



<script src="https://gist.github.com/Particular-gist/6341414.js?file=004RelationalPersistenceUsingNHibernate_saga_338.xml"></script>
</p> Timeouts
--------

For the timeout manager to store timeouts using NHibernate, use this configuration (SqlServer2008 in this case). This is valid from V3.2.3 onwards.

<p>



<script src="https://gist.github.com/Particular-gist/6341414.js?file=005RelationalPersistenceUsingNHibernate_to_338.cs"></script>



<script src="https://gist.github.com/Particular-gist/6341414.js?file=006RelationalPersistenceUsingNHibernate_to_338.xml"></script>
</p>


