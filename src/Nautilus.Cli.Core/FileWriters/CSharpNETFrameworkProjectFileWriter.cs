using System.IO;
using System.Xml;
using Nautilus.Cli.Core.Abstraction;
using Nautilus.Cli.Core.Configurations.Enums;
using Nautilus.Cli.Core.Models;

namespace Nautilus.Cli.Core.FileWriters
{
	public class CSharpNETFrameworkProjectFileWriter : IProjectFilePackageWriter
	{
		private ProjectMetadata _projectMetadata;
		private string _projectFullPath;
		private const string XmlNameSpace = "http://schemas.microsoft.com/developer/msbuild/2003";

		#region IProjectFilePackageWriter implementations
		public void Initialize(ProjectTarget targetFramework, ProjectMetadata projectMetadata)
		{
			_projectMetadata = projectMetadata;

			_projectFullPath = _projectMetadata.ProjectFullPath;
		}

		public void UpdatePackageReference(string packageName, string newVersion)
		{
			var xmlDoc = new XmlDocument();
			xmlDoc.Load(_projectFullPath);

			//var nt = new NameTable();
			var nsMgr = new XmlNamespaceManager(xmlDoc.NameTable);
			nsMgr.AddNamespace("namespace", XmlNameSpace);

			var xpathString = $"//namespace:PackageReference[@Include='{packageName}']";
			var node = xmlDoc.DocumentElement.SelectSingleNode(xpathString, nsMgr);

            // code below is used for old csproj xml file format
            //var versionNode = node.FirstChild.FirstChild;
            XmlNode versionNode = node.Attributes["Version"];
            if (versionNode != null)
            {
                versionNode.Value = newVersion;
            }
            else
            {
                var versionXpathString = $"//namespace:Version";
                versionNode = node.SelectSingleNode(versionXpathString, nsMgr);
                versionNode.InnerText = newVersion;
                //versionNode.Value = newVersion;
            }

			var fileInfo = new FileInfo(_projectFullPath);
			var fullBackupFile = Path.Combine(fileInfo.DirectoryName, $"{fileInfo.Name}.backup");

			// Copy original file as backup
			File.Copy(_projectFullPath, fullBackupFile, true);

			// save edited content to original file
			xmlDoc.Save(_projectFullPath);
		}
		#endregion
	}
}