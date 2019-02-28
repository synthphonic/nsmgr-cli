using System.Drawing;
using Colorful;
using SolutionNugetPackagesUpdater.Services;

namespace SolutionNugetPackagesUpdater
{
	class Program
	{
		static void Main(string[] args)
		{
			var configService = new ConfigurationService();

			if (args.Length == 0)
			{
				if (!configService.Validate())
				{
					Console.WriteLine("app_config.json is not yet created", Color.Red);
					return;
				}

				configService.LoadConfiguration();
                using (var finder = new FinderService(configService))
                {
                    finder.Execute();
                }

				// process here
			}
			else
			{
				if (args[0].ToLower().Equals("--createappconfig"))
				{
					configService.CreateConfigFile();
					Console.WriteLine("app_config.json file created", Color.AliceBlue);
				}

				return;
			}
		}
	}
}