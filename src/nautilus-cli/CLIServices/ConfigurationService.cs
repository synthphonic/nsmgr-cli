using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using NautilusCLI.Abstraction;
using NautilusCLI.Core.Models;
using NautilusCLI.Core.Utils;

namespace NautilusCLI.CLIServices
{
    public class ConfigurationService : IConfigurationService
	{
		private readonly Assembly _asm;
		private Configuration _config;

		public ConfigurationService()
		{
			_asm = Assembly.GetExecutingAssembly();
		}

		public ConfigurationService(Assembly asm)
		{
			_asm = asm;
		}

		internal void CreateConfigFile()
		{
			var executingPath = FileUtil.GetFullPath(_asm);
			var configFile = Path.Combine(executingPath, Configuration.FileName);
			var configFileExists = FileUtil.IsFileExists(configFile);

			if (!configFileExists)
			{
				var excludeFolderPattern = new[] { "Sample1", "Sample2" };
				var searchPatterns = new[] { "*.csproj", "packages.config" };
				var config = new Configuration
				{
					ParentSearchPath = "/set/your/full/path/folder/here",
					SearchFilePatterns = searchPatterns,
					ExcludeFoldersPattern = excludeFolderPattern,
                    OutputFile = "/set/your/output/full/path/folder/here"
                };

				using (var stream = File.CreateText(configFile))
				{
					var json = JsonConvert.SerializeObject(config);
					stream.Write(json);
					stream.Flush();
					stream.Close();
				}
			}
		}

		public bool Validate()
		{
			var executingPath = FileUtil.GetFullPath(_asm);
			var configFile = Path.Combine(executingPath, Configuration.FileName);
			var configFileExists = FileUtil.IsFileExists(configFile);

			return configFileExists;
		}

		public void LoadConfiguration()
		{
			var executingPath = FileUtil.GetFullPath(_asm);
			var configFileName = Path.Combine(executingPath, Configuration.FileName);
			var json = File.ReadAllText(configFileName);

			_config = JsonConvert.DeserializeObject<Configuration>(json);
		}

		#region IConfigurationService implementation
        public string GetOutputFile()
        {
            return _config.OutputFile;
        }

        public string GetPath()
		{
			return _config.ParentSearchPath;
		}

		public string[] GetSearchPatterns()
		{
			return _config.SearchFilePatterns;
		}

		public string[] GetExcludeFoldersPattern()
		{
			return _config.ExcludeFoldersPattern;
		}

		#endregion
	}
}