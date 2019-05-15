namespace NautilusCLI.Abstraction
{
	public interface IConfigurationService
	{
		string GetPath();
		string[] GetExcludeFoldersPattern();
		string[] GetSearchPatterns();
	}
}