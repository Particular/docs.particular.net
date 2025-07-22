# NServiceBus Dependency Injection - Externally Managed Mode (.NET 10)

This sample demonstrates how to use NServiceBus with an external dependency injection container in .NET 10, using modern C# features.

## Features Demonstrated

- External dependency injection container management with Microsoft.Extensions.DependencyInjection
- NServiceBus endpoint configuration with external container integration
- Modern C# 12+ features:
  - Primary constructors for dependency injection
  - Record types for immutable messages
  - Top-level programs

## Key Components

- **MyMessage**: Record type implementing IMessage
- **Greeter**: Service class using primary constructor for logger injection
- **MyHandler**: Message handler using primary constructor for multiple dependencies
- **MessageSender**: Service for sending messages using primary constructor
- **Program**: Main application using Host.CreateApplicationBuilder and external container

## Prerequisites

- .NET 10 SDK
- Visual Studio 2022 17.12+ or compatible IDE

## Running the Sample

1. Start the solution
2. Press Enter to send a message
3. Observe the message being handled with dependency injection
4. Press any other key to exit

## Project Structure

```text
Sample/
├── Sample.csproj          # .NET 10 project file
├── Program.cs             # Main application with external DI
├── MyMessage.cs           # Message record type
├── MyHandler.cs           # Message handler with DI
├── Greeter.cs             # Injected service
└── MessageSender.cs       # Message sending service
```
