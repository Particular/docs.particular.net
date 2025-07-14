using NServiceBus;
using System;

namespace Endpoint;

public class TestMsg : ICommand
{
    public Guid Id { get; set; }
}