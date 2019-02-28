using System.IO;
using System.Reflection;

namespace SolutionNugetPackagesUpdater.Utils
{
	public static class FileUtil
	{
		internal static string GetFullPath(Assembly asm)
		{
			var fullDirectory = Path.GetDirectoryName(asm.Location);
			return fullDirectory;
		}

		internal static bool IsFileExists(string fileName)
		{
			return File.Exists(fileName);
		}
	}
}