# EllipticBit.Coalescence.SignalR

ASP.NET Core SignalR client support library for the [Coalescence](https://gitlab.com/EllipticBit/coalescence-generator) code generation system.

This package provides a small dependency-injection friendly repository over one or more SignalR `HubConnection` instances, allowing Coalescence-generated SignalR clients (and your own code) to resolve named hub connections from the service container.

## Features

- `AddCoalescenceSignalRServices(HubConnection)` registers the connection repository and a default `HubConnection`.
- `ICoalescenceSignalRRepository` resolves the default connection (`Get()`) or a named connection (`Get(name)`).
- `ICoalescenceSignalRServiceBuilder.AddHubConnection(name, connection)` registers additional named connections.

## Requirements

- Targets `.NET Standard 2.0`, so it runs on .NET Framework 4.6.1+, .NET Core 2.0+, and modern .NET (.NET 8+).
- Depends on `Microsoft.AspNetCore.SignalR.Client`.

## Installation

Install from [NuGet](https://www.nuget.org/):

```bash
dotnet add package EllipticBit.Coalescence.SignalR
```

Or add a `PackageReference` to your project file (replace the version with the latest published version):

```xml
<PackageReference Include="EllipticBit.Coalescence.SignalR" Version="x.y.z" />
```

## Getting Started

### 1. Register the default hub connection

Build a `HubConnection` and register it as the default Coalescence SignalR connection:

```csharp
using EllipticBit.Coalescence.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

var connection = new HubConnectionBuilder()
    .WithUrl("https://api.example.com/hubs/events")
    .WithAutomaticReconnect()
    .Build();

// Registers ICoalescenceSignalRRepository with the default connection.
var builder = services.AddCoalescenceSignalRServices(connection);
```

### 2. Register additional named connections (optional)

```csharp
var notifications = new HubConnectionBuilder()
    .WithUrl("https://api.example.com/hubs/notifications")
    .Build();

builder.AddHubConnection("notifications", notifications);
```

### 3. Resolve and use a connection

```csharp
using EllipticBit.Coalescence.SignalR;

var provider = services.BuildServiceProvider();
var repository = provider.GetRequiredService<ICoalescenceSignalRRepository>();

// Default connection.
HubConnection hub = repository.Get();

// Named connection.
HubConnection notifications = repository.Get("notifications");

await hub.StartAsync();
var result = await hub.InvokeCoreAsync<string>("Echo", new object[] { "hello" });
```

## Related Packages

| Package | Description |
| --- | --- |
| `EllipticBit.Coalescence.Shared` | Shared abstractions. |
| `EllipticBit.Coalescence.Request` | HTTP client transport. |
| `EllipticBit.Coalescence.AspNetCore` | ASP.NET Core server-side support. |
| `EllipticBit.Coalescence.SignalR` | SignalR client transport support (this package). |

## License

Licensed under the [MIT License](https://gitlab.com/EllipticBit/coalescence-dotnet). See the `LICENSE` file for details.

## Contributing

Contributions are welcome! To contribute:

1. Fork the repository and create a feature branch.
2. Make your changes, following the existing code style and conventions.
3. Add or update tests where appropriate and ensure the solution builds.
4. Open a merge/pull request with a clear description of the change and its motivation.

### AI / LLM-assisted contributions

If any part of your contribution was generated with the assistance of a Large Language Model (LLM) or other generative AI tool, you **must** include the exact prompt(s) used to generate the contribution in the `PROMPTS.txt` file at the root of the repository. Append each prompt along with a short note describing what it produced. Pull requests containing LLM-generated content without the corresponding prompts will not be accepted.
