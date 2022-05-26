namespace Nautilus.SolutionExplorer.Core.Utils;

public static class RestoreFile
{
    public static void Execute(string filePath)
    {
        var originalFileName = Path.GetFileNameWithoutExtension(filePath);
        var fileExtension = Path.GetExtension(filePath);
        var dirName = Path.GetDirectoryName(filePath);

        //
        // Assumptions for restoring back to original csproj file
        //      1. the biggest index number is the latest file
        //      2. the backup copu filename was not manually changed by the user.
        //
        for (var i = 99; i >= 1; i--)
        {
            //var backupFileName = $"{fileName}_nsmgr_copy_{i}";
            var currentBackupFile = $"{originalFileName}_nsmgr_copy_{i}.{fileExtension}";
            var currentBackupFilePath = Path.Combine(dirName, currentBackupFile);
            if (File.Exists(currentBackupFilePath))
            {
                File.Delete(filePath);
                File.Copy(currentBackupFilePath, filePath);
                File.Delete(currentBackupFilePath);

                return;
            }
        }
    }
}