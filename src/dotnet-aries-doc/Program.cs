using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Pandv.AriesDoc.Generator;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Loader;

namespace Pandv.AriesDoc
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return ExecCommand(args);
        }

        private static int ExecCommand(string[] args)
        {
            try
            {
                var app = new CommandLineApplication
                {
                    Name = "dotnet aries",
                    FullName = "Aries document generate Command Line Tool",
                    Description = "AriesDoc is help we generate raml doc file form asp.net core.",
                };

                app.Command("doc", command =>
                {
                    command.Description = app.Description;
                    var directory = command.Option(
                        "-t", "The raml doc files where to save.", CommandOptionType.SingleValue);

                    var dllDirectory = command.Option(
                        "-f", "The publish dll directory.", CommandOptionType.SingleValue);

                    var startupName = command.Option(
                        "-s", "The startup class name.", CommandOptionType.SingleValue);

                    var baseUrl = command.Option(
                        "-b", "The base url.", CommandOptionType.SingleValue);

                    var version = command.Option(
                        "-v", "The raml version.", CommandOptionType.SingleValue);

                    command.OnExecute(() =>
                    {
                        if (string.IsNullOrWhiteSpace(dllDirectory.Value())
                            || string.IsNullOrWhiteSpace(directory.Value()))
                        {
                            Console.WriteLine("Invalid doc directory or dllDirectory");
                            app.ShowHelp();
                            return 2;
                        }

                        Generate(dllDirectory.Value(), directory.Value(),
                            startupName.HasValue() ? startupName.Value() : "Startup",
                            baseUrl.Value(), version.Value());
                        Console.WriteLine("Aries doc generate Done.");
                        return 0;
                    });
                });

                app.OnExecute(() =>
                {
                    app.ShowHelp();
                    return 2;
                });

                return app.Execute(args);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return 1;
            }
        }

        public static void Generate(string dllDirectory, string docDirectory, string startupName,
            string baseUri, string version)
        {
            var assemblies = Directory.EnumerateFiles(dllDirectory)
                .Where(i => i.EndsWith(".dll"))
                .Select(i => AssemblyLoadContext.Default.LoadFromAssemblyPath(i))
                .ToArray();
            var startupType = assemblies.SelectMany(i => i.ExportedTypes)
                .FirstOrDefault(x => string.Equals(x.Name, startupName));
            var webHostBuilder = WebHost.CreateDefaultBuilder(new string[0])
                .UseStartup(startupType)
                .ConfigureServices(services =>
                {
                    services.AddMvcCore(o => o.SetApiExplorerVisible());
                    if (version == "0.8")
                        services.AddRAMLDocGeneratorV08();
                    else
                        services.AddRAMLDocGeneratorV10();
                })
                .UseUrls(baseUri)
                .Build()
                .GeneratorDoc(docDirectory, baseUri);
        }
    }
}