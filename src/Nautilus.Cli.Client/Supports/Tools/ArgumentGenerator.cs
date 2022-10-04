namespace Nautilus.Cli.Client.Supports.Tools;

internal static class ArgumentGenerator
{
    internal static Argument<T> CreateArgument<T>(string name, string description, bool isHidden = false)
    {
        var argument = new Argument<T>
        {
            Name = name,
            Description = description,
            IsHidden = isHidden
        };

        return argument;
    }

    internal static CommonArgument? Common { get; private set; } = new CommonArgument();
}

internal class CommonArgument
{
    internal Argument<string> VersionArgument()
    {
        return ArgumentGenerator.CreateArgument<string>("version", "The version number to use");
    }
}