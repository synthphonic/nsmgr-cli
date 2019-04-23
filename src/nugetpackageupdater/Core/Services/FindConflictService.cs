using SolutionNugetPackagesUpdater.Core.FileReaders;

namespace SolutionNugetPackagesUpdater.Core.Services
{
	public class FindConflictService
	{
		private string _fileName;

		public FindConflictService(string fileName)
		{
			_fileName = fileName;
		}

		public void Run()
		{
			var slnFileReader = new SolutionFileReader();
			slnFileReader.Read(_fileName);
		}
	}
}
