using System;
using System.IO;
using System.Linq;
using JetBrains.ProjectModel;
using NuGet;

namespace ReSharperPlugin.UnrealEditor.PluginDelivery
{
    [SolutionComponent]
    public class PluginDetector
    {
        private readonly ISolution mySolution;
        private readonly ILogger myLogger;


        public PluginDetector(ISolution solution, ILogger logger, IsUnrealSolution isUnrealSolution )
        {
            if (isUnrealSolution.myValidUnrealSolution) return;
            mySolution = solution;
            myLogger = logger;
        }
    }
}