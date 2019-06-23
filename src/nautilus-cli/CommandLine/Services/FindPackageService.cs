using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Nautilus.Cli.Client.CommandLine.Layout;
using Nautilus.Cli.Core.Extensions;
using Nautilus.Cli.Core.FileReaders;
using Nautilus.Cli.Core.Models;

namespace Nautilus.Cli.Client.CommandLine.Services
{
    public class FindPackageService
    {
        private readonly string _solutionFileName;
        private readonly string _packageName;
        private readonly bool _returnResults;

        public FindPackageService(string solutionFileName, string packageName, bool returnResults = false)
        {
            _solutionFileName = solutionFileName;
            _packageName = packageName;
            _returnResults = returnResults;
        }

		public async Task Run()
		{
			Colorful.Console.WriteLine();
			Colorful.Console.WriteLine("Working. Please wait...", Color.GreenYellow);

            var slnFileReader = new SolutionFileReader(_solutionFileName, true);
			var solution = slnFileReader.Read();

            var flatList = solution.ExtractNugetPackageAsFlatList();
            var categorizedByPackageNameList = solution.CategorizeByPackageName(flatList);

            await Task.Delay(1);

            Colorful.Console.WriteLine();

            var projects = categorizedByPackageNameList[_packageName];

            if (_returnResults)
            {
                Results = projects;
            }
            else
            {
                WriteOutput(projects, solution.SolutionFileName);
            }
        }

        private void WriteOutput(IList<NugetPackageReferenceExtended> projects, string solutionFileName)
        {
            Colorful.Console.WriteLine();
            Colorful.Console.Write("{0,-15}", "Solution ");
            Colorful.Console.WriteLine($": {solutionFileName}", Color.PapayaWhip);

            Colorful.Console.WriteLine();

            Colorful.Console.WriteLine($"{_packageName}", Color.Aqua);

            foreach (var project in projects)
            {
                Colorful.Console.Write($"In Project ");
                Colorful.Console.Write(CliStringFormatter.Format40, Color.Azure, project.ProjectName);
                Colorful.Console.Write("[{0,-16}]", Color.Azure, project.ProjectTargetFramework);
                Colorful.Console.Write(" found version ");
                Colorful.Console.Write("{0}", Color.Azure, project.Version);
                Colorful.Console.WriteLine();
            }
        }

        public IList<NugetPackageReferenceExtended> Results { get; private set; }
    }
}
