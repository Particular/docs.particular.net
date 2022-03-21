using NServiceBus;
using System;

public class TestMsg : ICommand
{
    public Guid Id { get; set; }
}