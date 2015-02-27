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

#region HostIdFixer
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
        Guid hostId = CreateGuid(Environment.MachineName, settings.EndpointName());
        string location = Assembly.GetExecutingAssembly().Location;
        Dictionary<string, string> properties = new Dictionary<string, string>
                                {
                                    {"Location",location}
                                };
        bus.HostInformation = new HostInformation(hostId, Environment.MachineName, properties);
    }

    static Guid CreateGuid(params string[] data)
    {
        using (MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider())
        {
            byte[] inputBytes = Encoding.Default.GetBytes(String.Concat(data));
            byte[] hashBytes = provider.ComputeHash(inputBytes);
            return new Guid(hashBytes);
        }
    }

    public void Stop()
    {
    }
}
#endregion
