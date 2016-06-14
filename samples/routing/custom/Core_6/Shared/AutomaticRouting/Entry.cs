class Entry
{
    public Entry(string publisher, string data)
    {
        Publisher = publisher;
        Data = data;
    }

    public string Publisher { get; set; }
    public string Data { get; }
}
