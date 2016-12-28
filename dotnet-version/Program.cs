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

                var paths = target.Argument("path", "If specified, the relative path(s) to the project(s)", true);

                target.OnExecute(() =>
                {
                    var pathList = paths.Values;
                    if (newVersion.HasValue() == envVar.HasValue())
                    {
                        Logger.LogError("--new-version or --env-var must be set, not both.");
                        return 1;
                    }
                    var versionString = envVar.HasValue() ? Environment.GetEnvironmentVariable(envVar.Value()) : newVersion.Value();
                    if(!string.IsNullOrWhiteSpace(versionString))
                        SetVersion(versionString, pathList);
                    return 0;
                });
            });

            app.Command("read", target =>
            {

                var paths = target.Argument("path", "If specified, the relative path to the project", false);
                target.HelpOption("-? | -help");
                target.OnExecute(() =>
                {
                    ReadVersion(paths.Values);
                    return 0;
                });
            });

            app.HelpOption("-? | -help");

            return app.Execute(args);
        }

        private static IEnumerable<string> ProjectFiles(IEnumerable<string> paths)
        {
            if (!paths.Any())
            {
                var localPath = Path.Combine(Directory.GetCurrentDirectory(), Project.FileName);
                if (File.Exists(localPath))
                {
                    yield return localPath;
                }
            }
            else
            {
                var fullPaths = paths.Select(x => Path.GetFullPath(x));
                var directToProjectFiles = fullPaths.Where(x => Path.GetFileName(x).Equals(Project.FileName));
                var folderPaths = fullPaths.Where(x => !directToProjectFiles.Contains(x)).Where(x => Directory.Exists(x));
                var folderProjectPaths = folderPaths.Select(x => Path.Combine(x, Project.FileName));
                var foundProjects = directToProjectFiles.Union(folderProjectPaths).Where(x=> File.Exists(x));
                foreach(var p in foundProjects)
                {
                    yield return p;
                }
            }
        }

        private static void ReadVersion(IEnumerable<string> paths)
        {
            var projectFilePath = ProjectFiles(paths).FirstOrDefault();
            
            dynamic projectFile = JsonConvert.DeserializeObject(File.ReadAllText(projectFilePath));
            Console.WriteLine(projectFile.version);
        }

        public static void SetVersion(string newVersion, IEnumerable<string> paths)
        {
            //lets figure out the correct paths from the paths list
            foreach (var projectFilePath in ProjectFiles(paths))
            {
                dynamic projectFile = JsonConvert.DeserializeObject(File.ReadAllText(projectFilePath));
                projectFile.version = $"{newVersion}-*";
                File.WriteAllText(projectFilePath, JsonConvert.SerializeObject(projectFile, Formatting.Indented));
            }
        }
    }
}
