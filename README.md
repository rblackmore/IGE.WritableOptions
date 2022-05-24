# WritableOptions
WritableOptions allows you to write configuration options to json file. By default, this is your *'appsettings.json'* file, but can be changed if desired.

## Documentation

### Dependency Injection
Using Microsoft.Extensions.DependencyInjection, use the IServiceCollection extension method `ConfigureWritableOptions<T>()` where `T` is your options POCO class.

**The Section must be an imediate child of the root object in the *.json file.**

#### Examples:


MyConfig Class
```csharp
    public class MyConfig
    {
        public string Name { get; set; }
        public int Score { get; set; }
    }
```
Simple Example, which will use the SectionName 'MyConfig' and use the *appsettings.json* file.
```csharp
    IConfigurationSection configurationSection = 
    context.Configuration.GetSection(nameof(MyConfig));

    services.ConfigureWritableOptions<MyConfig>(configurationSection);
```
Custom file
```csharp

    services.ConfigureWritableOptions<MyConfig>(
        configurationSection,
        file: "appsettings.myconfig.json");
```
Custom JsonSerializerOptions (default is `WriteIndented = true`)
```csharp

    JsonSerializerOptions jsonOptions = new () 
    {
        WriteIndented = false,    
    }

    services.ConfigureWritableOptions<MyConfig>(
        configurationSection,
        jsonSerializeOptions: jsonOptions);
```

### Reading Configuration
Reading configuration settings is simple. Just inject 