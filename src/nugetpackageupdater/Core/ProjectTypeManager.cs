using System.IO;
using System.Xml;
using NugetPckgUpdater.Core.Configurations;
using SolutionNugetPackagesUpdater.Core.Configurations.Enums;

namespace NugetPckgUpdater.Core
{
	public class ProjectTypeManager
    {
        private string _file;

        public ProjectTypeManager(string file)
        {
            _file = file;
        }

        public TargetFramework ProjectType()
        {
            var projectType = CheckForNativeProject();

            if (projectType == TargetFramework.Unknown)
			{
                projectType = CheckForNETStandardProject();
            }

            return projectType;
        }

        private TargetFramework CheckForNativeProject()
        {
			if (!File.Exists(_file))
			{
				return TargetFramework.Unknown;
			}

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
				return TargetFramework.Unknown;

            var iosProject = foundTaggedElements[0].InnerText.Contains(VisualStudioProjectSetting.iOSTypeGuid);
            if (iosProject)
                return TargetFramework.NativeiOS;

			var iosNativeProject = foundTaggedElements[0].InnerText.Contains(VisualStudioProjectSetting.iOSBindingTypeGuid);
			if (iosNativeProject)
				return TargetFramework.NativeiOSBinding;

			var androidProject = foundTaggedElements[0].InnerText.Contains(VisualStudioProjectSetting.AndroidTypeGuid);
            if (androidProject)
                return TargetFramework.NativeAndroid;

			return TargetFramework.Unknown;
		}

        private TargetFramework CheckForNETStandardProject()
		{
			if (!File.Exists(_file))
			{
				return TargetFramework.Unknown;
			}

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

                        return TargetFrameworkSetting.Get(xmlElement.InnerText);
                    }
                }
            }

            return TargetFramework.Unknown;
        }
    }
}
