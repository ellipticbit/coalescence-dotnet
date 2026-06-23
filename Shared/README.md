# EllipticBit.Coalescence.Shared

Shared support library for the [Coalescence](https://gitlab.com/EllipticBit/coalescence-generator) code generation system.

Coalescence generates strongly-typed Web API clients and servers from a single contract definition. This package contains the framework-agnostic primitives that the generated code and the other Coalescence runtime libraries (Request, AspNetCore, SignalR) build on, including the dependency-injection service builder, serialization abstractions, authentication abstractions, and the request/response contracts.

## Features

- `AddCoalescenceServices()` dependency-injection entry point returning an `ICoalescenceServiceBuilder`.
- Named, options-based configuration via `CoalescenceOptionsBase` and `ICoalescenceOptionsRepository`.
- Pluggable serialization through `ICoalescenceSerializer` with built-in JSON (`CoalescenceJsonSerializer`) and XML (`CoalescenceXmlSerializer`) implementations.
- Authentication abstraction via `ICoalescenceAuthentication` (with a no-op `CoalescenceNullAuthentication` default).
- Shared request/response contracts (`ICoalescenceRequest`, `ICoalescenceRequestFactory`, `ICoalescenceResponse`, multipart content builders) consumed by the transport-specific packages.

## Requirements

- Targets `.NET Standard 2.0`, so it runs on .NET Framework 4.6.1+, .NET Core 2.0+, and modern .NET (.NET 8+).

## Installation

Install from [NuGet](https://www.nuget.org/):

```bash
dotnet add package EllipticBit.Coalescence.Shared
```

Or add a `PackageReference` to your project file (replace the version with the latest published version):

```xml
<PackageReference Include="EllipticBit.Coalescence.Shared" Version="x.y.z" />
```

> In most applications you will not reference this package directly. It is pulled in automatically by the transport packages such as `EllipticBit.Coalescence.Request`.

## Getting Started

Register the shared Coalescence services with the .NET dependency injection container. `AddCoalescenceServices()` returns an `ICoalescenceServiceBuilder` that the other Coalescence packages extend to register their named options.

```csharp
using EllipticBit.Coalescence.Shared;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// Registers the shared authentication and options repository services.
ICoalescenceServiceBuilder builder = services.AddCoalescenceServices();
```

### Providing a custom serializer

Implement `ICoalescenceSerializer` to control how request and response bodies are (de)serialized, then supply it through the options of a transport package.

```csharp
using EllipticBit.Coalescence.Shared;

public sealed class MyJsonSerializer : ICoalescenceSerializer
{
    public string ContentType => "application/json";

    public Task<string> Serialize<T>(T value) { /* ... */ }

    public Task<T> Deserialize<T>(string content) { /* ... */ }
}
```

### Providing custom authentication

Implement `ICoalescenceAuthentication` to attach credentials (bearer tokens, API keys, etc.) to outgoing requests:

```csharp
using EllipticBit.Coalescence.Shared;
using Microsoft.Extensions.DependencyInjection;

services.AddCoalescenceServices();
services.AddTransient<ICoalescenceAuthentication, MyBearerTokenAuthentication>();
```

## Related Packages

| Package | Description |
| --- | --- |
| `EllipticBit.Coalescence.Shared` | Shared abstractions (this package). |
| `EllipticBit.Coalescence.Request` | HTTP client transport built on `HttpClient`. |
| `EllipticBit.Coalescence.AspNetCore` | ASP.NET Core server-side support. |
| `EllipticBit.Coalescence.SignalR` | SignalR client transport support. |

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
