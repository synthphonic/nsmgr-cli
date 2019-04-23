using System.IO;
using System.Xml;
using NugetPckgUpdater.Core.Configurations;
using NugetPckgUpdater.Core.Models;

namespace NugetPckgUpdater.Core
{
    public class ProjectTypeManager
    {
        private string _file;

        public ProjectTypeManager(string file)
        {
            _file = file;
        }

        public ProjectType ProjectType()
        {
            var projectType = CheckForNativeProject();

            if (projectType == Models.ProjectType.Unknown)
            {
                projectType = CheckForNETStandardroject();
            }

            return projectType;
        }

        private ProjectType CheckForNativeProject()
        {
            var xmlContent = string.Empty;
            using (var fs = File.OpenRead(_file))
            {
                using (var sr = new StreamReader(fs))
                {
                    xmlContent = sr.ReadToEnd();
                }
            }

            var doc = new XmlDocument();
            doc.LoadXml(xmlContent);

            var foundTaggedElements = doc.DocumentElement.GetElementsByTagName("ProjectTypeGuids");
            if (foundTaggedElements.Count == 0)
                return Models.ProjectType.Unknown;

            var iosProject = foundTaggedElements[0].InnerText.Contains(VisualStudioSettings.iOSTypeGuid);
            if (iosProject)
                return Models.ProjectType.NativeiOS;

            var androidProject = foundTaggedElements[0].InnerText.Contains(VisualStudioSettings.AndroidTypeGuid);
            if (androidProject)
                return Models.ProjectType.NativeAndroid;

            return Models.ProjectType.Unknown;
        }

        private ProjectType CheckForNETStandardroject()
        {
            var xmlContent = string.Empty;
            using (var fs = File.OpenRead(_file))
            {
                using (var sr = new StreamReader(fs))
                {
                    xmlContent = sr.ReadToEnd();
                }
            }

            var doc = new XmlDocument();
            doc.LoadXml(xmlContent);

            var root = doc.DocumentElement;
            var sdkValue = root.GetAttribute("Sdk");
            if (sdkValue.Equals("Microsoft.NET.Sdk"))
            {
                var element = root.GetElementsByTagName("TargetFramework");
                var enumerator = element.GetEnumerator();
                while(enumerator.MoveNext())
                {
                    var xmlElement = enumerator.Current as XmlElement;
                    if (xmlElement != null)
                    {
                        var adsa = xmlElement.InnerText;

                        return NETStandardVersion.Get(xmlElement.InnerText);
                    }
                }
            }

            return Models.ProjectType.Unknown;
        }
    }
}
