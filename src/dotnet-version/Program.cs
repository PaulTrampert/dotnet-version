using System;
using System.IO;
using Microsoft.DotNet.ProjectModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace dotnet_version
{
    public class Program
    {
        private static readonly ILogger Logger = new LoggerFactory().AddConsole().CreateLogger<Program>();
        public static void Main(string[] args)
        {
            if (args.Length == 0 || string.Equals(args[0], "-help", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Usage: dotnet version <VERSION_STRING>");
                return;
            }
            try
            {
                var projectFilePath =
                    $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{Project.FileName}";
                Logger.LogInformation($"Loading project from {projectFilePath}...");
                dynamic projectFile = JsonConvert.DeserializeObject(File.ReadAllText(projectFilePath));
                Logger.LogInformation($"Project loaded.");
                projectFile.version = args[0];
                Logger.LogInformation($"Version set to {args[0]}");
                File.WriteAllText(projectFilePath, JsonConvert.SerializeObject(projectFile, Formatting.Indented));
                Logger.LogInformation($"Project saved.");
            }
            catch (FileNotFoundException e)
            {
                Logger.LogError(new EventId(1, "Project file not found"), e, "The project file was not found. Make sure you are running this command from the project root.");
            }
            catch (Exception e)
            {
                Logger.LogError(new EventId(-1, "Unknown Error"), e, "Unexpected exception occurred.");
            }
        }
    }
}
