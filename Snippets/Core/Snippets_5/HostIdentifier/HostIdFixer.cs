#pragma warning disable 618

namespace Core5.HostIdentifier
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Hosting;
    using NServiceBus.Settings;
    using NServiceBus.Unicast;

    #region HostIdFixer

    public class HostIdFixer : IWantToRunWhenConfigurationIsComplete
    {

        public HostIdFixer(UnicastBus bus, ReadOnlySettings settings)
        {
            Guid hostId = CreateGuid(Environment.MachineName, settings.EndpointName());
            string location = Assembly.GetExecutingAssembly().Location;
            Dictionary<string, string> properties = new Dictionary<string, string>
            {
                {"Location", location}
            };
            bus.HostInformation = new HostInformation(hostId, Environment.MachineName, properties);
        }

        static Guid CreateGuid(params string[] data)
        {
            using (MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider())
            {
                byte[] inputBytes = Encoding.Default.GetBytes(string.Concat(data));
                byte[] hashBytes = provider.ComputeHash(inputBytes);
                return new Guid(hashBytes);
            }
        }

        public void Run(Configure config)
        {
        }
    }

    #endregion

}