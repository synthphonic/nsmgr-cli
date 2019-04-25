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

        private IEnumerable<string> ReadSolutionFile2()
        {
            var fileName = Path.GetFileName(_file);

            var fileContents = new List<string>();
            using (var fs = File.OpenRead(_file))
            {
                using (var sr = new StreamReader(fs))
                {
                    var s = sr.ReadLine();
                    s = sr.ReadLine();
                    while (!string.IsNullOrWhiteSpace(s))
                    {
                        fileContents.Add(s);
                        s = sr.ReadLine();
                    }
                    
                    sr.Close();
                }

                fs.Close();
            }

            return fileContents;
        }
    }
}