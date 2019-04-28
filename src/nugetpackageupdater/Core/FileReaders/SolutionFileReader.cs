using System.Collections.Generic;
using System.IO;
using System.Text;
using SolutionNugetPackagesUpdater.Abstraction;

namespace SolutionNugetPackagesUpdater.Core.FileReaders
{
    public class SolutionFileReader : IFileReader
    {
        private string _file;

        public object Read(string file)
        {
            _file = file;

            return ReadSolutionFile();
        }

        private IEnumerable<string> ReadSolutionFile()
        {
            var fileContents = new List<string>();

            using (var sr = new StreamReader(_file,Encoding.UTF8))
            {
                while (string.IsNullOrEmpty(sr.ReadLine()))
                {
                }

                var line = sr.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    fileContents.Add(line);
                    line = sr.ReadLine();
                }

                sr.Close();
            }

            return fileContents;
        }
    }
}