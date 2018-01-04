# AriesDoc

AriesDoc is help we generate raml doc file form asp.net core.

It base on ```Microsoft.AspNetCore.Mvc.ApiExplorer```.


## Quick start

### Install package

* Package Manager

```
Install-Package Pandv.AriesDoc.Generator -Version 0.0.1
```
* .NET CLI

```
dotnet add package Pandv.AriesDoc.Generator --version 0.0.1
```
* Paket CLI

```
paket add Pandv.AriesDoc.Generator --version 0.0.1
```

Nuget : https://www.nuget.org/packages/Pandv.AriesDoc.Generator/0.0.1

### Simple use example

1. set the services

``` csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc(o => o.SetApiExplorerVisible());
        services.AddRAMLDocGeneratorV08();
    }
}
```

2. call the generate function

``` csharp
public class Program
{
    public static void Main(string[] args)
    {
        BuildWebHost(args)
            .GeneratorDoc(Directory.GetCurrentDirectory());
    }
}
```

The example project is here : https://github.com/CodeBabyBear/AriesDoc/tree/master/test/Example

