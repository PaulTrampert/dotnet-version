# dotnet-version
Tool to update the version of a project.json.

## Usage

Include this in the tools section of your project.json. It's available via nuget:
```json
{
  "tools": {
    "dotnet-version": "1.1.0"
  }
}
```

To change the version listed in the project.json from the command line, make sure your working directory is your project root. Then call `dotnet version set --new-version <version_number>`. You can also use an environment variable to set the version by calling `dotnet version set --env-var <variable_name>`. This is particularly useful in the project.json scripts section, where environment variables will not be resolved.

Then from the command prompt, change your working directory to the root of your project. For example if your `project.json` is in `C:\Users\Paul\Project`, you would `cd C:\Users\Paul\Project`. You can then change the version string in the `project.json` with `dotnet version <yourversionnumber>`.

The primary purpose of this is build servers. the build script might do something like this:
```batch
dotnet version 1.3.0
dotnet pack -c Release
git commit -a -m "Release 1.3.0"
git tag 1.3.0
dotnet version 1.3.1-*
git commit -a -m "Next working version"
```
This would set the version to 1.3.0, then build and pack the project.



