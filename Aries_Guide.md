## Add configuration
1. Found your project and right click mouse, choose Edit csproj.
2. Add below configuration,when you add pelase note <b>DotNetCliToolReference</b> is what we needed rather than <b>PackageReference</b>.
```
<ItemGroup>
    <DotNetCliToolReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Tools" Version="2.0.0" />
    <DotNetCliToolReference Include="dotnet-aries" Version="*" /><!--add this-->
</ItemGroup>
```
This is a necessary step.

## How to use 
In this step we hypothesis you had add conguration in your csproj. It's easy for us to use this tool.<br>
- Open the terminal, here we take Windows as an example. Founding the base path of your project.<br>
Run the command
```
dotnet publish -c Release
```
- Run aries command to export Document
```
dotnet aries doc -t D:\AriesDoc\ -f D:\Example\bin\Release\netcoreapp2.0\publish -b http://localhost:63298
```
- Export success will output
```
Aries doc generate Done
```
Finished!

## Parameters 
 - -t required! You should gave the path where Aries to exported API Doc.
 - -f required! You should point to where the <b>publish</b> folder is.
 - -b required! The base path of your API. Actually, it is not a necessary field, but we recommend it. 
 - -s Optional! The start class name of your project. if not given, it would use the default field <b>StartUp</b>
 - -v Optional! The version number of raml. If not given, it would use the default value <b>1.0</b>.