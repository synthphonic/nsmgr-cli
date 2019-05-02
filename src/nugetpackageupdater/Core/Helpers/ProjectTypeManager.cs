using System.IO;
using System.Xml;
using NugetPckgUpdater.Core.Configurations;
using SolutionNugetPackagesUpdater.Core.Configurations.Enums;

namespace SolutionNugetPackagesUpdater.Core.Helpers
{
	public class ProjectTypeManager
    {
        private string _file;

        public ProjectTypeManager(string file)
        {
            _file = file;
        }

        public ProjectTarget GetTargetFramework()
        {
            var projectType = CheckForNativeProject();

            if (projectType == ProjectTarget.Unknown)
			{
                projectType = CheckForNETStandardProject();
            }

			if (projectType == ProjectTarget.Unknown)
			{
				projectType = CheckForNETFrameworkProject();
			}

            return projectType;
        }

		private ProjectTarget CheckForNETFrameworkProject()
		{
			if (!File.Exists(_file))
			{
				return ProjectTarget.Unknown;
			}

			var xmlContent = string.Empty;
			using (var fs = File.OpenRead(_file))
			{
				using (var sr = new StreamReader(fs))
				{
					xmlContent = sr.ReadToEnd();
				}
			}

			var xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xmlContent);

			var xmlNsManager = new XmlNamespaceManager(xmlDoc.NameTable);
			xmlNsManager.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003");

			var xmlNode = xmlDoc.SelectSingleNode("//x:TargetFrameworkVersion", xmlNsManager);
			var frameworkVersion = xmlNode.InnerText;

			if (!string.IsNullOrWhiteSpace(frameworkVersion))
			{
				if (frameworkVersion.Contains("4.5"))
					return ProjectTarget.NETFramework45;
				if (frameworkVersion.Contains("4.6"))
					return ProjectTarget.NETFramework46;
				if (frameworkVersion.Contains("4.7"))
					return ProjectTarget.NETFramework47;
				if (frameworkVersion.Contains("4.8"))
					return ProjectTarget.NETFramework48;
			}

			return ProjectTarget.Unknown;
		}

        private ProjectTarget CheckForNativeProject()
        {
			if (!File.Exists(_file))
			{
				return ProjectTarget.Unknown;
			}

            var xmlContent = string.Empty;
            using (var fs = File.OpenRead(_file))
            {
                using (var sr = new StreamReader(fs))
                {
                    xmlContent = sr.ReadToEnd();
                }
            }

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);

            var foundTaggedElements = xmlDoc.DocumentElement.GetElementsByTagName("ProjectTypeGuids");
			if (foundTaggedElements.Count == 0)
				return ProjectTarget.Unknown;

            var iosProject = foundTaggedElements[0].InnerText.Contains(VisualStudioProjectSetting.iOSTypeGuid);
            if (iosProject)
                return ProjectTarget.NativeiOS;

			var iosNativeProject = foundTaggedElements[0].InnerText.Contains(VisualStudioProjectSetting.iOSBindingTypeGuid);
			if (iosNativeProject)
				return ProjectTarget.NativeiOSBinding;

			var androidProject = foundTaggedElements[0].InnerText.Contains(VisualStudioProjectSetting.AndroidTypeGuid);
            if (androidProject)
                return ProjectTarget.NativeAndroid;

			return ProjectTarget.Unknown;
		}

        private ProjectTarget CheckForNETStandardProject()
		{
			if (!File.Exists(_file))
			{
				return ProjectTarget.Unknown;
			}

			var xmlContent = string.Empty;
            using (var fs = File.OpenRead(_file))
            {
                using (var sr = new StreamReader(fs))
                {
                    xmlContent = sr.ReadToEnd();
                }
            }

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);

            var root = xmlDoc.DocumentElement;
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

            return ProjectTarget.Unknown;
        }
    }
}
