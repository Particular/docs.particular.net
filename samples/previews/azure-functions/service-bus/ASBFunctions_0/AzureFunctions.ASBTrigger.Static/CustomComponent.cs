using System;

public class CustomComponent
{
    readonly string initialValue;

    public CustomComponent(string initialValue)
    {
        this.initialValue = $"{initialValue} (Constructed at {DateTime.UtcNow:O})";
    }

    public string GetValue() => initialValue;
}