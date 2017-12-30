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
    public class Student
    {
        public string Name { get; set; }
    }

    public class Program
    {
        public string Name { get; set; }

        public Student Student { get; set; }

        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .GeneratorDoc(Directory.GetCurrentDirectory())
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
