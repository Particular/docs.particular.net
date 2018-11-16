using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using NServiceBus;

public static class GetNServiceBusInfo
{
    public static MvcHtmlString OutputNServiceBusInfo(this HtmlHelper _)
    {
        var nsbAssembly = typeof(IEndpointInstance).Assembly;
        var att = nsbAssembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute)).OfType<AssemblyFileVersionAttribute>().First();
        var v = new Version(att.Version);
        var script = $"<script>window.NSB_VERSION = '{v.Major}.{v.Minor}.{v.Build}';</script>";
        return new MvcHtmlString(script);
    }
}