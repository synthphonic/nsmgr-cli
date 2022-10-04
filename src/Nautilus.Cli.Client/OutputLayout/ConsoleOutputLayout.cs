namespace Nautilus.SolutionExplorerClient.OutputLayout;

internal static class ConsoleOutputLayout
{
    internal static void DisplayExceptionMessageFormat<TException>(TException ex, string logFileName = "", bool showFullError = false)
        where TException : Exception, new()
    {
        Colorful.Console.Write($"[ERR] ", Color.Red);
        Colorful.Console.WriteLine($"{ex.Message}");

        Console.WriteLine("");

        if (showFullError)
        {
            Colorful.Console.WriteLine(ex.StackTrace, Color.Red);
            Console.WriteLine("");
        }

        if (!string.IsNullOrWhiteSpace(logFileName) && File.Exists(logFileName))
        {
            LogToFileIfDebugMode<TException>(ex, logFileName, showFullError);
        }

        DisplayProgramHasTerminatedMessage();
    }

    internal static void LogToFileIfDebugMode<TException>(TException ex, string logFileName, bool showFullError) where TException : Exception, new()
    {
        if (showFullError)
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

    internal static void DisplayFinishingMessage(Stopwatch sw)
    {
        Colorful.Console.WriteLine($"\n{ConsoleMessageConstants.CompletedSuccessfully}", Color.GreenYellow);
        Colorful.Console.WriteLine($"execution time : {sw.Elapsed.TotalSeconds} secs\n", Color.GreenYellow);
    }

    internal static void DisplayExecutionTimeMessage(Stopwatch sw)
    {
        Colorful.Console.WriteLine($"{ConsoleMessageConstants.ExecutionTime} {sw.Elapsed}", Color.Yellow);
    }

    internal static void DisplayProgramHasTerminatedMessage()
    {
        Colorful.Console.WriteLine($"{ConsoleMessageConstants.ProgramTerminated}", Color.Red);
        Console.WriteLine("");
    }

    internal static void DisplayCompletedSuccessfullyFinishingMessage()
    {
        Colorful.Console.WriteLine($"\n{ConsoleMessageConstants.CompletedSuccessfully}\n");
    }
}