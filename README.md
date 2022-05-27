# WritableOptions

## What is WritableOptions?
Using the IConfiguration and IOptions APIs from the `Microsoft.Extensions.Configuration` .NET library. WritableOptions allows you to create a simple configuration POCO class, to persist user settings to a JSON settings file. The default file used is `appsettings.json`.
These settings can be change and persisted at run time of your application.
WritableOptions also supports Dependendency Injection.

# Documentation

## Getting Started

### Install-Package
```powershell
Install-Package IGE.WritableOptions
```

### Configuring Writable Options

Simplest configuration, using `IHostBuilder` extension method, or `IServiceCollection` extension Method.

```csharp
hostBuilder.UseWritableOptions<MySettings>("MySettings");
```

```csharp
services.AddWritableOptions<MySettings>(configurationRoot, "MySettings");
```

Or using a custom `*.json` file. This will create a new file in the specified directory, relative to the apps executing directory `.\config\mysettings.json`

```csharp
hostBuilder.UseWritableOptions<MySettings>("MySettings", "config/mysettings.json");
```

```csharp
services.AddWritableOptions<MySettings>(configurationRoot, "MySettings", "config/mysettings.json");
```

A custom JSON Serializer can also be used. Teh below example is what's used by default.

```csharp
hostBuilder.UseWritableOptions<MySettings>(\
    nameof(MySettings),
    "config/mysettings.json",
    jsonSerializerOptionsFactory: () => new JsonSerializerOptions 
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
    });
```

```csharp
services.AddWritableOptions<MySettings>(
    configurationRoot,
    nameof(MySettings),
    "config/mysettings.json",
    jsonSerializerOptionsFactory: () => new JsonSerializerOptions 
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
    });
```

### Reading and Updating Writable Options
First, simply inject an `IOptions<MySettings>` object for reading, or a `IWritableOptions<MySettings>` object to reading and writing.
```csharp
public class MySettings
{
    public string? Name { get; set; }
    public int HighScore { get; set; }
}
```
```csharp
private readonly IWritableOptions<MySettings> options;

public MyControllerService(IWritableOptions<MySettings> options)
{
    this.options = options;
}
```

```csharp
this.options.Update(opt => {
    opt.Name = "Grugg";
    opt.HighScore = 9001;
});
```
To read data, simple access the options.Value object.
```csharp
MySettings settings = this.options.Value;
```