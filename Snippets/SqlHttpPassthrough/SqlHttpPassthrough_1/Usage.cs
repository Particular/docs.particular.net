using System.Collections.Generic;
using NServiceBus.Transport.SqlServerNative;

public class Usage
{
    void Serialize()
    {
        #region Serialize


        #endregion
    }

    void Deserialize()
    {
        string headersString = null;

        #region Deserialize

        var headers = Headers.DeSerialize(headersString);

        #endregion
    }
}