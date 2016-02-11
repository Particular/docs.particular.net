using System;

[Serializable]
public class HeaderInfo
{
    /// <summary>
    /// The key used to lookup the value in the header collection.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// The value stored under the key in the header collection.
    /// </summary>
    public string Value { get; set; }
}

