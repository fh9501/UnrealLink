using System;
using System.IO;
using System.Linq;
using JetBrains.ProjectModel;

namespace ReSharperPlugin.UnrealEditor
{
    [SolutionComponent]
    public class IsUnrealSolution
    {
        public readonly bool myValidUnrealSolution;
        private string myUE4SourcesDirectory;
        private string myUE4PluginsDirectory;
        private string myUE4DeveloperPluginDirectory;
        private readonly ISolution mySolution;

        public IsUnrealSolution(ISolution solution)
        {
            mySolution = solution;

            foreach (var project in mySolution.GetProjectsByName("UE4"))
            {
                var suffix = "Source\\Runtime\\CoreUObject\\Public\\UObject\\ObjectMacros.h";
                var files = project.GetAllProjectFiles(file => file.Location.FullPath.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)).ToList();
                if (files.Count != 1) 
                    continue;

                var fullPath = files.Single().Location.FullPath;
                myUE4SourcesDirectory = fullPath.Substring(0, fullPath.Length - suffix.Length);
                myUE4PluginsDirectory = Path.Combine(myUE4SourcesDirectory, "Plugins");
                myUE4DeveloperPluginDirectory = Path.Combine(myUE4PluginsDirectory, "Development");
                myValidUnrealSolution = Directory.Exists(myUE4SourcesDirectory) &&
                                        Directory.Exists(myUE4PluginsDirectory) &&
                                        Directory.Exists(myUE4DeveloperPluginDirectory);
            }
        }
    }
}