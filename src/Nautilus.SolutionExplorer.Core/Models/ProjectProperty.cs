namespace Nautilus.SolutionExplorer.Core.Models;

internal class ProjectProperty : IProjectElementValue
{
    private string _parentElement;
    private Project _project;

    public ProjectProperty(Project project)
    {
        _project = project;
    }

    public void Create(string elementName, string elementValue)
    {
        var fileWriter = new FileWriterContext(_project);
        fileWriter.AddOrUpdateElement(_parentElement, elementName, elementValue);
    }

    public void Remove(string elementName)
    {
        var fileWriter = new FileWriterContext(_project);
        fileWriter.DeleteElement(_parentElement, elementName);
    }

    internal IProjectElementValue SetParent(string parentElement)
    {
        _parentElement = parentElement;

        return this;
    }
}