# Create and run project

[.NET Core command-line interface](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2)

The .NET Core command-line interface (CLI) is a new cross-platform toolchain for developing .NET applications. The CLI is a foundation upon which higher-level tools, such as Integrated Development Environments (IDEs), editors, and build orchestrators, can rest.

> dotnet new console -lang F# -o helloworld  
> cd helloworld  
> dotnet run

# F# console structure

## Open

Referencing code by using the fully qualified namespace or module path every time can create code that is hard to write, read, and maintain. Instead, you can use the open keyword for frequently used modules and namespaces so that when you reference a member of that module or namespace, you can use the short form of the name instead of the fully qualified name.

## [<EntryPoint>]
The entry point to a program that is compiled as an executable file is where execution formally starts. You specify the entry point to an F# application by applying the EntryPoint attribute to the program's main function.

## let
A binding associates an identifier with a value or function. You use the let keyword to bind a name to a value or function.

## printfn
Prints formatted output to stdout, adding a newline.
https://en.wikipedia.org/wiki/Printf_format_string

## Console
Represents the standard input, output, and error streams for console applications.

# Publish & Run

> dotnet publish -c Release -o publish
> dotnet publish\helloworld.dll

' https://hub.docker.com/r/microsoft/dotnet/
' > docker run -it --name fsharp-demo microsoft/dotnet:latest
> docker run -it --name fsharp-demo microsoft/dotnet:2.1.401-sdk

> mkdir helloworld

> docker cp publish/. fsharp-demo:/helloworld

> dotnet helloworld.dll

' > docker container rm fsharp-demo

> dotnet publish -c Release -r win10-x64 -o win10-x64

> win10-x64\helloworld.exe
