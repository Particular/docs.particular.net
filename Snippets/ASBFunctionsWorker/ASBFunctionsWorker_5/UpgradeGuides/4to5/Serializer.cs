using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.MessageInterfaces;
using NServiceBus.Serialization;
using NServiceBus.Settings;

namespace ASBFunctionsWorker_5.UpgradeGuides._4to5
{
    class Serializer
    {
        public void SetSerializer()
        {
            #region ASBFunctionsWorker-4to5-serializer
            var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .UseNServiceBus(configuration =>
            {
                configuration.AdvancedConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
            })
            .Build();
            #endregion
        }
    }

    class NewtonsoftJsonSerializer : SerializationDefinition
    {
        public override Func<IMessageMapper, IMessageSerializer> Configure(IReadOnlySettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
