/*
 * Reference: https://stackoverflow.com/questions/3754118/how-to-filter-directory-enumeratefiles-with-multiple-criteria
 * Added by Shah Z. S
 * Added on : 24 Feb 2019
 */

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace NautilusCLI.Core.Utils
{
	public static class DirectoryUtil
	{
		/// <summary>
		/// Gets the files with regex version
		/// </summary>
		/// <returns>The files.</returns>
		/// <param name="path">Path.</param>
		/// <param name="searchPatternExpression">Search pattern expression.</param>
		/// <param name="searchOption">Search option.</param>
		public static IEnumerable<string> GetFiles(string path, 
			string searchPatternExpression = "", 
			SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			Regex reSearchPattern = new Regex(searchPatternExpression, RegexOptions.IgnoreCase);
			return Directory.EnumerateFiles(path, "*", searchOption)
							.Where(file =>
									 reSearchPattern.IsMatch(Path.GetExtension(file)));
		}

		/// <summary>
		/// Gets the files. Takes same patterns, and executes in parallel
		/// </summary>
		/// <returns>The files.</returns>
		/// <param name="path">Path.</param>
		/// <param name="searchPatterns">Search patterns.</param>
		/// <param name="excludeFoldersPattern">Excludes folders with the patterns enlisted</param>
		/// <param name="searchOption">Search option.</param>
		public static IEnumerable<string> GetFiles(string path,
			string[] searchPatterns,
			string[] excludeFoldersPattern,
			SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{

			var excludePathFound = false;
			var files = GetFiles(path, searchPatterns, searchOption);
			var fileCount = files.Count();
			var a = files.ToList();

			var newfileList = new List<string>();

			var fileEnumerator = files.GetEnumerator();
			while (fileEnumerator.MoveNext())
			{
				excludePathFound = false;
				foreach (var excludePattern in excludeFoldersPattern)
				{
					var isExists = fileEnumerator.Current.Contains(excludePattern);
					if (isExists)
					{
						excludePathFound = true;
						break;
					}
				}

				if (!excludePathFound)
				{
					newfileList.Add(fileEnumerator.Current);
				}
			}

			return newfileList;
		}

		/// <summary>
		/// Gets the files. Takes same patterns, and executes in parallel
		/// </summary>
		/// <returns>The files.</returns>
		/// <param name="path">Path.</param>
		/// <param name="searchPatterns">Search patterns.</param>
		/// <param name="searchOption">Search option.</param>
		public static IEnumerable<string> GetFiles(string path, 
			string[] searchPatterns, 
			SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			return searchPatterns.AsParallel()
				   .SelectMany(searchPattern =>
						  Directory.EnumerateFiles(path, searchPattern, searchOption));
		}
	}
}