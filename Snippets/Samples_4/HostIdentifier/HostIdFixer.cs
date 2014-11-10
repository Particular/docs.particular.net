namespace Snippets_4.HostIdentifier
{
    using System;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using NServiceBus;
    using NServiceBus.Hosting;
    using NServiceBus.Unicast;

    #region HostIdFixer-V4
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
            var instanceIdentifier = Assembly.GetExecutingAssembly().Location;
            bus.HostInformation = new HostInformation(hostId, Environment.MachineName, instanceIdentifier);
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
}
