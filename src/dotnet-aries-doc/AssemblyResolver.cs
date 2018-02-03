using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.DependencyModel.Resolution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Pandv.AriesDoc
{
    internal sealed class AssemblyResolver : IDisposable
    {
        private readonly ICompilationAssemblyResolver assemblyResolver;
        public readonly Assembly[] Assemblies;
        private readonly DependencyContext[] dependencyContexts;

        public AssemblyResolver(string path)
        {
            Assemblies = Directory.EnumerateFiles(path)
                .Where(i => i.EndsWith(".dll"))
                .Select(i => AssemblyLoadContext.Default.LoadFromAssemblyPath(i))
                .ToArray();

            this.assemblyResolver = new CompositeCompilationAssemblyResolver
                                    (new ICompilationAssemblyResolver[]
            {
            new AppBaseCompilationAssemblyResolver(Path.GetDirectoryName(path)),
            new ReferenceAssemblyPathResolver(),
            new PackageCompilationAssemblyResolver()
            });
            AssemblyLoadContext.Default.Resolving += OnResolving;
            dependencyContexts = Assemblies.Select(i => DependencyContext.Load(i))
                .Where(i => i != null).ToArray();
        }

        public void Dispose()
        {
            AssemblyLoadContext.Default.Resolving -= OnResolving;
        }

        private Assembly OnResolving(AssemblyLoadContext context, AssemblyName name)
        {
            bool NamesMatch(RuntimeLibrary runtime)
            {
                return string.Equals(runtime.Name, name.Name, StringComparison.OrdinalIgnoreCase);
            }

            var assembly = Assemblies.FirstOrDefault(i => string.Equals(i.GetName().Name, name.Name, StringComparison.OrdinalIgnoreCase));
            if (assembly == null)
            {
                assembly = dependencyContexts.Select(i =>
                {
                    RuntimeLibrary library = i.RuntimeLibraries.FirstOrDefault(NamesMatch);
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
                            return AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblies[0]);
                        }
                        else
                            return null;
                    }
                    else
                        return null;
                }).FirstOrDefault(i => i != null);
            }
            return assembly;
        }
    }
}