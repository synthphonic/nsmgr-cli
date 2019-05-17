namespace Nautilus.Cli.Core.Abstraction
{
	public interface IConfigurationService
	{
		string GetPath();
		string[] GetExcludeFoldersPattern();
		string[] GetSearchPatterns();
	}
}