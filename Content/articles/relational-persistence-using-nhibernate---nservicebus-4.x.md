<!--
title: "Relational Persistence Using NHibernate - NServiceBus 4.x"
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


Subscriptions, Sagas, Timeouts and Gateway persistance
------------------------------------------------------

<p> To use Subscriptions, Sagas, Timeouts and Gateway persistance using NHibernate, use this configuration:


<script src="https://gist.github.com/Particular-gist/6341414.js?file=007RelationalPersistenceUsingNHibernate_4x.cs"></script>
</p> NServiceBus then picks up the connection setting from your app.config. Here is an example

<p> NOTE: When using SQL 2012 you need to change the dialect to MsSql2012Dialect.

NOTE: Additional dialects are available in the NHibernate.Dialect namespace, [NHibernate documentation.](http://nhforge.org/doc/nh/en/index.html#configuration-xmlconfig)


<script src="https://gist.github.com/Particular-gist/6341414.js?file=008RelationalPersistenceUsingNHibernate_4x.xml"></script>
</p>


