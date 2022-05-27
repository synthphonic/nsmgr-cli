namespace Nautilus.SolutionExplorer.Core.Utils;

public static class FileUtil
{
    public static string GetFileName(string fullFilePath)
    {
        return Path.GetFileName(fullFilePath);
    }

    public static string GetFileNameWithoutExtension(string fullFilePath)
    {
        return Path.GetFileNameWithoutExtension(fullFilePath);
    }

    public static string GetFullPath(string fullFilePath)
    {
        var fullDirectory = Path.GetDirectoryName(fullFilePath);
        return fullDirectory;
    }

    //public static string GetFullPath(Assembly asm)
    //{
    //    var fullDirectory = Path.GetDirectoryName(asm.Location);
    //    return fullDirectory;
    //}

    public static string ReadFileContent(string fileName)
    {
        var content = string.Empty;
        using (var fs = File.OpenRead(fileName))
        {
            using (var sr = new StreamReader(fs))
            {
                content = sr.ReadToEnd();
            }
        }

        return content;
    }

    //public static bool IsFileExists(string fileName)
    //{
    //    return File.Exists(fileName);
    //}

    //public static string PathCombine(string path1, string path2)
    //{
    //    return Path.Combine(path1, path2);
    //}
}