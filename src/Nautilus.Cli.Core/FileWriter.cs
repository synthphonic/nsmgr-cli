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
		private readonly ProjectMetadata _metadata;
		private readonly ProjectTarget _targetFramework;

		private FileWriter()
		{
			_fileWriters = new Dictionary<ProjectTarget, IProjectFilePackageWriter>
			{
				[ProjectTarget.NETStandard20] = new CSharpNETStandardProjectFileWriter(),
				[ProjectTarget.NativeAndroid] = new CSharpNETFrameworkProjectFileWriter(),
				[ProjectTarget.NativeiOS] = new CSharpNETFrameworkProjectFileWriter(),
				[ProjectTarget.NETFramework46] = new CSharpNETFrameworkProjectFileWriter()
			};
		}

		public FileWriter(Project project) : this()
		{
			_metadata = project.Metadata;
			_targetFramework = project.TargetFramework;
		}

		public bool UpdateNugetPackage(string packageName, string version)
		{
			var fileWriter = _fileWriters[_targetFramework];
			fileWriter.Initialize(_targetFramework, _metadata);
			fileWriter.UpdatePackageReference(packageName, version);

			return true;
		}
	}
}