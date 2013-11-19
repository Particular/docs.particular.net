---
title: "Preparing your machine to run NServiceBus"
tags: ""
summary: ""
---

<style type="text/css">div#uls ul{
    margin-bottom: -15px;
  }
  ul.ulcheck {
    list-style-image: url("http://particular.blob.core.windows.net/media/Default/img/check.png");
  }
  ul.ulnotcheck {
    list-style-image: url("http://particular.blob.core.windows.net/media/Default/img/redx.png");
  }
</style>
<script>
    function getParameterByName(name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
        return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    }
    var installer = getParameterByName("installer");
    if (installer=="nservicebus")
    {
        installer="NServiceBus";
    }
    if (installer=="NServiceBus")
    {
        _gaq.push(['_trackEvent', 'Installed', 'NServiceBus Total [nuget]']);
        _gaq.push(['_trackEvent', 'Installed', 'NServiceBus Total [msi+nuget]']);
        var version = getParameterByName("version");
        var method = getParameterByName("method");
        var logaction = installer+" "+version+" [ "+method+" ]";
        _gaq.push(['_trackEvent', 'Installed', logaction]);
    }
</script>
<script type="text/javascript">
$(document).ready(function(){
    var lowerHref = window.location.href.toLowerCase()
    if (lowerHref.indexOf("dtc=true")>-1){
        $("#li_dtc").addClass("ulcheck");
        //$("#code_dtc").hide();
    }
    if (lowerHref.indexOf("dtc=false")>-1) {
        $("#li_dtc").addClass("ulnotcheck");
    }
    if (lowerHref.indexOf("msmq=true")>-1){
        $("#li_msmq").addClass("ulcheck");
        //$("#code_msmq").hide();
    }
    if (lowerHref.indexOf("msmq=false")>-1){
        $("#li_msmq").addClass("ulnotcheck");
    }
    if (lowerHref.indexOf("raven=true")>-1){
        $("#li_ravendb").addClass("ulcheck");
        //$("#code_ravendb").hide();
    }
    if (lowerHref.indexOf("raven=false")>-1){
        $("#li_ravendb").addClass("ulnotcheck");
        $("#ravendbport").show();
    }
    if (lowerHref.indexOf("perfcounter=true")>-1){
        $("#li_performance").addClass("ulcheck");
        //$("#code_performance").hide();
    }
    if (lowerHref.indexOf("perfcounter=false")>-1){
        $("#li_performance").addClass("ulnotcheck");
    }
});
</script>

NServiceBus relies on a few key infrastructure components in order to run properly.

<div id="uls">
-   <span style="font-size: 14px; line-height: 24px;">DTC</span>

-   <span style="font-size: 14px; line-height: 24px;">MSMQ</span>

-   RavenDB <span id="ravendbport" style="display:none">(Note: only port
    8080 is being scanned to see if RavenDB is installed.)</span>

-   Performance Counters


When installing NServicebus, the installation process verifies that these components are installed as required. If any of the components is not installed, the installation process will install the missing component.

<span style="font-size: 14px; line-height: 24px;">To manually install these components (or make sure they are installed properly), type the following commands into Package Manager Console inside Visual Studio
</span> <span style="font-size: 14px; line-height: 24px;"> </span>
<span style="font-size: 14px; line-height: 24px;">(Package Manager Console can be found under the menu View, Other Windows)</span>

**For NServiceBus version 4.0 or later:**

<div class="nuget-badge" style="color: rgb(0, 0, 0); font-family: Calibri; font-size: medium; line-height: normal; background-color: rgb(240, 240, 240);">
`PM> Install-NServiceBusDtc  PM> Install-NServiceBusMsmq PM> Install-NServiceBusRavenDB PM> Install-NServiceBusPerformanceCounters`{style="background-color: rgb(32, 32, 32); border: 4px solid rgb(192, 192, 192); border-top-left-radius: 5px; border-top-right-radius: 5px; border-bottom-right-radius: 5px; border-bottom-left-radius: 5px; box-shadow: rgb(110, 110, 110) 2px 2px 3px; color: rgb(226, 226, 226); display: block; font-size: 1.2em; font-family: 'andale mono', 'lucida console', monospace; line-height: 1.2em; overflow: auto; padding: 1px;"}


<span style="font-weight: 600;">For NServiceBus version 3.x:</span>

<div class="nuget-badge" style="color: rgb(0, 0, 0); font-family: Calibri; font-size: medium; line-height: normal; background-color: rgb(240, 240, 240);">
`PM> Install-Dtc PM> Install-Msmq PM> Install-RavenDB PM> Install-PerformanceCounters`{style="background-color: rgb(32, 32, 32); border: 4px solid rgb(192, 192, 192); border-top-left-radius: 5px; border-top-right-radius: 5px; border-bottom-right-radius: 5px; border-bottom-left-radius: 5px; box-shadow: rgb(110, 110, 110) 2px 2px 3px; color: rgb(226, 226, 226); display: block; font-size: 1.2em; font-family: 'andale mono', 'lucida console', monospace; line-height: 1.2em; overflow: auto; padding: 1px;"}




The Windows operating system requires that elevated privileges to install and configure some of these infrastructure components. Elevated privileges require that you run these command from a process created with [administrator rights](http://windows.microsoft.com/en-us/windows7/how-do-i-run-an-application-once-with-a-full-administrator-access-token)
.

For more information on poweshell command line changes in NServiceBus version 4, see [release notes](/blog/nservicebus-v4.0-release-notes#powershell) .

RavenDB installation is checked by examining port 8080 and port 8081 on the installation local machine. If RavenDB is installed on any other port it needs to be configured and upgraded manually. For more information on configuring RavenDB see [Installing RavenDB for NServiceBus](using-ravendb-in-nservicebus-installing.md) and [Connecting to RavenDB from NServiceBus](using-ravendb-in-nservicebus-connecting.md)



