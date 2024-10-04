using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using NServiceBus;

public static class GetNServiceBusInfo
{
    public static IHtmlContent OutputNServiceBusInfo(this IHtmlHelper<dynamic> _)
    {
        Assembly nsbAssembly = typeof(IEndpointInstance).Assembly;
        AssemblyFileVersionAttribute att = nsbAssembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute)).OfType<AssemblyFileVersionAttribute>().First();
        var v = new Version(att.Version);
        string script = $"<script>window.NSB_VERSION = '{v.Major}.{v.Minor}.{v.Build}';</script>";
        return new HtmlString(script);
    }
}