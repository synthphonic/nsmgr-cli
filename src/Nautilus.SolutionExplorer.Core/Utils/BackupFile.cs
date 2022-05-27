namespace Nautilus.SolutionExplorer.Core.Utils;

public static class BackupFile
{
    public static void Execute(string filePath)
    {
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        var fileExtension = Path.GetExtension(filePath);
        var dirName = Path.GetDirectoryName(filePath);

        for (var i = 1; i < 100; i++)
        {
            var newFileName = $"{fileName}_nsmgr_copy_{i}.{fileExtension}";
            var newFilePath = Path.Combine(dirName, newFileName);
            if (!File.Exists(newFilePath))
            {
                File.Copy(filePath, newFilePath);
                return;
            }
        }
    }
}