using System;
using System.IO;
using Microsoft.DotNet.ProjectModel;
using Newtonsoft.Json;

namespace dotnet_version
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var projectFilePath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{Project.FileName}";
            dynamic projectFileText = JsonConvert.DeserializeObject(File.ReadAllText(projectFilePath));
            projectFileText.version = args[0];
            File.WriteAllText(projectFilePath, JsonConvert.SerializeObject(projectFileText, Formatting.Indented));
        }
    }
}
