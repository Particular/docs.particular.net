using NServiceBus;
using NServiceBus.DataBus;
using System.IO;
using System;
using NServiceBus.Features;
using System.Threading.Tasks;
using System.Threading;
using Core7.DataBus.Custom;

class DataBusUpgradeGuide
{
    void SerializerMandatory(EndpointConfiguration endpointConfiguration)
    {
        #region 7to8-databus-serializer-mandatory
        endpointConfiguration.UseDataBus<FileShareDataBus>();
        #endregion
    }

    void CustomSerializer(EndpointConfiguration endpointConfiguration)
    {
        #region 7to8-databus-custom-serializer
        endpointConfiguration.RegisterComponents(registration => registration.ConfigureComponent<MyCustomDataBusSerializer>(DependencyLifecycle.SingleInstance));
        endpointConfiguration.UseDataBus<FileShareDataBus>();
        #endregion
    }

    void CustomImplementation(EndpointConfiguration endpointConfiguration)
    {
        #region 7to8-databus-custom-implementation
        endpointConfiguration.UseDataBus(typeof(MyCustomDataBus));
        #endregion
    }

    class MyCustomDataBus : IDataBus
    {
        public Task<Stream> Get(string key)
        {
            throw new NotImplementedException();
        }

        public Task<string> Put(Stream stream, TimeSpan timeToBeReceived)
        {
            throw new NotImplementedException();
        }

        public Task Start()
        {
            throw new NotImplementedException();
        }
    }

    class MyCustomDataBusSerializer : IDataBusSerializer
    {
        public object Deserialize(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void Serialize(object databusProperty, Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}