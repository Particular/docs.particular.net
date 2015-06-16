namespace Snippets4.HostIdentifier
{
    using System;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Hosting;
    using NServiceBus.Unicast;

    #region HostIdFixer

    public class HostIdFixer : IWantToRunWhenConfigurationIsComplete
    {

        public HostIdFixer(UnicastBus bus)
        {
            Guid hostId = CreateGuid(Environment.MachineName, Configure.EndpointName);
            string identifier = Assembly.GetExecutingAssembly().Location;
            bus.HostInformation = new HostInformation(hostId, Environment.MachineName, identifier);
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

        public void Run()
        {
        }
    }

    #endregion
}