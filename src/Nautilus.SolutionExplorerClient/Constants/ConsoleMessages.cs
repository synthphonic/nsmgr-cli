namespace Nautilus.SolutionExplorerClient.Constants;

public static class ConsoleMessages
{
    private const string ErrorsHasOccurredMessage = "Error(s) has occurred.";
    private const string CheckLogMessage = "Detail of error is in the log file";

    internal static void DisplayProgramHasTerminatedMessage()
    {
        Colorful.Console.WriteLine("Program has terminated", Color.Red);
        Console.WriteLine("");
    }

    internal static void SolutionFileExceptionMessageFormat(SolutionFileException solutionFileEx)
    {
        Console.WriteLine("");
        Colorful.Console.WriteLine(solutionFileEx.Message, Color.Red);
        Console.WriteLine("");
    }

    internal static void DisplayProjectNotFoundMessageFormat(ProjectNotFoundException ex, string logFileName, bool debugMode = false)
    {
        Console.WriteLine("");
        Colorful.Console.WriteLine(ErrorsHasOccurredMessage, Color.Red);
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
        Colorful.Console.WriteLine($"execution time : {sw.Elapsed.TotalSeconds} secs\n", Color.GreenYellow);
    }

    internal static void DisplayCompletedSuccessfullyFinishingMessage()
    {
        Colorful.Console.WriteLine("\nCompleted successfully", Color.GreenYellow);
    }

    internal static void LogToFileIfDebugMode(ProjectNotFoundException ex, string logFileName, bool debugMode)
    {
        if (debugMode)
        {
            Colorful.Console.WriteLine(CheckLogMessage, Color.Red);

            LoggingManager.Instance.Initialize(logFileName, true);

            Colorful.Console.WriteLine(ex.Message, Color.Red);

            var baseEx = ex.GetBaseException();
            Colorful.Console.WriteLine(baseEx.StackTrace, Color.Red);

            LoggingManager.Instance.WriteError($"{ex.Message}\n{baseEx.StackTrace}");

            LoggingManager.Instance.Close();
        }
    }
}