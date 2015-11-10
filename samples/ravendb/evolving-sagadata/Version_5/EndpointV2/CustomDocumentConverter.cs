using Raven.Client.Listeners;
using Raven.Json.Linq;

#region converter
public class CustomDocumentConverter : IDocumentConversionListener
{

    public void AfterConversionToEntity(string key, RavenJObject document, RavenJObject metadata, object entity)
    {
        OrderSagaData data = entity as OrderSagaData;
        if (data != null)
        {
            data.NumberOfItems = document["ItemCount"].Value<int>();
        }
    }

    public void AfterConversionToDocument(string key, object entity, RavenJObject document, RavenJObject metadata)
    {
        OrderSagaData data = entity as OrderSagaData;
        if (data != null)
        {
            document["ItemCount"] = data.NumberOfItems;
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