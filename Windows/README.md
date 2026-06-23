# EllipticBit.Coalescence.Windows

WPF and WinForms specific utilities for the [Coalescence](https://gitlab.com/EllipticBit/coalescence-generator) code generation system.

This package contains client UI helpers used by Coalescence-generated desktop applications, including change-tracking model objects that implement `INotifyPropertyChanged`, identity-based instance caching, and a fluent builder for composing stable hash keys from arbitrary values using xxHash64.

## Features

- `TrackingObject` / `TrackingObjectBase` - base classes implementing `INotifyPropertyChanged` with built-in change tracking (`HasChanges`) and JSON (de)serialization hooks.
- Identity caching of tracking objects via weak references so a single instance is shared for a given tracking key.
- `HashKeyBuilder` and the `AddKey(...)` extension methods - build a `ulong` xxHash64 key from numbers, strings, GUIDs, dates, byte arrays, and collections of those types.

## Requirements

- Targets `.NET Framework 4.8` and `.NET 8.0 (Windows)`.
- Uses WPF (`UseWPF`), so it is intended for Windows desktop applications.

## Installation

Install from [NuGet](https://www.nuget.org/):

```bash
dotnet add package EllipticBit.Coalescence.Windows
```

Or add a `PackageReference` to your project file (replace the version with the latest published version):

```xml
<PackageReference Include="EllipticBit.Coalescence.Windows" Version="x.y.z" />
```

## Getting Started

### Build a hash key

`HashKeyBuilder` lets you compose a deterministic 64-bit key from any combination of values. The `AddKey` extension methods are fluent, so calls can be chained, and `HashKey` returns the computed `ulong`.

```csharp
using EllipticBit.Coalescence.Windows;

ulong key = new HashKeyBuilder()
    .AddKey(customerId)            // int, long, Guid, string, DateTime, ...
    .AddKey(name)
    .AddKey(new[] { 1, 2, 3 })     // collections are supported too
    .HashKey;
```

### Create a change-tracking model

Derive your view models from `TrackingObject<T>` to get `INotifyPropertyChanged` and change tracking for free. `HasChanges` reflects whether any tracked property has been modified.

```csharp
using EllipticBit.Coalescence.Windows;

public sealed class Customer : TrackingObject<Customer>
{
    private string _name;

    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
    }
}

var customer = new Customer();
customer.Name = "Ada Lovelace";

// Bind directly in WPF; HasChanges becomes true after the assignment above.
bool dirty = customer.HasChanges;
```

> The exact change-notification helper methods are defined on the `TrackingObject` base type; consult IntelliSense for the members available on your target framework.

## Related Packages

| Package | Description |
| --- | --- |
| `EllipticBit.Coalescence.Shared` | Shared abstractions. |
| `EllipticBit.Coalescence.Request` | HTTP client transport. |
| `EllipticBit.Coalescence.AspNetCore` | ASP.NET Core server-side support. |
| `EllipticBit.Coalescence.Windows` | WPF/WinForms utilities (this package). |

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
