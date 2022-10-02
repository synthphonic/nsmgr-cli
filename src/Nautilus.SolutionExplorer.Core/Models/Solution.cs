namespace Nautilus.SolutionExplorer.Core.Models;

public class Solution
{
    private IList<Project> _projects;

    public Solution(FileInfo solutionFile)
    {
        //SolutionFullPath = solutionFullPath;
        //SolutionFileName = FileUtil.GetFileName(solutionFullPath);
        _projects = new List<Project>();
        SolutionFile = solutionFile;
    }

    internal void Done()
    {
        Projects = new ReadOnlyCollection<Project>(_projects);
    }

    internal void AddProject(Project project)
    {
        _projects.Add(project);
    }

    //public string SolutionFileName { get; private set; }
    //public string SolutionFullPath { get; private set; }
    public ReadOnlyCollection<Project> Projects { get; private set; }
    public FileInfo SolutionFile { get; }
}
