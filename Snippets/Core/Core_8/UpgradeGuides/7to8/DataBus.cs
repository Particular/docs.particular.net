namespace Core8.UpgradeGuides
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus;
    using NServiceBus.DataBus;

    class DataBusUpgradeGuide
    {
        void SerializerMandatory(EndpointConfiguration endpointConfiguration)
        {
            #region 7to8-databus-serializer-mandatory
            endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>();
            #endregion
        }

        void CustomSerializer(EndpointConfiguration endpointConfiguration)
        {
            #region 7to8-databus-custom-serializer
            endpointConfiguration.UseDataBus<FileShareDataBus, MyCustomDataBusSerializer>();
            #endregion
        }

        void CustomImplementation(EndpointConfiguration endpointConfiguration)
        {
            #region 7to8-databus-custom-implementation
            endpointConfiguration.UseDataBus(serviceProvider => new MyCustomDataBus(serviceProvider.GetRequiredService<SomeDependency>()), new SystemJsonDataBusSerializer());
            #endregion
        }

        class SomeDependency
        {
        }

        class MyCustomDataBus : IDataBus
        {
            public MyCustomDataBus(SomeDependency dependency)
            {
            }

            public Task<Stream> Get(string key, CancellationToken cancellationToken = new CancellationToken())
            {
                return Task.FromResult(default(Stream));
            }

            public Task<string> Put(Stream stream, TimeSpan timeToBeReceived, CancellationToken cancellationToken = new CancellationToken())
            {
                return Task.FromResult("");
            }

            public Task Start(CancellationToken cancellationToken = new CancellationToken())
            {
                return Task.CompletedTask;
            }
        }

        class MyCustomDataBusSerializer : IDataBusSerializer
        {
            public string ContentType => throw new NotImplementedException();

            #region 7to8-databus-type-information
            public object Deserialize(Type propertyType, Stream stream)
            #endregion
            {
                throw new NotImplementedException();
            }


            public void Serialize(object databusProperty, Stream stream)
            {
                throw new NotImplementedException();
            }
        }
    }
}