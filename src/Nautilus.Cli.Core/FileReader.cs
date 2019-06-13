/*
 * ref: https://stackoverflow.com/questions/14110212/reading-specific-xml-elements-from-xml-file
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Nautilus.Cli.Core.Abstraction;
using Nautilus.Cli.Core.Configurations.Enums;
using Nautilus.Cli.Core.Exceptions;
using Nautilus.Cli.Core.FileReaders;
using Nautilus.Cli.Core.Models;
using Nautilus.Cli.Core.Utils;

namespace Nautilus.Cli.Core
{
	public class FileReader
	{
		private readonly Dictionary<ProjectTarget, IProjectFilePackageReader> _fileReaders;
		private readonly ProjectMetadata _metadata;
		private readonly ProjectTarget _targetFramework;
		private readonly bool _packagesConfigFileExist;

		public FileReader(Project project) : this()
		{
			_metadata = project.Metadata;
			_targetFramework = project.TargetFramework;
			_packagesConfigFileExist = project.PackagesConfigFileExist;
		}

		internal FileReader(ProjectTarget targetFramework, ProjectMetadata projectMetadata) : this()
		{
			_metadata = projectMetadata;
			_targetFramework = targetFramework;
		}

		private FileReader()
		{
			_fileReaders = new Dictionary<ProjectTarget, IProjectFilePackageReader>
			{
				//[SolutionProjectElement.PackageConfig] = new PackageConfigFileReader(),
				//[SolutionProjectElement.iOS] = new CSharpNativeProjectFileReader(),
				//[SolutionProjectElement.Android] = new CSharpNativeProjectFileReader()
				[ProjectTarget.NETFramework] = new CSharpNETFrameworkProjectFileReader(),
				[ProjectTarget.NETFramework46] = new PackageConfigFileReader(),
				[ProjectTarget.NETStandard20] = new CSharpProjectFileReader(),
				[ProjectTarget.NETCoreApp20] = new CSharpProjectFileReader(),
				[ProjectTarget.NETCoreApp21] = new CSharpProjectFileReader(),
				[ProjectTarget.NativeiOS] = new CSharpNETFrameworkProjectFileReader(),
				[ProjectTarget.NativeiOSBinding] = new CSharpNETFrameworkProjectFileReader(),
				[ProjectTarget.NativeAndroid] = new CSharpNETFrameworkProjectFileReader()
			};
		}

		public object ReadFile()
		{
			//
			// SOME NOTES:
			// for ProjectTarget.NETFramework46, we need to check the following:
			// 1. check if packages.json file exists. if yes, then read from it
			// 2. if packages.json does not exists, then read from C# project file
			//

			try
			{
				var packageConfigExists = false;

				object returnedObject = null;

				if (_targetFramework == ProjectTarget.NETFramework46)
				{
					var projectFileName = _metadata.ProjectFullPath;
					var packageConfigFile = Path.Combine(Path.GetDirectoryName(projectFileName), "packages.config");
					packageConfigExists = File.Exists(packageConfigFile);

					returnedObject = packageConfigExists
						? _fileReaders[_targetFramework].Read(_metadata.ProjectFullPath)
						: _fileReaders[ProjectTarget.NETFramework].Read(_metadata.ProjectFullPath);
				}
				else
				{
					returnedObject = _fileReaders[_targetFramework].Read(_metadata.ProjectFullPath);
				}

				return returnedObject;
			}
			catch (KeyNotFoundException keyNotFoundEx)
			{
				var exceptionMessage = new StringBuilder();
				exceptionMessage.AppendFormat($"File cannot be read for '{_metadata.ProjectFileName}'\n");
				exceptionMessage.AppendFormat($"Reason:{keyNotFoundEx.Message}");
				throw new CLIException(exceptionMessage.ToString(), keyNotFoundEx);

			}
			catch (FileNotFoundException fileNotFoundEx)
			{
				var exceptionMessage = new StringBuilder();
				exceptionMessage.AppendFormat($"This error shouldn't have happened! Something went seriously wrong at '{_metadata.ProjectFileName}'\n");
				exceptionMessage.AppendFormat($"Reason:{fileNotFoundEx.Message}");
				throw new CLIException(exceptionMessage.ToString(), fileNotFoundEx);
			}
			catch (Exception ex)
			{
				var exceptionMessage = new StringBuilder();
				exceptionMessage.AppendFormat($"This error shouldn't have happened! Something went seriously wrong at '{_metadata.ProjectFileName}'\n");
				exceptionMessage.AppendFormat($"Reason:{ex.Message}");
				throw new CLIException(exceptionMessage.ToString(), ex);
			}
		}

		public bool TryGetPackageVersion(string packageName, out string version)
		{
			version = string.Empty;

			try
			{
				IList<NugetPackageReference> packageReferences = null;
				NugetPackageReference found = null;

				if (_packagesConfigFileExist)
				{
					packageReferences = _fileReaders[_targetFramework].Read(_metadata.ProjectFullPath) as IList<NugetPackageReference>;
					found = packageReferences.FirstOrDefault(x => x.PackageName.Equals(packageName));
					version = found.Version;

					return !string.IsNullOrWhiteSpace(found.Version);
				}

				packageReferences = _fileReaders[ProjectTarget.NETFramework].Read(_metadata.ProjectFullPath) as IList<NugetPackageReference>;
				found = packageReferences.FirstOrDefault(x => x.PackageName.Equals(packageName));
				version = found.Version;

				return !string.IsNullOrWhiteSpace(found.Version);
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