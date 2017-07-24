#pragma warning disable 618
namespace Core4.HostIdentifier
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

    public class HostIdFixer :
        IWantToRunWhenConfigurationIsComplete
    {

        public HostIdFixer(UnicastBus bus)
        {
            var hostId = CreateGuid(Environment.MachineName, Configure.EndpointName);
            var identifier = Assembly.GetExecutingAssembly().Location;
            bus.HostInformation = new HostInformation(
                hostId: hostId,
                displayName: Environment.MachineName,
                displayInstanceIdentifier: identifier);
        }

        static Guid CreateGuid(params string[] data)
        {
            using (var provider = new MD5CryptoServiceProvider())
            {
                var inputBytes = Encoding.Default.GetBytes(string.Concat(data));
                var hashBytes = provider.ComputeHash(inputBytes);
                return new Guid(hashBytes);
            }
        }

        public void Run()
        {
        }
    }

    #endregion
}