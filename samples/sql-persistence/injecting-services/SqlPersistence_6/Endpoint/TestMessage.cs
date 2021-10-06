using NServiceBus;
using System;

public class TestMessage : ICommand
{
    public Guid Id { get; set; }
}