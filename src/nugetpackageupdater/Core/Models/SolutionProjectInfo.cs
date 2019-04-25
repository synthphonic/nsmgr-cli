using System;
using System.Diagnostics;

namespace SolutionNugetPackagesUpdater.Core.Models
{
    public class SolutionProjectInfo
    {
        // NOTE:             
        // Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Storiveo.UnitTests", "Storiveo\Storiveo.UnitTests\Storiveo.UnitTests.csproj", "{37DF5F99-DA45-4D54-A8DA-F3EE97703A6E}"

        internal static SolutionProjectInfo Extract(string data)
        {
            Debug.WriteLine(data);

            var split = data.Split(',');

            //const string FindChars = "\\";
            var projectPath = split[1].Trim();

            // \"{510DE4D4-F0EE-479F-B9AC-54F9A3EA6F17}\"
            var projectGuid = split[2].Trim().Replace("{", string.Empty).Replace("}", string.Empty);


            var s = new Guid(projectGuid);

            var ipd = Getaa(split[0]);

            var spi = new SolutionProjectInfo
            {
                ProjectTypeGuid = ipd.ProjectTypeGuid,
                ProjectName = ipd.ProjectName,
                ProjectPath = projectPath,
                ProjectGuid = projectGuid
            };
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
            return spi;
        }

        private static InternalProjectData Getaa(string firstSplit)
        {
            var split = firstSplit.Split(',');
            var cleanup = split[0].Replace("Project(\"{", string.Empty);
            cleanup = cleanup.Replace("}\") =", string.Empty);
            cleanup = cleanup.Substring(0, cleanup.Length - 1);

            //var secondTemplate = cleanup;
            var projectTypeGuid = cleanup.Substring(0, 36);
            var projectName = cleanup.Replace($"{projectTypeGuid} \"", string.Empty);

            var ipd = new InternalProjectData
            {
                ProjectName = projectName,
                ProjectTypeGuid = projectTypeGuid
            };

            return ipd;
        }

        public string ProjectName { get; internal set; }
        public string ProjectTypeGuid { get; internal set; }
        public string ProjectPath { get; internal set; }
        public string ProjectGuid { get; internal set; }

        class InternalProjectData
        {
            internal string ProjectName { get; set; }
            internal string ProjectTypeGuid { get; set; }
        }
    }
}
