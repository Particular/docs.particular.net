<!--
title: "DefiningMessagesAs and DefiningEventsAs When Starting Endpoint"
tags: ""
summary: "<p><span style="font-size: 14px; line-height: 24px;">When defining an endpoint with the following declaration (in bold):</span></p>
<pre><code>class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization
{
public void Init()
{
Configure.With()
.DefaultBuilder()
.DefiningEventsAs(t =&gt; t.Namespace != null &amp;&amp; t.Namespace.StartsWith(&quot;MyMessages&quot;))
.DefiningMessagesAs(t =&gt; t.Namespace != null &amp;&amp; t.Namespace.EndsWith(&quot;Messages&quot;));
}
}
</code></pre>

"
-->

<span style="font-size: 14px; line-height: 24px;">When defining an endpoint with the following declaration (in bold):</span>

    class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization
    {
    public void Init()
    {
    Configure.With()
    .DefaultBuilder()
    .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("MyMessages"))
    .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.EndsWith("Messages"));
    }
    }

a FATAL NServiceBus.Hosting.GenericHost exception is thrown: "Exception when starting endpoint.".

The reason is that NServiceBus itself uses namespaces that end with messages.

To fix, include your default namespace; for example:

    class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization
    {
    public void Init()
    {
    Configure.With()
    .DefaultBuilder()
    .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("MyMessages"))
    .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.BeginsWith("MyCompany") && t.Namespace.EndsWith("Messages"));
    }
    }

