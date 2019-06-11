using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nautilus.Cli.Core.Components.Http;
using Nautilus.Cli.Core.Models;

namespace Nautilus.Cli.Core.Components
{
	public class NugetPackageOnlineComparer
	{
		private readonly Solution _solution;
		private readonly Action _writeProgressHandler;

		public NugetPackageOnlineComparer(Solution solution, Action writeProgressHandler = null)
		{
			_solution = solution;
			_writeProgressHandler = writeProgressHandler;
			Result = new Dictionary<string, IList<NugetPackageInformationComparer>>();
		}

		public async Task Run()
		{
			foreach (var project in _solution.Projects)
			{
				await GetOnlinePackagesAsync(project);
			}
		}

		public Dictionary<string, IList<NugetPackageInformationComparer>> Result { get; private set; }

		private async Task GetOnlinePackagesAsync(Project project)
		{
			var projectPackages = new List<NugetPackageInformationComparer>();

			foreach (var package in project.Packages)
			{
				var requestor = NugetPackageHttpRequest.QueryRequest(package.PackageName, false);
				var response = await requestor.ExecuteAsync();
				projectPackages.Add(new NugetPackageInformationComparer(package, response.Data[0]));

				_writeProgressHandler();
			}

			Result[project.ProjectName] = projectPackages;
		}
	}
}