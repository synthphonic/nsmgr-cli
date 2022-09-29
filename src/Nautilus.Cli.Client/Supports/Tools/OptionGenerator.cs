namespace Nautilus.Cli.Client.Supports.Tools;

internal class OptionGenerator
{
    internal static Option<T> CreateOption<T>(string name, string description, string alias, bool isRequired = false, bool allowMultipleArgumentsPerToken = false)
    {
        var option = new Option<T>(name: name, description: description) { IsRequired = isRequired, AllowMultipleArgumentsPerToken = allowMultipleArgumentsPerToken };
        option.AddAlias(alias);

        return option;
    }

    internal static CommonOption? Common { get; private set; } = new CommonOption();
}

internal class CommonOption
{
    internal Option<FileInfo> ProjectPathOption()
    {
        return OptionGenerator.CreateOption<FileInfo>("--project-path", "The project path", "-p", true);
    }

    internal Option<string> ProjectOption()
    {
        return OptionGenerator.CreateOption<string>("--project", "The project name", "-p", true);
    }

    internal Option<FileInfo> SolutionOption()
    {
        return OptionGenerator.CreateOption<FileInfo>("--solution", "The solution path", "-s", true);        
    }
}