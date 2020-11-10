using System;

public class CustomComponent
{
    readonly string initialValue;

    public CustomComponent(string initialValue)
    {
        this.initialValue = $"{initialValue} {DateTime.UtcNow:O}";
    }

    public string GetValue() => initialValue;
}