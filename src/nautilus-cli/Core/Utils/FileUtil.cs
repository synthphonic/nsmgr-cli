using System.IO;
using System.Reflection;

namespace NautilusCLI.Core.Utils
{
	public static class FileUtil
	{
		internal static string GetFileName(string fullFilePath)
		{
			return Path.GetFileName(fullFilePath);
		}

		internal static string GetFullPath(string fullFilePath)
		{
			var fullDirectory = Path.GetDirectoryName(fullFilePath);
			return fullDirectory;
		}

		internal static string GetFullPath(Assembly asm)
		{
			var fullDirectory = Path.GetDirectoryName(asm.Location);
			return fullDirectory;
		}

		internal static bool IsFileExists(string fileName)
		{
			return File.Exists(fileName);
		}

		internal static string PathCombine(string path1, string path2)
		{
			return Path.Combine(path1, path2);
		}
	}
}