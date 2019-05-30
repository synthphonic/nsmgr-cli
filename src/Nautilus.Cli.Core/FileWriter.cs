using System;
using System.IO;
using System.Xml;
using Nautilus.Cli.Core.Models;

namespace Nautilus.Cli.Core
{
	public class FileWriter
	{
		private readonly ProjectMetadata _metadata;
		private string XmlNameSpace = "http://schemas.microsoft.com/developer/msbuild/2003";

		private FileWriter()
		{
			//_fileReaders = new Dictionary<ProjectTarget, IProjectFilePackageReader>
			//{
			//	//[SolutionProjectElement.PackageConfig] = new PackageConfigFileReader(),
			//	//[SolutionProjectElement.iOS] = new CSharpNativeProjectFileReader(),
			//	//[SolutionProjectElement.Android] = new CSharpNativeProjectFileReader()
			//	[ProjectTarget.NETFramework46] = new PackageConfigFileReader(),
			//	[ProjectTarget.NETStandard20] = new CSharpProjectFileReader(),
			//	[ProjectTarget.NETCoreApp20] = new CSharpProjectFileReader(),
			//	[ProjectTarget.NETCoreApp21] = new CSharpProjectFileReader(),
			//	[ProjectTarget.NativeiOS] = new CSharpNativeProjectFileReader(),
			//	[ProjectTarget.NativeiOSBinding] = new CSharpNativeProjectFileReader(),
			//	[ProjectTarget.NativeAndroid] = new CSharpNativeProjectFileReader()
			//};
		}

		public FileWriter(Project project) : this()
		{
			_metadata = project.Metadata;
		}

		public bool UpdateNugetPackage(string packageName, string version)
		{
			var xmlDoc = new XmlDocument();
			xmlDoc.Load(_metadata.ProjectFullPath);

			//var nt = new NameTable();
			var nsMgr = new XmlNamespaceManager(xmlDoc.NameTable);
			nsMgr.AddNamespace("namespace", XmlNameSpace);

			var xpathString = $"//namespace:PackageReference[@Include='{packageName}']";
			try
			{
				var node = xmlDoc.DocumentElement.SelectSingleNode(xpathString, nsMgr);
				var versionNode = node.FirstChild.FirstChild;
				versionNode.Value = version;

				var fileInfo = new FileInfo(_metadata.ProjectFullPath);
				var fullBackupFile = Path.Combine(fileInfo.DirectoryName, $"{fileInfo.Name}.backup");

				// Copy original file as backup
				File.Copy(_metadata.ProjectFullPath, fullBackupFile, true);

				// save edited content to original file
				xmlDoc.Save(_metadata.ProjectFullPath);

				return true;
			}
			catch (InvalidOperationException)
			{
				return false;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}