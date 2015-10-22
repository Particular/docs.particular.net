using Raven.Client.Listeners;
using Raven.Json.Linq;

#region documentconverter
public class CustomDocumentConverter : IDocumentConversionListener
{
    public void AfterConversionToDocument(string key, object entity, RavenJObject document, RavenJObject metadata)
    {
        //write
        OrderSagaData data = entity as OrderSagaData;
        if (data != null)
        {
            document["ItemCount"] = data.NumberOfItems;
        }
    }

    public void AfterConversionToEntity(string key, RavenJObject document, RavenJObject metadata, object entity)
    {
        //read
        OrderSagaData data = entity as OrderSagaData;
        if (data != null)
        {
            // Since version 1 is only aware of ItemCount it will not set NumberOfItems.
            // But version 2 will always set ItemCount so we are safe to use ItemCount 
            // until all version 1 have been decommissioned. 
            data.NumberOfItems = document["ItemCount"].Value<int>();
        }
    }

    public void BeforeConversionToDocument(string key, object entity, RavenJObject metadata)
    {
    }

    public void BeforeConversionToEntity(string key, RavenJObject document, RavenJObject metadata)
    {
    }
}
#endregion