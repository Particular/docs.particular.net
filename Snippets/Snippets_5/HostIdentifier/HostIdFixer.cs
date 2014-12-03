using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using NServiceBus;
using NServiceBus.Hosting;
using NServiceBus.Settings;
using NServiceBus.Unicast;
#pragma warning disable 618

#region HostIdFixer 5
public class HostIdFixer : IWantToRunWhenBusStartsAndStops
{
    UnicastBus bus;
    ReadOnlySettings settings;

    public HostIdFixer(UnicastBus bus, ReadOnlySettings settings)
    {
        this.bus = bus;
        this.settings = settings;
    }

    public void Start()
    {
        var hostId = CreateGuid(Environment.MachineName, settings.EndpointName());
        var location = Assembly.GetExecutingAssembly().Location;
        var properties = new Dictionary<string, string>
                                {
                                    {"Location",location}
                                };
        bus.HostInformation = new HostInformation(hostId, Environment.MachineName, properties);
    }

    static Guid CreateGuid(params string[] data)
    {
        using (var provider = new MD5CryptoServiceProvider())
        {
            var inputBytes = Encoding.Default.GetBytes(String.Concat(data));
            var hashBytes = provider.ComputeHash(inputBytes);
            return new Guid(hashBytes);
        }
    }

    public void Stop()
    {
    }
}
#endregion
