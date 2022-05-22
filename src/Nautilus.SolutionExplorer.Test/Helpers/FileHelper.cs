namespace Nautilus.SolutionExplorer.Test.Helpers;

public static class FileHelper
{
    /// <summary>
    /// Deletes the file given in the parameter
    /// </summary>
    /// <param name="fullFileName"></param>
    /// <returns>Returns true if file is successful deleted, else returns false</returns>
    internal static bool DeleteFile(string fullFileName)
    {
        if (File.Exists(fullFileName))
        {
            File.Delete(fullFileName);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Gets the desktop folder full filename
    /// </summary>
    /// <param name="logFileName"></param>
    /// <returns></returns>
    internal static string GetDesktopFullFileName(string logFileName)
    {
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var fullFilePath = Path.Combine(desktopPath, logFileName);
        return fullFilePath;
    }

}
