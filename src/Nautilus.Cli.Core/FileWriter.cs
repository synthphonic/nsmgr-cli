using System.Collections.Generic;
using Nautilus.Cli.Core.Abstraction;
using Nautilus.Cli.Core.Configurations.Enums;
using Nautilus.Cli.Core.FileWriters;
using Nautilus.Cli.Core.Models;

namespace Nautilus.Cli.Core
{
	public class FileWriter
	{
		private readonly Dictionary<ProjectTarget, IProjectFilePackageWriter> _fileWriters;
		private readonly Project _project;

		private FileWriter()
		{
			_fileWriters = new Dictionary<ProjectTarget, IProjectFilePackageWriter>
			{
				[ProjectTarget.NETStandard20] = new CSharpNETStandardProjectFileWriter(),
				[ProjectTarget.NativeAndroid] = new CSharpNETFrameworkProjectFileWriter(),
				[ProjectTarget.NativeiOS] = new CSharpNETFrameworkProjectFileWriter(),
                [ProjectTarget.NativeUWP] = new CSharpNETFrameworkProjectFileWriter(),
                [ProjectTarget.NETFramework] = new CSharpNETFrameworkProjectFileWriter(),
				[ProjectTarget.NETFramework46] = new PackageConfigFileWriter()
			};
		}

		public FileWriter(Project project) : this()
		{
			_project = project;
		}

		public bool UpdateNugetPackage(string packageName, string version)
		{
			IProjectFilePackageWriter fileWriter = null;

			if (_project.TargetFramework == ProjectTarget.NETFramework46)
			{
				if (_project.PackagesConfigFileExist)
				{
					fileWriter = _fileWriters[_project.TargetFramework];
				}
				else
				{
					fileWriter = _fileWriters[ProjectTarget.NETFramework];
				}
			}
			else
			{
				fileWriter = _fileWriters[_project.TargetFramework];
			}

			fileWriter.Initialize(_project.TargetFramework, _project.Metadata);
			fileWriter.UpdatePackageReference(packageName, version);

			return true;
		}
	}
}