using System;

[Serializable]
public class HeaderInfo
{
    // The key used to lookup the value in the header collection.
    public string Key { get; set; }

    // The value stored under the key in the header collection.
    public string Value { get; set; }
}