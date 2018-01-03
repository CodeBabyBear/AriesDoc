
dotnet publish -c Release
dotnet %CD%\src\dotnet-aries-doc\bin\Release\netcoreapp2.0\publish\dotnet-aries.dll doc -f %CD%\test\Example\bin\Release\netcoreapp2.0\publish -t %CD%\test\Example\bin\Release\netcoreapp2.0\publish -b http://www.1.com