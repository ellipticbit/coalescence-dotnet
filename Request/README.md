# EllipticBit.Coalescence.Request

HTTP client transport library for the [Coalescence](https://gitlab.com/EllipticBit/coalescence-generator) code generation system.

This package provides a fluent, options-driven HTTP request pipeline built on top of `IHttpClientFactory`. It is the client-side runtime used by Coalescence-generated Web API clients, but it can also be used directly to build and send HTTP requests with pluggable serialization, authentication, automatic retries, and optional Zstandard (zstd) compression.

## Features

- Fluent request builder: `CreateRequest(name).Get()/Post()/Put()/Patch()/Delete()/Head()`.
- Named configuration via `CoalescenceRequestOptions` (HttpClient id, retry count, retry delay, date formatting, default authentication scheme, error handling).
- Integrates with `IHttpClientFactory` named clients.
- Pluggable JSON/XML serialization inherited from `EllipticBit.Coalescence.Shared`.
- Built-in retry with configurable count and delay.
- Optional zstd request/response compression via `ZStdDelegatingHandler`.

## Requirements

- Targets `.NET Standard 2.0`, so it runs on .NET Framework 4.6.1+, .NET Core 2.0+, and modern .NET (.NET 8+).

## Installation

Install from [NuGet](https://www.nuget.org/):

```bash
dotnet add package EllipticBit.Coalescence.Request
```

Or add a `PackageReference` to your project file (replace the version with the latest published version):

```xml
<PackageReference Include="EllipticBit.Coalescence.Request" Version="x.y.z" />
```

## Getting Started

### 1. Register the services

`AddCoalescenceRequestServices()` registers the request factory, while `AddCoalescenceRequestOptions(...)` (an extension on the shared `ICoalescenceServiceBuilder`) registers one or more named option sets. Each option set is bound to a named `HttpClient`.

```csharp
using EllipticBit.Coalescence.Request;
using EllipticBit.Coalescence.Shared;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// Register a named HttpClient that the request options will use.
services.AddHttpClient("example-api", client =>
{
    client.BaseAddress = new Uri("https://api.example.com");
});

// Register the shared services + the named Coalescence request options.
services.AddCoalescenceServices()
    .AddCoalescenceRequestOptions("example", new CoalescenceRequestOptions("example", "example-api")
    {
        MaxRetryCount = 3,
        RetryDelay = 250
    });

// Register the request factory.
services.AddCoalescenceRequestServices();

var provider = services.BuildServiceProvider();
```

### 2. Send a request

Resolve `ICoalescenceRequestFactory` and build a request fluently. The response is awaited and disposed asynchronously.

```csharp
using EllipticBit.Coalescence.Shared.Request;

var factory = provider.GetRequiredService<ICoalescenceRequestFactory>();

// GET https://api.example.com with the default authentication scheme.
await using var response = await factory.CreateRequest("example")
    .Get()
    .Authentication()
    .Send();

string body = await response.AsString();
```

### 3. POST/PUT/DELETE with a serialized body

```csharp
var factory = provider.GetRequiredService<ICoalescenceRequestFactory>();

await using var response = await factory.CreateRequest("example")
    .Post()
    .Path("api", "v1", "contacts")
    .Serialized(new Contact { Name = "Ada Lovelace", Email = "ada@example.com" })
    .Send();

var created = await response.AsSerialized<Contact>();
```

### Deserializing typed responses

`ICoalescenceResponse` exposes helpers such as `AsString()` and `AsSerialized<T>()` to read the response payload using the configured serializer.

## Related Packages

| Package | Description |
| --- | --- |
| `EllipticBit.Coalescence.Shared` | Shared abstractions (pulled in automatically). |
| `EllipticBit.Coalescence.Request` | HTTP client transport (this package). |
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
