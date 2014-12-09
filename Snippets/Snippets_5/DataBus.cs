using NServiceBus;
using NServiceBus.DataBus;
using System;
using System.IO;

public class DataBus
{
    public void Simple()
    {
        #region FileShareDataBus

        var configuration = new BusConfiguration();

        configuration.UseDataBus<FileShareDataBus>();

        #endregion
    }
}

namespace CustomDataBusPluginSnippet
{
    #region CustomDataBus

    class CustomDataBus : IDataBus
    {
        public Stream Get( string key )
        {
            return File.OpenRead( "blob.dat" );
        }

        public string Put( Stream stream, TimeSpan timeToBeReceived )
        {
            using( var destination = File.OpenWrite( "blob.dat" ) )
            {
                stream.CopyTo( destination );
            }
            return "the-key-of-the-stored-file-such-as-the-full-path";
        }

        public void Start()
        {
        }
    }

    #endregion

    #region PluginCustomDataBusV5 5


    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public void Customize( BusConfiguration configuration )
        {
            //other configuration calls

            configuration.UseDataBus( typeof( CustomDataBus ) );
        }
    }

    #endregion

}