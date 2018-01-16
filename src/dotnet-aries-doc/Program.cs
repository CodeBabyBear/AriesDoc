using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.DependencyModel.Resolution;
using Newtonsoft.Json;
using Pandv.AriesDoc.Generator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
                             IsRelativePath = false
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

                        Generate(docConfig.PublishDllDirectory, docConfig.DocDirectory,
                            docConfig.StartupClassName,
                            docConfig.BaseUrl, docConfig.RamlVersion);
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
                //.Select(i => AssemblyLoadContext.Default.LoadFromAssemblyPath(i))
                .Select(i => new AssemblyResolver(i).Assembly)
                .ToArray();
            assemblies.ToList().ForEach(i => DependencyContext.Load(i));
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

    internal sealed class AssemblyResolver : IDisposable
    {
        private readonly ICompilationAssemblyResolver assemblyResolver;
        private readonly DependencyContext dependencyContext;
        private readonly AssemblyLoadContext loadContext;

        public AssemblyResolver(string path)
        {
            this.Assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
            this.dependencyContext = DependencyContext.Load(this.Assembly);

            this.assemblyResolver = new CompositeCompilationAssemblyResolver
                                    (new ICompilationAssemblyResolver[]
            {
            new AppBaseCompilationAssemblyResolver(Path.GetDirectoryName(path)),
            new ReferenceAssemblyPathResolver(),
            new PackageCompilationAssemblyResolver()
            });

            this.loadContext = AssemblyLoadContext.GetLoadContext(this.Assembly);
            this.loadContext.Resolving += OnResolving;
        }

        public Assembly Assembly { get; }

        public void Dispose()
        {
            this.loadContext.Resolving -= this.OnResolving;
        }

        private Assembly OnResolving(AssemblyLoadContext context, AssemblyName name)
        {
            bool NamesMatch(RuntimeLibrary runtime)
            {
                return string.Equals(runtime.Name, name.Name, StringComparison.OrdinalIgnoreCase);
            }

            RuntimeLibrary library =
                this.dependencyContext.RuntimeLibraries.FirstOrDefault(NamesMatch);
            if (library != null)
            {
                var wrapper = new CompilationLibrary(
                    library.Type,
                    library.Name,
                    library.Version,
                    library.Hash,
                    library.RuntimeAssemblyGroups.SelectMany(g => g.AssetPaths),
                    library.Dependencies,
                    library.Serviceable);

                var assemblies = new List<string>();
                this.assemblyResolver.TryResolveAssemblyPaths(wrapper, assemblies);
                if (assemblies.Count > 0)
                {
                    return this.loadContext.LoadFromAssemblyPath(assemblies[0]);
                }
            }

            return null;
        }
    }
}