namespace Nautilus.Cli.Client.Supports.Tools;

internal class OptionGenerator
{
    /// <summary>
    /// Generic method to create an option
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="defaultValue"></param>
    /// <param name="alias"></param>
    /// <param name="isRequired"></param>
    /// <param name="allowMultipleArgumentsPerToken"></param>
    /// <param name="isHidden"></param>
    /// <returns></returns>
    internal static Option<T> CreateOption<T>(string name, string description, string alias = "", bool isRequired = false, bool allowMultipleArgumentsPerToken = false, bool isHidden = false)
    {
        var option = new Option<T>(name: name, description: description) { IsRequired = isRequired, AllowMultipleArgumentsPerToken = allowMultipleArgumentsPerToken, IsHidden = isHidden };

        if (!string.IsNullOrWhiteSpace(alias))
        {
            option.AddAlias(alias);
        }

        return option;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="defaultValue"></param>
    /// <param name="alias"></param>
    /// <param name="isRequired"></param>
    /// <param name="allowMultipleArgumentsPerToken"></param>
    /// <param name="isHidden"></param>
    /// <returns></returns>
    internal static Option<T> CreateOption<T>(string name, string description, T defaultValue, string alias = "", bool isRequired = false, bool allowMultipleArgumentsPerToken = false, bool isHidden = false)
    {
        var option = CreateOption<T>(name, description, alias, isRequired, allowMultipleArgumentsPerToken, isHidden);
        option.SetDefaultValue(defaultValue);

        return option;
    }

    internal static CommonOption? Common { get; private set; } = new CommonOption();
}

internal class CommonOption
{
    /// <summary>
    /// The full path of the project name option
    /// </summary>
    /// <returns></returns>
    internal Option<FileInfo> ProjectPathOption()
    {
        return OptionGenerator.CreateOption<FileInfo>("--project-path", "The project path", "-p", isRequired: true);
    }

    /// <summary>
    /// The .csproj project file name
    /// </summary>
    /// <returns></returns>
    internal Option<string> ProjectNameOption()
    {
        return OptionGenerator.CreateOption<string>(name: "--project",description: "The project name", alias: "-p", isRequired: true);
    }

    /// <summary>
    /// The full path of the solution file name option
    /// </summary>
    /// <returns></returns>
    internal Option<FileInfo> SolutionPathOption()
    {
        return OptionGenerator.CreateOption<FileInfo>("--solution", "The solution path", "-s", isRequired: true);
    }
}