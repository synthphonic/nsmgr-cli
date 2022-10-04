namespace Nautilus.SolutionExplorer.Core.FileReaders;

public class SolutionFileReader
{
    private readonly bool _processProjectsOnly;
    private readonly FileInfo _solutionFile;
    private readonly IList<ProjectMetadata> _projectMetadataList;

    public SolutionFileReader(FileInfo solutionFile, bool processProjectsOnly = false)
    {
        _solutionFile = solutionFile;
        _processProjectsOnly = processProjectsOnly;

        _projectMetadataList = new List<ProjectMetadata>();
    }

    public Solution Read()
    {
        if (!_solutionFile.Exists)
        {
            throw new SolutionFileException($"Solution file '{_solutionFile.FullName}' is invalid", "solutionFileName");
        }

        var solutionFileContents = ReadSolutionFile();

        var f = new List<string>(solutionFileContents);
        var searchResults = f.Where(x => x.Contains("Project(")).ToList();

        foreach (var item in searchResults)
        {
            ExtractProjectMetadata(item, _solutionFile.FullName);
        }

        var solution = new Solution(_solutionFile);

        foreach (var metadata in _projectMetadataList)
        {
            var project = new Project(metadata);
            project.Read();
            solution.AddProject(project);
        }

        solution.Done();

        return solution;
    }

    private void ExtractProjectMetadata(string data, string fileName)
    {
        var projectMetadata = ProjectMetadataExtractor.Extract(data, fileName);

        if (_processProjectsOnly)
        {
            if (projectMetadata.ProjectType != SolutionProjectElement.VirtualFolder)
                _projectMetadataList.Add(projectMetadata);
        }
        else
        {
            _projectMetadataList.Add(projectMetadata);
        }
    }

    private IEnumerable<string> ReadSolutionFile()
    {
        var fileContents = new List<string>();

        using (var sr = new StreamReader(_solutionFile.FullName, Encoding.UTF8))
        {
            while (string.IsNullOrEmpty(sr.ReadLine()))
            {
            }

            var line = sr.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                fileContents.Add(line);
                line = sr.ReadLine();
            }

            sr.Close();
        }

        return fileContents;
    }
}
