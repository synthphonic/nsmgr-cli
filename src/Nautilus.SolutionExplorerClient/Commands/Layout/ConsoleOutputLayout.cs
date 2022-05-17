namespace Nautilus.SolutionExplorerClient.Commands.Layout;

internal static class ConsoleOutputLayout
{
    internal static void DisplayProjectNotFoundMessageFormat(ProjectNotFoundException ex, bool debugMode = false)
    {
        Console.WriteLine("");
        Colorful.Console.WriteLine(ex.Message, Color.Red);
        Console.WriteLine("");

        if (debugMode)
        {
            var baseEx = ex.GetBaseException();
            Colorful.Console.WriteLine(baseEx.StackTrace, Color.Red);
            Console.WriteLine("");
        }

        Colorful.Console.WriteLine(ConsoleMessages.ProgramTerminated, Color.Red);
        Console.WriteLine("");
    }

    internal static void DisplayNugetPackageNotFoundMessageFormat(NugetPackageNotFoundException ex, bool debugMode = false)
    {
        Console.WriteLine("");
        Colorful.Console.WriteLine(ex.Message, Color.Red);
        Console.WriteLine("");

        if (debugMode)
        {
            var baseEx = ex.GetBaseException();
            Colorful.Console.WriteLine(baseEx.StackTrace, Color.Red);
            Console.WriteLine("");
        }

        Colorful.Console.WriteLine(ConsoleMessages.ProgramTerminated, Color.Red);
        Console.WriteLine("");
    }

    internal static void DisplayGeneralExceptionMessageFormat(Exception ex, bool debugMode = false)
    {
        Console.WriteLine("");
        Colorful.Console.WriteLine(ex.Message, Color.Red);
        Console.WriteLine("");

        if (debugMode)
        {
            var baseEx = ex.GetBaseException();
            Colorful.Console.WriteLine(baseEx.StackTrace, Color.Red);
            Console.WriteLine("");
        }

        Colorful.Console.WriteLine(ConsoleMessages.ProgramTerminated, Color.Red);
        Console.WriteLine("");
    }

    internal static void DisplayCLIExceptionMessageFormat(CLIException cliEx, bool debugMode = false)
    {
        Console.WriteLine("");
        Colorful.Console.WriteLine(cliEx.Message, Color.Red);
        Console.WriteLine("");

        if (debugMode)
        {
            Colorful.Console.WriteLine(cliEx.StackTrace, Color.Red);
            Console.WriteLine("");
        }

        Colorful.Console.WriteLine(ConsoleMessages.ProgramTerminated, Color.Red);
        Console.WriteLine("");
    }

    internal static void SolutionFileExceptionMessageFormat(SolutionFileException solutionFileEx)
    {
        Console.WriteLine("");
        Colorful.Console.WriteLine(solutionFileEx.Message, Color.Red);
        Console.WriteLine("");
        Colorful.Console.WriteLine(ConsoleMessages.ProgramTerminated, Color.Red);
        Console.WriteLine("");
    }

    internal static void DisplayFinishingMessage(Stopwatch sw)
    {
        Colorful.Console.WriteLine("\nCompleted successfully", Color.GreenYellow);
        Colorful.Console.WriteLine($"execution time : {sw.Elapsed.TotalSeconds} secs\n", Color.GreenYellow);
    }
}