namespace Nautilus.SolutionExplorer.Core.Abstraction
{
	public interface IConfigurationService
	{
		string GetPath();
		string[] GetExcludeFoldersPattern();
		string[] GetSearchPatterns();
	}
}