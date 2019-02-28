﻿namespace SolutionNugetPackagesUpdater.Models
{
    public class Configuration
    {
        public string ParentSearchPath { get; set; }
        public string[] ExcludeFoldersPattern { get; set; }
        public string[] SearchFilePatterns { get; set; }

        public string OutputFile { get; set; }
        public static string FileName { get; } = "app_config.json";
    }
}