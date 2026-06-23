# EllipticBit.Coalescence.AspNetCore

ASP.NET Core server-side support library for the [Coalescence](https://gitlab.com/EllipticBit/coalescence-generator) code generation system.

This package provides the server-side building blocks used by Coalescence-generated ASP.NET Core controllers, including a controller base class with multipart helpers, exception-to-HTTP middleware, additional route constraints for the numeric and date/time types that ASP.NET Core does not support out of the box, and a Zstandard (zstd) response compression provider.

## Features

- `CoalescenceControllerBase` - a `ControllerBase` subclass with helpers for reading multipart content as serialized objects, text, streams, or byte arrays.
- `CoalescenceExceptionMiddleware` - translates `CoalescenceHttpException` instances into structured HTTP responses.
- `AddCoalescenceConstraints()` - registers route constraints for `byte`, `sbyte`, `short`, `ushort`, `uint`, `ulong`, `DateTimeOffset`, and `TimeSpan`.
- `ZStdCompressionProvider` - a zstd `ICompressionProvider` for ASP.NET Core response compression.

## Requirements

- Targets `.NET 8.0` and references the `Microsoft.AspNetCore.App` shared framework.

## Installation

Install from [NuGet](https://www.nuget.org/):

```bash
dotnet add package EllipticBit.Coalescence.AspNetCore
```

Or add a `PackageReference` to your project file (replace the version with the latest published version):

```xml
<PackageReference Include="EllipticBit.Coalescence.AspNetCore" Version="x.y.z" />
```

## Getting Started

### Register route constraints

Add the Coalescence route constraints to the routing options during application startup:

```csharp
using EllipticBit.Coalescence.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.AddCoalescenceConstraints();
});
```

### Register the exception middleware

Add `CoalescenceExceptionMiddleware` to the request pipeline to convert `CoalescenceHttpException` instances into HTTP responses:

```csharp
var app = builder.Build();

app.UseMiddleware<CoalescenceExceptionMiddleware>();
app.MapControllers();

app.Run();
```

### Enable zstd response compression

Register the zstd compression provider with ASP.NET Core response compression:

```csharp
using EllipticBit.Coalescence.AspNetCore;

builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<ZStdCompressionProvider>();
});

// ...
app.UseResponseCompression();
```

### Derive a controller from CoalescenceControllerBase

`CoalescenceControllerBase` is constructed with the registered `ICoalescenceSerializer` instances and exposes helpers for reading multipart uploads:

```csharp
using EllipticBit.Coalescence.AspNetCore;
using EllipticBit.Coalescence.Shared;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/contacts")]
public class ContactsController : CoalescenceControllerBase
{
    public ContactsController(IEnumerable<ICoalescenceSerializer> serializers)
        : base(serializers)
    {
    }

    [HttpPost("import")]
    public async Task<IActionResult> Import()
    {
        var contact = await MultipartAsSerialized<Contact>("payload");
        return Ok(contact);
    }
}
```

## Related Packages

| Package | Description |
| --- | --- |
| `EllipticBit.Coalescence.Shared` | Shared abstractions (pulled in automatically). |
| `EllipticBit.Coalescence.Request` | HTTP client transport. |
| `EllipticBit.Coalescence.AspNetCore` | ASP.NET Core server-side support (this package). |
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
