namespace Nautilus.SolutionExplorer.Core.Utils;

public class ProjectTargetFrameworkExtractor
{
    private string _projectFilePath;

    public ProjectTargetFrameworkExtractor(string projectFilePath)
    {
        _projectFilePath = projectFilePath;
    }

    public ProjectTargetFramework GetTargetFramework()
    {        
        var projectTargetFramework = CheckForNativeProject();

        if (projectTargetFramework == ProjectTargetFramework.Unknown)
        {
            projectTargetFramework = CheckForNETStandardProject();
        }

        if (projectTargetFramework == ProjectTargetFramework.Unknown)
        {
            projectTargetFramework = CheckForNETFrameworkProject();
        }

        if (projectTargetFramework == ProjectTargetFramework.Unknown)
        {
            projectTargetFramework = CheckForNET5orNET6Project();
        }

        return projectTargetFramework;
    }

    private ProjectTargetFramework CheckForNET5orNET6Project()
    {
        if (!File.Exists(_projectFilePath))
        {
            return ProjectTargetFramework.Unknown;
        }

        // TODO: replace this code with FileUtil.ReadFileContent
        var xmlContent = string.Empty;
        using (var fs = File.OpenRead(_projectFilePath))
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

        /*
            Potential support for Sdk element
                Microsoft.NET.Sdk
                Microsoft.NET.Sdk.Web
                Microsoft.NET.Sdk.BlazorWebAssembly
                Microsoft.NET.Sdk.Razor
                more to come
         */
        if (sdkValue.Contains("Microsoft.NET.Sdk"))
        {
            var element = root.GetElementsByTagName("TargetFramework");
            var enumerator = element.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var xmlElement = enumerator.Current as XmlElement;
                if (xmlElement != null)
                {
                    var adsa = xmlElement.InnerText;

                    return TargetFrameworkSetting.Get(xmlElement.InnerText);
                }
            }
        }

        return ProjectTargetFramework.Unknown;
    }

    private ProjectTargetFramework CheckForNETFrameworkProject()
    {
        if (!File.Exists(_projectFilePath))
        {
            return ProjectTargetFramework.Unknown;
        }

        var xmlContent = string.Empty;
        using (var fs = File.OpenRead(_projectFilePath))
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
        if (xmlNode == null)
            return ProjectTargetFramework.Unknown;

        var frameworkVersion = xmlNode.InnerText;

        if (!string.IsNullOrWhiteSpace(frameworkVersion))
        {
            if (frameworkVersion.Contains("2.0"))
                return ProjectTargetFramework.NETFramework20;
            if (frameworkVersion.Contains("3.5"))
                return ProjectTargetFramework.NETFramework35;
            if (frameworkVersion.Contains("4.0"))
                return ProjectTargetFramework.NETFramework40;
            if (frameworkVersion.Contains("4.5"))
                return ProjectTargetFramework.NETFramework45;
            if (frameworkVersion.Contains("4.6"))
                return ProjectTargetFramework.NETFramework46;
            if (frameworkVersion.Contains("4.7"))
                return ProjectTargetFramework.NETFramework47;
            if (frameworkVersion.Contains("4.8"))
                return ProjectTargetFramework.NETFramework48;
        }

        return ProjectTargetFramework.Unknown;
    }

    private ProjectTargetFramework CheckForNativeProject()
    {
        if (!File.Exists(_projectFilePath))
        {
            return ProjectTargetFramework.Unknown;
        }

        var xmlContent = string.Empty;
        using (var fs = File.OpenRead(_projectFilePath))
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
            return ProjectTargetFramework.Unknown;

        var iosProject = foundTaggedElements[0].InnerText.Contains(VisualStudioProjectSetting.iOSTypeGuid);
        if (iosProject)
            return ProjectTargetFramework.NativeiOS;

        var iosNativeProject = foundTaggedElements[0].InnerText.Contains(VisualStudioProjectSetting.iOSBindingTypeGuid);
        if (iosNativeProject)
            return ProjectTargetFramework.NativeiOSBinding;

        var androidProject = foundTaggedElements[0].InnerText.Contains(VisualStudioProjectSetting.AndroidTypeGuid);
        if (androidProject)
            return ProjectTargetFramework.NativeAndroid;

        var uwpProject = foundTaggedElements[0].InnerText.Contains(VisualStudioProjectSetting.UWPTypeGuid);
        if (uwpProject)
            return ProjectTargetFramework.NativeUWP;

        return ProjectTargetFramework.Unknown;
    }

    private ProjectTargetFramework CheckForNETStandardProject()
    {
        if (!File.Exists(_projectFilePath))
        {
            return ProjectTargetFramework.Unknown;
        }

        var xmlContent = string.Empty;
        using (var fs = File.OpenRead(_projectFilePath))
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
            while (enumerator.MoveNext())
            {
                var xmlElement = enumerator.Current as XmlElement;
                if (xmlElement != null)
                {
                    var adsa = xmlElement.InnerText;

                    return TargetFrameworkSetting.Get(xmlElement.InnerText);
                }
            }
        }

        return ProjectTargetFramework.Unknown;
    }
}
