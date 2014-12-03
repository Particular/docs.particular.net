using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using NServiceBus;
using NServiceBus.Hosting;
using NServiceBus.Unicast;

#region HostIdFixer 4
public class HostIdFixer : IWantToRunWhenBusStartsAndStops
{
    UnicastBus bus;

    public HostIdFixer(UnicastBus bus)
    {
        this.bus = bus;
    }

    public void Start()
    {
        var hostId = CreateGuid(Environment.MachineName, Configure.EndpointName);
        var identifier = Assembly.GetExecutingAssembly().Location;
        bus.HostInformation = new HostInformation(hostId, Environment.MachineName, identifier);
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
