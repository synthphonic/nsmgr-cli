/*
 * ref: https://stackoverflow.com/questions/14110212/reading-specific-xml-elements-from-xml-file
 */

using System;
using System.Collections.Generic;
using System.Text;
using SolutionNugetPackagesUpdater.Abstraction;
using SolutionNugetPackagesUpdater.Core.Configurations.Enums;
using SolutionNugetPackagesUpdater.Core.Exceptions;
using SolutionNugetPackagesUpdater.Core.FileReaders;
using SolutionNugetPackagesUpdater.Core.Models;

namespace SolutionNugetPackagesUpdater.Core
{
	public class FileReader
	{
		//private readonly string _file;
		private readonly Dictionary<ProjectTarget, IProjectFilePackageReader> _fileReaders;
		private ProjectMetadata _metadata;
		private ProjectTarget _targetFramework;

		public FileReader(ProjectTarget targetFramework, ProjectMetadata projectMetadata) : this()
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
				[ProjectTarget.NETFramework46] = new PackageConfigFileReader(),
				[ProjectTarget.NETStandard20] = new CSharpProjectFileReader(),
				[ProjectTarget.NETCoreApp20] = new CSharpProjectFileReader(),
				[ProjectTarget.NETCoreApp21] = new CSharpProjectFileReader(),
				[ProjectTarget.NativeiOS] = new CSharpNativeProjectFileReader(),
				[ProjectTarget.NativeiOSBinding] = new CSharpNativeProjectFileReader(),
				[ProjectTarget.NativeAndroid] = new CSharpNativeProjectFileReader()
			};
		}

		public object ReadFile()
		{
			try
			{
				var returnedObject = _fileReaders[_targetFramework].Read(_metadata.ProjectFullPath);

				return returnedObject;
			}
			catch (KeyNotFoundException keyNotFoundEx)
			{
				var exceptionMessage = new StringBuilder();
				exceptionMessage.AppendFormat($"File cannot be read for '{_metadata.ProjectFileName}'\n");
				exceptionMessage.AppendFormat($"Reason:{keyNotFoundEx.Message}");
				throw new CLIException(exceptionMessage.ToString(), keyNotFoundEx);

			}
			catch (Exception ex)
			{
				var exceptionMessage = new StringBuilder();
				exceptionMessage.AppendFormat($"This error shouldn't have happened! Something went seriously wrong at '{_metadata.ProjectFileName}'\n");
				exceptionMessage.AppendFormat($"Reason:{ex.Message}");
				throw new CLIException(exceptionMessage.ToString(), ex);
			}
		}
	}
}