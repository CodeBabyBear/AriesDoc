using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Pandv.AriesDoc.Generator;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Example
{
    public class Program
    {
        public string Name { get; set; }

        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            var generator = host.Services.GetRequiredService<IDocGenerator>();
            var a = generator.Generate().Select(i => i.Serialize()).ToArray();
            //.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
