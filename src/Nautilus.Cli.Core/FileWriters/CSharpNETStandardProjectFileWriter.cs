using System.IO;
using System.Xml;
using Nautilus.Cli.Core.Abstraction;
using Nautilus.Cli.Core.Configurations.Enums;
using Nautilus.Cli.Core.Models;

namespace Nautilus.Cli.Core.FileWriters
{
	public class CSharpNETStandardProjectFileWriter : IProjectFilePackageWriter
	{
		private ProjectMetadata _projectMetadata;
		private string _projectFullPath;

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

			var xpathString = $"//PackageReference[@Include='{packageName}']";
			var node = xmlDoc.DocumentElement.SelectSingleNode(xpathString);
			var versionNode = node.Attributes["Version"];
			versionNode.Value = newVersion;

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
