using System.Collections.Generic;
using System.Linq;
using SolutionNugetPackagesUpdater.Core.FileReaders;
using SolutionNugetPackagesUpdater.Core.Models;

namespace SolutionNugetPackagesUpdater.Core.Services
{
    public class FindConflictService
    {
        private string _fileName;
        private IList<SolutionProjectInfo> _spiList;

        public FindConflictService(string fileName)
        {
            _fileName = fileName;
            _spiList = new List<SolutionProjectInfo>();
        }

        public void Run()
        {
            var slnFileReader = new SolutionFileReader();
            var fileContents = slnFileReader.Read(_fileName) as IEnumerable<string>;

            var f = new List<string>(fileContents);
            var searchResults = f.Where(x => x.Contains("Project(")).ToList();

            foreach (var item in searchResults)
            {
                Extract(item);
            }
        }

        private void Extract(string data)
        {
            var spi = SolutionProjectInfo.Extract(data);
            _spiList.Add(spi);
        }
    }
}