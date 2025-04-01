﻿using OpenTelemetry;
using System.Diagnostics;
using System.Net;

#region custom-processor

class NetHostProcessor : BaseProcessor<Activity>
{
    readonly string hostName = Dns.GetHostName();

    public override void OnStart(Activity data)
    {
        data.SetTag("net.host.name", hostName);
    }
}

#endregion