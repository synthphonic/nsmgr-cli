using System.Collections.Generic;
using System.IO;
using SolutionNugetPackagesUpdater.Abstraction;

namespace SolutionNugetPackagesUpdater.Core.FileReaders
{
	public class SolutionFileReader : IFileReader
	{
		private string _file;

		public object Read(string file)
		{
			_file = file;

			ReadSolutionFile();

			return null;
		}

		private void ReadSolutionFile()
		{
			var fileName = Path.GetFileName(_file);

			var fileContents = new List<string>();
			using (var fs = File.OpenRead(_file))
			{
				using (var sr = new StreamReader(fs))
				{
					fileContents.Add(sr.ReadLine());
				}

				fs.Flush();
				fs.Close();
			}
		}
	}
}
