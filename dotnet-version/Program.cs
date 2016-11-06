using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.DotNet.ProjectModel;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace dotnet_version
{
    public class Program
    {
        private static readonly ILogger Logger = new LoggerFactory().AddConsole().CreateLogger<Program>();

        public static int Main(params string[] args)
        {
            var app = new CommandLineApplication();
            app.Command("set", target =>
            {
                var newVersion = target.Option("--new-version", 
                    "If specified, the new version to set in the project.json. Version must be in semver format (major.minor.patch).", 
                    CommandOptionType.SingleValue);
                var envVar = target.Option("--env-var",
                    "If specified, the environment variable contianing the new version. Version must be in semver format (major.minor.patch).",
                    CommandOptionType.SingleValue);

                target.HelpOption("-? | -help");

                target.OnExecute(() =>
                {
                    if (newVersion.HasValue() == envVar.HasValue())
                    {
                        Logger.LogError("--new-version or --env-var must be set, not both.");
                        return 1;
                    }
                    var versionString = envVar.HasValue() ? Environment.GetEnvironmentVariable(envVar.Value()) : newVersion.Value();
                    if(!string.IsNullOrWhiteSpace(versionString))
                        SetVersion(versionString);
                    return 0;
                });
            });

            app.Command("read", target =>
            {
                target.HelpOption("-? | -help");
                target.OnExecute(() =>
                {
                    ReadVersion();
                    return 0;
                });
            });

            app.HelpOption("-? | -help");

            return app.Execute(args);
        }

        private static void ReadVersion()
        {
            var projectFilePath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{Project.FileName}";
            dynamic projectFile = JsonConvert.DeserializeObject(File.ReadAllText(projectFilePath));
            Console.WriteLine(projectFile.version);
        }

        public static void SetVersion(string newVersion)
        {
            var projectFilePath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{Project.FileName}";
            dynamic projectFile = JsonConvert.DeserializeObject(File.ReadAllText(projectFilePath));
            projectFile.version = $"{newVersion}-*";
            File.WriteAllText(projectFilePath, JsonConvert.SerializeObject(projectFile, Formatting.Indented));
        }
    }
}
