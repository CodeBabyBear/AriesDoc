using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Pandv.AriesDoc.Generator;
using System;
using System.IO;
using System.Linq;

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
                    var config = command.Option(
                        "-c", "The config file.", CommandOptionType.SingleValue);

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

                    var xml = command.Option(
                        "-x", "The xml comments file.", CommandOptionType.SingleValue);

                    command.OnExecute(() =>
                    {
                        var docConfig = !string.IsNullOrWhiteSpace(config.Value())
                         ? JsonConvert.DeserializeObject<DocConfig>(File.ReadAllText(config.Value()))
                         : new DocConfig()
                         {
                             RamlVersion = version.Value(),
                             StartupClassName = startupName.HasValue() ? startupName.Value() : "Startup",
                             BaseUrl = baseUrl.Value(),
                             DocDirectory = directory.Value(),
                             PublishDllDirectory = dllDirectory.Value(),
                             IsRelativePath = false,
                             XmlCommentsFile = xml.Value()
                         };

                        if (string.IsNullOrWhiteSpace(docConfig.PublishDllDirectory)
                            || string.IsNullOrWhiteSpace(docConfig.DocDirectory))
                        {
                            Console.WriteLine("Invalid doc directory or dllDirectory");
                            app.ShowHelp();
                            return 2;
                        }

                        if (docConfig.IsRelativePath)
                        {
                            var current = Directory.GetCurrentDirectory();
                            docConfig.PublishDllDirectory = Path.Combine(current, docConfig.PublishDllDirectory);
                            docConfig.DocDirectory = Path.Combine(current, docConfig.DocDirectory);
                        }
                        Generate(docConfig);
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

        private static void Generate(DocConfig docConfig)
        {
            var startupType = new AssemblyResolver(docConfig.PublishDllDirectory).Assemblies
                .SelectMany(i => i.ExportedTypes)
                .FirstOrDefault(x => string.Equals(x.Name, docConfig.StartupClassName));
            var webHostBuilder = WebHost.CreateDefaultBuilder(new string[0])
                .UseStartup(startupType)
                .ConfigureServices(services =>
                {
                    services.AddMvcCore(o => o.SetApiExplorerVisible());
                    if (docConfig.RamlVersion == "0.8")
                        services.AddRAMLDocGeneratorV08();
                    else
                        services.AddRAMLDocGeneratorV10();
                    services.AddXmlComments(docConfig.XmlCommentsFile);
                })
                .UseUrls(docConfig.BaseUrl)
                .Build()
                .GeneratorDoc(docConfig.DocDirectory, docConfig.BaseUrl);
        }
    }
}