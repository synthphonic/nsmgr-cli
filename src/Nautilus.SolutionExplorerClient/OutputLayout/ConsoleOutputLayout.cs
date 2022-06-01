namespace Nautilus.SolutionExplorerClient.OutputLayout;

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

        DisplayProgramHasTerminatedMessage();
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

        DisplayProgramHasTerminatedMessage();
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

        DisplayProgramHasTerminatedMessage();
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

        DisplayProgramHasTerminatedMessage();
    }

    internal static void DisplayFinishingMessage(Stopwatch sw)
    {
        Colorful.Console.WriteLine($"\n{ConsoleMessageConstants.CompletedSuccessfully}", Color.GreenYellow);
        Colorful.Console.WriteLine($"execution time : {sw.Elapsed.TotalSeconds} secs\n", Color.GreenYellow);
    }

    internal static void DisplayProgramHasTerminatedMessage()
    {
        Colorful.Console.WriteLine($"{ConsoleMessageConstants.ProgramTerminated}", Color.Red);
        Console.WriteLine("");
    }

    internal static void SolutionFileExceptionMessageFormat(SolutionFileException solutionFileEx)
    {
        Console.WriteLine("");
        Colorful.Console.WriteLine(solutionFileEx.Message, Color.Red);
        Console.WriteLine("");

        DisplayProgramHasTerminatedMessage();
    }

    internal static void DisplayProjectNotFoundMessageFormat(ProjectNotFoundException ex, string logFileName, bool debugMode = false)
    {
        Console.WriteLine("");
        Colorful.Console.WriteLine(ConsoleMessageConstants.ErrorsHasOccurredM, Color.Red);
        Console.WriteLine("");

        LogToFileIfDebugMode(ex, logFileName, debugMode);
    }

    internal static void DisplayNugetPackageNotFoundMessageFormat(NugetPackageNotFoundException ex, string logFileName, bool debugMode = false)
    {
        Console.WriteLine("");
        Colorful.Console.WriteLine(ex.Message, Color.Red);
        Console.WriteLine("");

        if (debugMode)
        {
            LoggingManager.Instance.Initialize(logFileName, true);

            var baseEx = ex.GetBaseException();
            Colorful.Console.WriteLine(baseEx.StackTrace, Color.Red);

            LoggingManager.Instance.WriteError($"{baseEx.Message}\n{baseEx.StackTrace}");

            Console.WriteLine("");

            LoggingManager.Instance.Close();
        }
    }

    internal static void DisplayGeneralExceptionMessageFormat(Exception ex, string logFileName, bool debugMode = false)
    {
        Console.WriteLine("");
        Colorful.Console.WriteLine(ex.Message, Color.Red);
        Console.WriteLine("");

        if (debugMode)
        {
            LoggingManager.Instance.Initialize(logFileName, true);

            var baseEx = ex.GetBaseException();
            Colorful.Console.WriteLine(baseEx.StackTrace, Color.Red);

            LoggingManager.Instance.WriteError($"{baseEx.Message}\n{baseEx.StackTrace}");

            Console.WriteLine("");

            LoggingManager.Instance.Close();
        }
    }

    internal static void DisplayCLIExceptionMessageFormat(CLIException cliEx, string logFileName, bool debugMode = false)
    {
        Console.WriteLine("");
        Colorful.Console.WriteLine(cliEx.Message, Color.Red);
        Console.WriteLine("");

        if (debugMode)
        {
            LoggingManager.Instance.Initialize(logFileName, true);

            Colorful.Console.WriteLine(cliEx.StackTrace, Color.Red);

            LoggingManager.Instance.WriteError($"{cliEx.Message}\n{cliEx.StackTrace}");

            Console.WriteLine("");

            LoggingManager.Instance.Close();
        }
    }

    internal static void DisplayExecutionTimeMessage(Stopwatch sw)
    {
        Colorful.Console.WriteLine($"{ConsoleMessageConstants.ExecutionTime} {sw.Elapsed}",Color.Yellow);
    }

    internal static void DisplayCompletedSuccessfullyFinishingMessage()
    {
        Colorful.Console.WriteLine($"\n{ConsoleMessageConstants.CompletedSuccessfully}\n");
    }

    internal static void LogToFileIfDebugMode(ProjectNotFoundException ex, string logFileName, bool debugMode)
    {
        if (debugMode)
        {
            Colorful.Console.WriteLine(ConsoleMessageConstants.CheckLog, Color.Red);

            LoggingManager.Instance.Initialize(logFileName, true);

            Colorful.Console.WriteLine(ex.Message, Color.Red);

            var baseEx = ex.GetBaseException();
            Colorful.Console.WriteLine(baseEx.StackTrace, Color.Red);

            LoggingManager.Instance.WriteError($"{ex.Message}\n{baseEx.StackTrace}");

            LoggingManager.Instance.Close();
        }
    }
}