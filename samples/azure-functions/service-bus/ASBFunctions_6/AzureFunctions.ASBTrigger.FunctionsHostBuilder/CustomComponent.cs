using System;

public class CustomComponent(string initialValue)
{
    readonly string initialValue = $"{initialValue} (Constructed at {DateTime.UtcNow:O})";

    public string GetValue() => initialValue;
}