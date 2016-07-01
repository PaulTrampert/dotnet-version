using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.DotNet.ProjectModel;
using Newtonsoft.Json;
using NuGet.Frameworks;
using NuGet.Versioning;

namespace dotnet_version
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var projectFilePath = $"{Directory.GetCurrentDirectory()}\\{Project.FileName}";
            dynamic projectFileText = JsonConvert.DeserializeObject(File.ReadAllText(projectFilePath));
            projectFileText.version = args[0];
            File.WriteAllText(projectFilePath, JsonConvert.SerializeObject(projectFileText, Formatting.Indented));
        }
    }
}
