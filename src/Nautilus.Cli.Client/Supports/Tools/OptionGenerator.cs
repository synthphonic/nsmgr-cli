namespace Nautilus.Cli.Client.Supports.Tools;

internal static class OptionGenerator
{
    internal static Option<T> CreateOption<T>(string name, string description, string alias, bool isRequired = false, bool allowMultipleArgumentsPerToken = false)
    {
        var option = new Option<T>(name: name, description: description) { IsRequired = isRequired, AllowMultipleArgumentsPerToken = allowMultipleArgumentsPerToken };
        option.AddAlias(alias);

        return option;
    }
}