using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using NServiceBus;
using NServiceBus.Hosting;
using NServiceBus.Unicast;

#region HostIdFixer
public class HostIdFixer : IWantToRunWhenBusStartsAndStops
{
    UnicastBus bus;

    public HostIdFixer(UnicastBus bus)
    {
        this.bus = bus;
    }

    public void Start()
    {
        Guid hostId = CreateGuid(Environment.MachineName, Configure.EndpointName);
        string identifier = Assembly.GetExecutingAssembly().Location;
        bus.HostInformation = new HostInformation(hostId, Environment.MachineName, identifier);
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
