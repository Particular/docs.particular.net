public class DataBusBlobData
{
    public DataBusBlobData(string name, string validUntilUtc)
    {
        Name = name;
        ValidUntilUtc = validUntilUtc;
    }

    public string Name { get; set; }

    public string ValidUntilUtc { get; set; }
}