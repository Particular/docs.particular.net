using System;

class ChaosGenerator
{
    public bool IsFailing { get; set; }

    public void Invoke()
    {
        if (IsFailing)
        {
            throw new Exception("Database is down");
        }
    }
}